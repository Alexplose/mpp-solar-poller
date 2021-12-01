using HidSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace mpp_solar_poller
{
    public class QMODCommand : Command
    {
        private QMODMode mode;

        public enum QMODMode
        {
            PowerOn = 1,
            StandBy = 2,
            Line = 3,
            Battery = 4,
            Fault = 5,
            PowerSaving = 6,
            Unknown = 0
        }
        public QMODCommand()
        {
            CommandName = "QMOD";
            ResponseSize = 5;
        }

        public override void Parse(string rawData)
        {
            switch (rawData[1])
            {
                case 'P': Mode = QMODMode.PowerOn; break;  // Power_On
                case 'S': Mode = QMODMode.StandBy; break;  // Standby
                case 'L': Mode = QMODMode.Line; break;  // Line
                case 'B': Mode = QMODMode.Battery; break;  // Battery
                case 'F': Mode = QMODMode.Fault; break;  // Fault
                case 'H': Mode = QMODMode.PowerSaving; break;  // Power_Saving
                default: Mode = QMODMode.Unknown; break;  // Unknown
            }
        }
        [TopicConfig(Icon = "solar-power")]
        public QMODMode Mode { get => mode; set { if (value != mode) { mode = value; NotifyPropertyChanged(); } } }

        public override string ToString()
        {
            return nameof(QMODCommand) + " Mode is :" + Mode;
        }
    }


    public class QPIRICommand : Command
    {
        private decimal gridCurrentRating;
        private decimal gridVoltageRating;
        private decimal outputVoltageRating;
        private decimal outputCurrentRating;
        private int outputVARating;
        private decimal outputWattRating;
        private decimal batteryRating;
        private decimal batteryRechargeVoltage;
        private decimal batteryUnderVoltage;
        private decimal batteryBulkVoltage;
        private decimal batteryFloatVoltage;
        private int batteryType;
        private int maxGridChargeCurrent;
        private int maxChargeCurrent;
        private int inputVoltageRange;
        private int outSourcePriority;
        private int chargerSourcePriority;
        private int machineType;
        private int topology;
        private int outMode;
        private decimal batteryRedischargeVoltage;
        private decimal outputFrequencyRating;
        private int parallelMaxNumber;

        public QPIRICommand()
        {
            CommandName = "QPIRI";
            ResponseSize = 110;
        }

        public decimal GridVoltageRating { get => gridVoltageRating; set { if (value != gridVoltageRating) { gridVoltageRating = value; NotifyPropertyChanged(); } } }
        public decimal GridCurrentRating { get => gridCurrentRating; set { if (value != gridCurrentRating) { gridCurrentRating = value; NotifyPropertyChanged(); } } }
        public decimal OutputVoltageRating { get => outputVoltageRating; set { if (value != outputVoltageRating) { outputVoltageRating = value; NotifyPropertyChanged(); } } }
        public decimal OutputCurrentRating { get => outputCurrentRating; set { if (value != outputCurrentRating) { outputCurrentRating = value; NotifyPropertyChanged(); } } }
        public int OutputVARating { get => outputVARating; set { if (value != outputVARating) { outputVARating = value; NotifyPropertyChanged(); } } }
        public decimal OutputWattRating { get => outputWattRating; set { if (value != outputWattRating) { outputWattRating = value; NotifyPropertyChanged(); } } }
        public decimal BatteryRating { get => batteryRating; set { if (value != batteryRating) { batteryRating = value; NotifyPropertyChanged(); } } }
        public decimal BatteryRechargeVoltage { get => batteryRechargeVoltage; set { if (value != batteryRechargeVoltage) { batteryRechargeVoltage = value; NotifyPropertyChanged(); } } }
        public decimal BatteryUnderVoltage { get => batteryUnderVoltage; set { if (value != batteryUnderVoltage) { batteryUnderVoltage = value; NotifyPropertyChanged(); } } }
        public decimal BatteryBulkVoltage { get => batteryBulkVoltage; set { if (value != batteryBulkVoltage) { batteryBulkVoltage = value; NotifyPropertyChanged(); } } }
        public decimal BatteryFloatVoltage { get => batteryFloatVoltage; set { if (value != batteryFloatVoltage) { batteryFloatVoltage = value; NotifyPropertyChanged(); } } }
        public int BatteryType { get => batteryType; set { if (value != batteryType) { batteryType = value; NotifyPropertyChanged(); } } }
        public int MaxGridChargeCurrent { get => maxGridChargeCurrent; set { if (value != maxGridChargeCurrent) { maxGridChargeCurrent = value; NotifyPropertyChanged(); } } }
        public int MaxChargeCurrent { get => maxChargeCurrent; set { if (value != maxChargeCurrent) { maxChargeCurrent = value; NotifyPropertyChanged(); } } }
        public int InputVoltageRange { get => inputVoltageRange; set { if (value != inputVoltageRange) { inputVoltageRange = value; NotifyPropertyChanged(); } } }
        public int OutSourcePriority { get => outSourcePriority; set { if (value != outSourcePriority) { outSourcePriority = value; NotifyPropertyChanged(); } } }
        public int ChargerSourcePriority { get => chargerSourcePriority; set { if (value != chargerSourcePriority) { chargerSourcePriority = value; NotifyPropertyChanged(); } } }

        public int ParallelMaxNumber { get => parallelMaxNumber; set { if (value != parallelMaxNumber) { parallelMaxNumber = value; NotifyPropertyChanged(); } } }

        public int MachineType { get => machineType; set { if (value != machineType) { machineType = value; NotifyPropertyChanged(); } } }
        public int Topology { get => topology; set { if (value != topology) { topology = value; NotifyPropertyChanged(); } } }
        public int OutMode { get => outMode; set { if (value != outMode) { outMode = value; NotifyPropertyChanged(); } } }
        public decimal BatteryRedischargeVoltage { get => batteryRedischargeVoltage; set { if (value != batteryRedischargeVoltage) { batteryRedischargeVoltage = value; NotifyPropertyChanged(); } } }
        [TopicConfig(Icon = "current-ac", Unit = "Hz")]
        public decimal OutputFrequencyRating { get => outputFrequencyRating; set { if (value != outputFrequencyRating) { outputFrequencyRating = value; NotifyPropertyChanged(); } } }
        public override void Parse(string rawData)
        {
            //"%d %d %d %d %d - %d %d %d %f",
            //  &batt_type, &max_grid_charge_current, &max_charge_current, &in_voltage_range,
            // &out_source_priority, &charger_source_priority,
            // &machine_type, &topology, &out_mode, &batt_redischarge_voltage)


            Console.WriteLine(rawData);
            var dataElem = rawData.Replace("(", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);

            //int i = 0;
            //foreach (var item in dataElem)
            //{
            //    byte[] ba = Encoding.Default.GetBytes(item);
            //    var hexString = BitConverter.ToString(ba);
            //    Console.WriteLine($" {i++} => {item} {hexString}");
            //}

            GridVoltageRating = Convert.ToDecimal(dataElem[0]);
            GridCurrentRating = Convert.ToDecimal(dataElem[1]);
            OutputVoltageRating = Convert.ToDecimal(dataElem[2]);
            OutputFrequencyRating = Convert.ToDecimal(dataElem[3]);
            OutputCurrentRating = Convert.ToDecimal(dataElem[4]);
            OutputVARating = Int32.Parse(dataElem[5]);
            OutputWattRating = Convert.ToDecimal(dataElem[6]);
            BatteryRating = Convert.ToDecimal(dataElem[7]);
            BatteryRechargeVoltage = Convert.ToDecimal(dataElem[8]);
            BatteryUnderVoltage = Convert.ToDecimal(dataElem[9]);
            BatteryBulkVoltage = Convert.ToDecimal(dataElem[10]);
            BatteryFloatVoltage = Convert.ToDecimal(dataElem[11]);
            BatteryType = Int32.Parse(dataElem[12]);
            MaxGridChargeCurrent = Int32.Parse(dataElem[13]);
            MaxChargeCurrent = Int32.Parse(dataElem[14]);
            InputVoltageRange = Int32.Parse(dataElem[15]);
            OutSourcePriority = Int32.Parse(dataElem[16]);
            ChargerSourcePriority = Int32.Parse(dataElem[17]);
            ParallelMaxNumber = Int32.Parse(dataElem[18]);
            MachineType = Int32.Parse(dataElem[19]);
            Topology = Int32.Parse(dataElem[20]);
            OutMode = Int32.Parse(dataElem[21]);
            BatteryRedischargeVoltage = Convert.ToDecimal(dataElem[22]);
            //PV Ok 23
            //PV Power balance 24
        }

        public override string ToString()
        {
            return $"Output W = {OutputWattRating}";
        }

    }


    public class QFLAGCommand : Command
    {
        private bool buzzerEnabled;
        private bool overloadByPass;
        private bool lCDTimeout;
        private bool overloadRestart;
        private bool overTempRestart;
        private bool backlightEnabled;
        private bool primarySourceAlarm;
        private bool recordDefaultCode;

        public QFLAGCommand()
        {
            CommandName = "QFLAG";
            ResponseSize = 10;
        }

        public bool BuzzerEnabled { get => buzzerEnabled; set { if(value != buzzerEnabled) { buzzerEnabled = value; NotifyPropertyChanged();}} }
        public bool OverloadByPass { get => overloadByPass; set { if(value != overloadByPass) { overloadByPass = value; NotifyPropertyChanged();}} }
        public bool LCDTimeout { get => lCDTimeout; set { if(value != lCDTimeout) { lCDTimeout = value; NotifyPropertyChanged();}} }
        public bool OverloadRestart { get => overloadRestart; set { if(value != overloadRestart) { overloadRestart = value; NotifyPropertyChanged();}} }
        public bool OverTempRestart { get => overTempRestart; set { if(value != overTempRestart) { overTempRestart = value; NotifyPropertyChanged();}} }
        public bool BacklightEnabled { get => backlightEnabled; set { if(value != backlightEnabled) { backlightEnabled = value; NotifyPropertyChanged();}} }
        public bool PrimarySourceAlarm { get => primarySourceAlarm; set { if(value != primarySourceAlarm) { primarySourceAlarm = value; NotifyPropertyChanged();}} }
        public bool RecordDefaultCode { get => recordDefaultCode; set { if(value != recordDefaultCode) { recordDefaultCode = value; NotifyPropertyChanged();}} }

        public override void Parse(string rawData)
        {
            rawData = rawData.Substring(1);
            var indexofD = rawData.IndexOf("D");

            BuzzerEnabled = (rawData.IndexOf("A") < indexofD);
            OverloadByPass= (rawData.IndexOf("B") < indexofD);
            LCDTimeout= (rawData.IndexOf("K") < indexofD);
            OverloadRestart= (rawData.IndexOf("U") < indexofD);
            OverTempRestart= (rawData.IndexOf("V") < indexofD);
            BacklightEnabled = (rawData.IndexOf("X") < indexofD);
            PrimarySourceAlarm= (rawData.IndexOf("Y") < indexofD);
            RecordDefaultCode= (rawData.IndexOf("Z") < indexofD);
            Console.WriteLine("QFLAG = " + rawData);
        }
    }

    public class QPIGSCommand : Command
    {
        private decimal gridVoltage;
        private decimal gridFrequency;
        private decimal outputVoltage;
        private decimal outputFrequency;
        private int loadVA;
        private int loadWatt;
        private decimal loadPercentage;
        private decimal busVoltage;
        private decimal batteryVoltage;
        private int batteryChargeCurrent;
        private int batteryCapacity;
        private int heatSinkTemperature;
        private decimal pVInputCurrent;
        private decimal pVInputVoltage;
        private decimal sCCVoltage;
        private int batteryDischargeCurrent;
        private char loadOn;
        private char sCCOn;
        private char aCChargeOn;
        private char pVOrACFeed;
        private int pvInputWatt;
        private decimal loadWattHour;
        private DateTime lastloadWattHourComputed;
        private decimal pvInputWattHour;
        private DateTime lastpvInputWattHourComputed;

        public QPIGSCommand()
        {
            CommandName = "QPIGS";
            ResponseSize = 126;
        }

        public override void Parse(string rawData)
        {
            var dataElem = rawData.Substring(1).Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine(rawData);
            /*int i = 0;

        foreach (var item in dataElem)
            {
                byte[] ba = Encoding.Default.GetBytes(item);
                var hexString = BitConverter.ToString(ba);
                Console.WriteLine($" {i++} => {item} {hexString}");
            }*/
            GridVoltage = Convert.ToDecimal(dataElem[0]);
            GridFrequency = Convert.ToDecimal(dataElem[1]);
            OutputVoltage = Convert.ToDecimal(dataElem[2]);
            OutputFrequency = Convert.ToDecimal(dataElem[3]);
            LoadVA = Int32.Parse(dataElem[4]);
            LoadWatt = Int32.Parse(dataElem[5]);

            LoadPercentage = Convert.ToDecimal(dataElem[6]);
            BusVoltage = Convert.ToDecimal(dataElem[7]);
            BatteryVoltage = Convert.ToDecimal(dataElem[8]);
            BatteryChargeCurrent = Int32.Parse(dataElem[9]);
            BatteryCapacity = Int32.Parse(dataElem[10]);
            HeatSinkTemperature = Int32.Parse(dataElem[11]);
            PVInputCurrent = Convert.ToDecimal(dataElem[12]);
            PVInputVoltage = Convert.ToDecimal(dataElem[13]);
            SCCVoltage = Convert.ToDecimal(dataElem[14]);
            BatteryDischargeCurrent = Int32.Parse(dataElem[15]);
            PVOrACFeed = dataElem[16][0];
            LoadOn = dataElem[16][3];
           // SCCOn = dataElem[16][1];
          //  ACChargeOn = dataElem[16][2];
            PVInputWatt = Int32.Parse(dataElem[19]);
        }

        [TopicConfig("V", "power-plug")]
        public decimal GridVoltage { get => gridVoltage; set { if (value != gridVoltage) { gridVoltage = value; NotifyPropertyChanged(); } } }
        [TopicConfig("Hz", "current-ac")]
        public decimal GridFrequency { get => gridFrequency; set { if (value != gridFrequency) { gridFrequency = value; NotifyPropertyChanged(); } } }
        [TopicConfig("V", "power-plug")]
        public decimal OutputVoltage { get => outputVoltage; set { if (value != outputVoltage) { outputVoltage = value; NotifyPropertyChanged(); } } }
        [TopicConfig("Hz", "current-ac")]
        public decimal OutputFrequency { get => outputFrequency; set { if (value != outputFrequency) { outputFrequency = value; NotifyPropertyChanged(); } } }
        [TopicConfig("VA", "chart-bell-curve")]
        public int LoadVA { get => loadVA; set { if (value != loadVA) { loadVA = value; NotifyPropertyChanged(); } } }

        [TopicConfig("W", "chart-bell-curve")]
        public int LoadWatt
        {
            get => loadWatt; set
            {
                if (value != loadWatt)
                {
                    loadWatt = value; NotifyPropertyChanged();
                    var interval = (DateTime.Now - lastloadWattHourComputed).TotalSeconds;
                    LoadWattHour = (value / (3600 / Convert.ToDecimal(interval)));
                }
            }
        }

        [TopicConfig("Wh", "chart-bell-curve")]
        public decimal LoadWattHour
        {
            get => loadWattHour; set
            {
                if (value != loadWattHour && lastloadWattHourComputed != new DateTime())
                {
                    loadWattHour = value; NotifyPropertyChanged();
                }
                lastloadWattHourComputed = DateTime.Now;

            }
        }


        [TopicConfig("%", "brightness-percent")]
        public decimal LoadPercentage { get => loadPercentage; set { if (value != loadPercentage) { loadPercentage = value; NotifyPropertyChanged(); } } }
        [TopicConfig("V", "details")]
        public decimal BusVoltage { get => busVoltage; set { if (value != busVoltage) { busVoltage = value; NotifyPropertyChanged(); } } }
        [TopicConfig("V", "battery-outline")]
        public decimal BatteryVoltage { get => batteryVoltage; set { if (value != batteryVoltage) { batteryVoltage = value; NotifyPropertyChanged(); } } }
        [TopicConfig("A", "current-dc")]
        public int BatteryChargeCurrent { get => batteryChargeCurrent; set { if (value != batteryChargeCurrent) { batteryChargeCurrent = value; NotifyPropertyChanged(); } } }
        [TopicConfig("%", "battery-outline")]
        public int BatteryCapacity { get => batteryCapacity; set { if (value != batteryCapacity) { batteryCapacity = value; NotifyPropertyChanged(); } } }
        [TopicConfig("°C", "details")]
        public int HeatSinkTemperature { get => heatSinkTemperature; set { if (value != heatSinkTemperature) { heatSinkTemperature = value; NotifyPropertyChanged(); } } }
        [TopicConfig("A", "solar-panel-large")]
        public decimal PVInputCurrent { get => pVInputCurrent; set { if (value != pVInputCurrent) { pVInputCurrent = value; NotifyPropertyChanged(); } } }
        [TopicConfig("V", "solar-panel-large")]
        public decimal PVInputVoltage { get => pVInputVoltage; set { if (value != pVInputVoltage) { pVInputVoltage = value; NotifyPropertyChanged(); } } }

        [TopicConfig("W", "solar-panel-large")]
        public int PVInputWatt
        {
            get => pvInputWatt; set
            {
                if (value != pvInputWatt)
                {
                    pvInputWatt = value; NotifyPropertyChanged();
                    var interval = (DateTime.Now - lastpvInputWattHourComputed).TotalSeconds;
                    PVInputWattHour = (value / (3600 / Convert.ToDecimal(interval)));
                }
            }
        }
        [TopicConfig("Wh", "solar-panel-large")]
        public decimal PVInputWattHour
        {
            get => pvInputWattHour; set
            {
                if (value != pvInputWattHour && lastpvInputWattHourComputed != new DateTime())
                {
                    pvInputWattHour = value; NotifyPropertyChanged();
                }
                lastpvInputWattHourComputed = DateTime.Now;

            }
        }

        [TopicConfig("V", "current-dc")]
        public decimal SCCVoltage { get => sCCVoltage; set { if (value != sCCVoltage) { sCCVoltage = value; NotifyPropertyChanged(); } } }
        [TopicConfig("A", "current-dc")]
        public int BatteryDischargeCurrent { get => batteryDischargeCurrent; set { if (value != batteryDischargeCurrent) { batteryDischargeCurrent = value; NotifyPropertyChanged(); } } }

        [TopicConfig("", "power")]
        public char PVOrACFeed { get => pVOrACFeed; set { if (value != pVOrACFeed) { pVOrACFeed = value; NotifyPropertyChanged(); } } }

        [TopicConfig("", "power")]
        public char LoadOn { get => loadOn; set { if (value != loadOn) { loadOn = value; NotifyPropertyChanged(); } } }
        [TopicConfig("", "power")]
        public char SCCOn { get => sCCOn; set { if (value != sCCOn) { sCCOn = value; NotifyPropertyChanged(); } } }
        [TopicConfig("", "power")]
        public char ACChargeOn { get => aCChargeOn; set { if (value != aCChargeOn) { aCChargeOn = value; NotifyPropertyChanged(); } } }



        public override string ToString()
        {
            //return System.Text.Json.JsonSerializer.Serialize(this);
            return $"LoadWatt {LoadWatt}";
        }

    }
    public class QPIWSCommand : Command
    {
        public QPIWSCommand()
        {
            CommandName = "QPIWS";
            ResponseSize = 42;
        }

        public override void Parse(string rawData)
        {
            rawData = rawData.Substring(1);

            PVLoss = rawData[0] == 1;
            InverterFault = rawData[1] == 1;
            BusOver = rawData[2] == 1;
            BusUnder = rawData[3] == 1;
            BusSoftFail = rawData[4] == 1;
            LineFail = rawData[5] == 1;
            OPVShort = rawData[6] == 1;
            PowerLimit = rawData[25] == 1;
            LowPVEnergy = rawData[33] == 1;
            Console.WriteLine(rawData);
        }

        private bool pVLoss;

        public bool PVLoss { get => pVLoss; set { if (pVLoss != value) { pVLoss = value; NotifyPropertyChanged(); } } }


        private bool inverterFault;

        public bool InverterFault { get => inverterFault; set { if (inverterFault != value) { inverterFault = value; NotifyPropertyChanged(); } } }

        private bool busOver;

        public bool BusOver { get => busOver; set { if (busOver != value) { busOver = value; NotifyPropertyChanged(); } } }


        private bool busUnder;

        public bool BusUnder { get => busUnder; set { if (busUnder != value) { busUnder = value; NotifyPropertyChanged(); } } }

        private bool busSoftFail;

        public bool BusSoftFail { get => busSoftFail; set { if (busSoftFail != value) { busSoftFail = value; NotifyPropertyChanged(); } } }

        private bool lineFail;

        public bool LineFail { get => lineFail; set { if (lineFail != value) { lineFail = value; NotifyPropertyChanged(); } } }

        private bool opvShort;

        public bool OPVShort { get => opvShort; set { if (opvShort != value) { opvShort = value; NotifyPropertyChanged(); } } }

        private bool powerLimit;
        public bool PowerLimit { get => powerLimit; set { if (powerLimit != value) { powerLimit = value; NotifyPropertyChanged(); } } }

        private bool lowPVEnergy;
        public bool LowPVEnergy { get => lowPVEnergy; set { if (lowPVEnergy != value) { lowPVEnergy = value; NotifyPropertyChanged(); } } }


    }


    public class QGMNCommand : Command
    {
        public QGMNCommand()
        {
            CommandName = "QGMN";
            ResponseSize = 5;
        }

        public override void Parse(string rawData)
        {
            Console.WriteLine(rawData);
            switch (rawData)
            {
                case "(023":
                    ModelName = "PIP-5048MK";
                    break;
                case "(024":
                    ModelName = "PIP-3024MK";
                    break;
                case "(037":
                    ModelName = "PIP-5048GK";
                    break;
                default:
                    ModelName = $"Unknown {rawData}";
                    break;
            }
        }
        private string modelName;
        public string ModelName
        {
            get { return modelName; }
            set
            {
                if (value != modelName)
                {
                    modelName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public override string ToString()
        {
            return $"{nameof(QGMNCommand)} {ModelName}";
        }

    }


    public class InputCommand : Command
    {
        public bool Result { get; set; }

        public override void Parse(string rawData)
        {
            Result = rawData.Contains("ACK");
            Console.WriteLine($"{nameof(InputCommand)}: {rawData}");
        }
    }


    public abstract class Command : INotifyPropertyChanged
    {
        public string CommandName { get; set; }
        public int ResponseSize { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void Parse(string rawData);


        public string ReadCommand(HidStream hidStream)
        {
            var cmd = System.Text.Encoding.ASCII.GetBytes(CommandName);
            var crc = new byte[] { 0xB7, 0xA9 };//BitConverter.GetBytes(Crc16.ComputeChecksum(cmd));
            var toWrite = new byte[cmd.Length + crc.Length + 1];
            System.Buffer.BlockCopy(cmd, 0, toWrite, 0, cmd.Length);
            System.Buffer.BlockCopy(crc, 0, toWrite, cmd.Length, crc.Length);
            toWrite[toWrite.Length - 1] = 0x0d;
            //Console.WriteLine(System.Text.Encoding.ASCII.GetString(toWrite));
            hidStream.Write(toWrite);
            var buf = new byte[Math.Min(9, ResponseSize)];
            var resBuffer = new byte[ResponseSize];
            int read = 0;
            int totalRead = 0;
            while (totalRead < ResponseSize)
            {
                read = hidStream.Read(buf, 0, buf.Length);
                //  Console.WriteLine($"Loop TotalRead={totalRead}, Count={read}");
                if (totalRead + read > ResponseSize)
                    System.Buffer.BlockCopy(buf, 0, resBuffer, totalRead, ResponseSize - totalRead);
                else
                    System.Buffer.BlockCopy(buf, 0, resBuffer, totalRead, read);
                totalRead += read;
            }
            //Console.WriteLine($"TotalRead={totalRead}, Count={read}");
            //System.Buffer.BlockCopy(buf, 0, resBuffer, totalRead, read);

            return System.Text.Encoding.ASCII.GetString(resBuffer).Replace("\0", "");
        }

        public void ProcessCommand(HidStream hidStream)
        {
            Parse(ReadCommand(hidStream));
        }

    }

    public class TopicConfigAttribute : Attribute
    {

        public TopicConfigAttribute(string unit = null, string icon = null)
        {
            Icon = icon;
            Unit = unit;
        }
        public string Icon { get; set; }
        public string Unit { get; set; }

    }

}
