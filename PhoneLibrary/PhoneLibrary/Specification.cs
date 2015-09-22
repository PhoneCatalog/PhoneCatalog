using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary
{
    public class Specification : BaseClass
    {
        public string OS { get; set; }
        public string Weight { get; set; }
        public string PhoneSize { get; set; }
        public string DisplaySize { get; set; }
        public string Processor { get; set; }
        public string Graphics { get; set; }
        public string RAM { get; set; }
        public string InternalMemory { get; set; }
        public string MemoryCardSlot { get; set; }
        public string MobileNetwork { get; set; }
        public int NumberOfSIM { get; set; }
        public string Sensors { get; set; }
        public Design PhoneDesign { get; set; }
        public Multimedia PhoneMultimedia { get; set; }
        public Interfaces PhoneInterfaces { get; set; }
        public Battery PhoneBattery { get; set; }
    }

}
