using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary.model
{
    public class Preset : BaseClass
    {
        public int SpecificationId { get; set; }
        public string Value { get; set; }

        public override object Clone()
        {
            return new Preset { Id = this.Id, Value = this.Value, SpecificationId = this.SpecificationId };
        }
    }
}