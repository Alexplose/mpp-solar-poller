using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MppSolarPoller.Common
{
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
}
