using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MppSolarPoller.Common
{
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
