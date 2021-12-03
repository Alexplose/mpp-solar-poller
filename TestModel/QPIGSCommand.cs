using MppSolarPoller.Common;
using System;
using System.IO;

namespace TestModel
{
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



        public override string ReadCommand(Stream hidStream)
        {
            return "(235.7 50.0 235.7 50.0 0518 0406 010 387 01.00 000 000 0035 00.0 197.2 00.00 00000 00010000 00 00 00000 010";
        }


        public override string ToString()
        {
            //return System.Text.Json.JsonSerializer.Serialize(this);
            return $"LoadWatt {LoadWatt}";
        }

    }


  

}
