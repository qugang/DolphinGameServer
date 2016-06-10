using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinNetWork
{
    public class ControllerProtocolAttribute : Attribute
    {
        public int ProtocolNumber { get; set; }
        public ControllerProtocolAttribute(int protocolNumber)
        {
            this.ProtocolNumber = protocolNumber;
        }

    }
}
