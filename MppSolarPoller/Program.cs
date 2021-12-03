using HidSharp;
using HidSharp.Utility;
using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using System.Diagnostics;
using System.Configuration;
using MppSolarPoller.Common;
using System.Reflection;

namespace mpp_solar_poller
{
    class Program
    {
        private static MqttClient Client;
        private static readonly string Topic = "homeassistant/sensor/";
        private static bool TopicRegistered = false;
        private static string RawInputCommand;
        private static int Interval = 5;
        private static string MqttServer;
        private static string ClientId;

        static void Main(string[] args)
        {
            HidSharpDiagnostics.EnableTracing = true;
            HidSharpDiagnostics.PerformStrictChecks = true;
            var list = DeviceList.Local;


            GetConfigurationParameter();


            Client = new MqttClient(MqttServer);
            Client.Connect(ClientId);
            Client.ConnectionClosed += Client_ConnectionClosed;

            Client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            Client.Subscribe(new string[] { $"{Topic}{Client.ClientId}" }, new byte[] { uPLibrary.Networking.M2Mqtt.Messages.MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            Console.WriteLine($"Subscribed to {Topic}{Client.ClientId}");


            var hidDevice = list.GetHidDevices().FirstOrDefault(c => c.DevicePath.EndsWith("hidraw0"));
            Console.WriteLine($"Found : {hidDevice.GetFriendlyName()}");

            HidStream hidStream;
            if (hidDevice.TryOpen(out hidStream))
            {
                Console.WriteLine("Opened device.");
                hidStream.ReadTimeout = Timeout.Infinite;
                
                QGMNCommand cmd = new QGMNCommand();
                if (args.Length == 1 ||System.Diagnostics.Debugger.IsAttached)
                    cmd.ModelName = "TestModel";
                else 
                    cmd.ProcessCommand(hidStream);
                var modelAssembly = Assembly.LoadFrom(cmd.ModelName);

                List<ICommand> commands = modelAssembly.GetExportedTypes().Where(c => c.BaseType == typeof(Command))
                    .Select(c => { var cmd = Activator.CreateInstance(c) as ICommand; cmd.PropertyChanged += Cmd_PropertyChanged; return cmd; }).ToList();

                

                Stopwatch stopwatch = new Stopwatch();

                using (hidStream)
                {
                    
                    while (true)
                    {
                        stopwatch.Restart();
                        if (!string.IsNullOrEmpty(RawInputCommand))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Executing Raw command : {RawInputCommand}");
                            (new InputCommand() { CommandName = RawInputCommand, ResponseSize = 5 }).ProcessCommand(hidStream);
                            RawInputCommand = null;
                            Console.ResetColor();
                        }

                        foreach (var command in commands)
                        {
                            if (!TopicRegistered)
                            {
                                RegisterTopics(command);
                            }
                            try { 
                            command.ProcessCommand(hidStream);
                            hidStream.Flush();
                            }
                            catch(Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine($"Error processing command {command.GetType()}");
                                Console.WriteLine(e);
                                Console.ResetColor();
                            }
                            //command.Parse("(235.1 49.9 235.1 49.9 0446 0347 008 367 01.00 000 000 0039 00.0 000.0 00.00 00000 00010000 00 00 00000 010??");
                            //Console.WriteLine(command.ToString());
                        }
                        var loopTime = stopwatch.ElapsedMilliseconds;
                        TopicRegistered = true;
                        Console.WriteLine("End of commands list");
                        System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(Interval).TotalMilliseconds - loopTime));
                    }
                }
            }
        }

        private static void GetConfigurationParameter()
        {
            var intervalString = ConfigurationManager.AppSettings["Interval"];
            if (int.TryParse(intervalString, out Interval))
            {
                Console.WriteLine("Unable to get Interval from configuration, using default value");
            }
            MqttServer = ConfigurationManager.AppSettings["mqttServer"];
            ClientId = ConfigurationManager.AppSettings["ClientId"];
        }

        private static void Client_ConnectionClosed(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("MQTT Disconnected");
            while(!Client.IsConnected)
            {
                try { 
                Client.Connect(ClientId);
                }
                catch
                { }
                System.Threading.Thread.Sleep(5000);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("MQTT Reconnected");
            Console.ResetColor();
        }




        /*
         * DESCRIPTION:                PAYLOAD:  OPTIONS:
----------------------------------------------------------------
Set output source priority  POP00     (Utility first)
                            POP01     (Solar first)
                            POP02     (SBU)

Set charger priority        PCP00     (Utility first)
                            PCP01     (Solar first)
                            PCP02     (Solar and utility)
                            PCP03     (Solar only)

Set the Charge/Discharge Levels & Cutoff
                            PBDV26.9  (Don't discharge the battery unless it is at 26.9v or more)
                            PBCV24.8  (Switch back to 'grid' when battery below 24.8v)
                            PBFT27.1  (Set the 'float voltage' to 27.1v)
                            PCVV28.1  (Set the 'charge voltage' to 28.1v)

Set other commands          PEa / PDa (Enable/disable buzzer)
                            PEb / PDb (Enable/disable overload bypass)
                            PEj / PDj (Enable/disable power saving)
                            PEu / PDu (Enable/disable overload restart);
                            PEx / PDx (Enable/disable backlight)
         */

        private static void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var command = System.Text.Encoding.UTF8.GetString(e.Message);
            Console.WriteLine($"Received {command} on {e.Topic}");

            if (command == "SendTopic")
                TopicRegistered = false;
            else
            {
                RawInputCommand = System.Text.Encoding.UTF8.GetString(e.Message);
            }
            Console.ResetColor();
        }

        private static bool RegisterTopics(object command)
        {
            Console.WriteLine($"Topics for for {command.GetType()}");
            foreach (var prop in command.GetType().GetProperties())
            {
                var attrib = prop.GetCustomAttributes(typeof(TopicConfigAttribute), false).FirstOrDefault() as TopicConfigAttribute;
                if (attrib != null)
                {
                    Console.WriteLine($"Registering topic for {prop.Name}");
                    Client.Publish($"{Topic}{Client.ClientId}_{prop.Name}", null, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                    Client.Publish($"{Topic}{Client.ClientId}_{prop.Name}/config",
                        System.Text.Encoding.ASCII.GetBytes(
                            $"{{\"name\" :\"{Client.ClientId}_{prop.Name}\", \"state_topic\" : \"{Topic}{Client.ClientId}_{prop.Name}\", \"unit_of_measurement\" :\"{attrib.Unit}\", \"icon\" :\"mdi:{attrib.Icon}\"}}"), uPLibrary.Networking.M2Mqtt.Messages.MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                }
            }

            return true;
        }

        private static void Cmd_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var value = sender.GetType().GetProperty(e.PropertyName).GetValue(sender).ToString();
            Console.WriteLine($"PropertyChanged {e.PropertyName} new Value is : {value}");
            if (Client.IsConnected)
                Client.Publish($"{Topic}{Client.ClientId}_{e.PropertyName}", System.Text.Encoding.ASCII.GetBytes(value));
        }
    }
}
