using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary.model
{
    public class Characteristic : BaseClass
    {
        public int PhoneId { get; internal set; }
        public Specification Specification { get; set; }
        public string Value { get; set; }

        public override object Clone()
        {
            return new Characteristic
            {
                Id = this.Id,
                PhoneId = this.PhoneId,
                Specification = this.Specification == null ? null : (Specification)this.Specification.Clone(),
                Value = this.Value
            };
        }
    }
}