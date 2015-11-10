using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary.model
{
    public class Producer : BaseClass
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Site { get; set; }
        public override object Clone()
        {
            return new Producer { Country = this.Country, Id = this.Id, Name = this.Name, Site = this.Site };
        }
    }
}
