using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MppSolarPoller.Common
{
    public class InputCommand : Command
    {
        public bool Result { get; set; }

        public override void Parse(string rawData)
        {
            Result = rawData.Contains("ACK");
            Console.WriteLine($"{nameof(InputCommand)}: {rawData}");
        }
    }

}
