using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary
{
    public class Interfaces : BaseClass
    {
        public Boolean Bluetooth { get; set; }
        public Boolean GPS { get; set; }
        public Boolean NFC { get; set; }
        public Boolean Radio { get; set; }
        public string WLAN { get; set; }
        public string USB { get; set; }
        public Boolean HDMI { get; set; }
        public Boolean MHL { get; set; }
        public Boolean IrDA { get; set; }
    }
}
