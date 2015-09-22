using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary
{
    public class Phone : BaseClass
    {
        public Producer PhoneProducer { get; set; }
        public string Series { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string DateOfRelease { get; set; }
        public Specification PhoneCharacteristics { get; set; }
        public List<Shop> Shops { get; set; }
        public List<Comment> PhoneComments { get; set; }
        public string Description { get; set; }
    }
}