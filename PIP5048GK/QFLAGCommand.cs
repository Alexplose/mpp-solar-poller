using MppSolarPoller.Common;
using System;

namespace PIP5048GK
{
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


  

}
