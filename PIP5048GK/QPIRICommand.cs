using MppSolarPoller.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIP5048GK
{
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
            var dataElem = rawData.Replace("(", "").Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

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
}
