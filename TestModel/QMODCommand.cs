using MppSolarPoller.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModel
{
    public class QMODCommand : Command, ICommand
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


        public override string ReadCommand(Stream hidStream)
        {
            return "(L";
        }
        public override void ProcessCommand(Stream hidStream)
        {
            Parse(ReadCommand(hidStream));
        }

        public override string ToString()
        {
            return nameof(QMODCommand) + " Mode is :" + Mode;
        }
    }
}
