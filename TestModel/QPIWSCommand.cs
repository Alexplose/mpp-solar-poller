using MppSolarPoller.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestModel
{
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


        public override string ReadCommand(Stream hidStream)
        {
            return "(000000000000000000000010000000000000";
        }

    }


  

}
