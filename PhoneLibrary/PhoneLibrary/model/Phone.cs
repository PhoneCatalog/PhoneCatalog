using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary.model
{
    public class Phone : BaseClass
    {
        public Producer PhoneProducer { get; set; }
        public string Series { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string DateOfRelease { get; set; }
        public string Description { get; set; }

        public override object Clone()
        {
            return new Phone
            {
                DateOfRelease = this.DateOfRelease,
                Description = this.Description,
                Id = this.Id,
                Model = this.Model,
                PhoneProducer = this.PhoneProducer == null ? null : (Producer)this.PhoneProducer.Clone(),
                Series = this.Series,
                Type = this.Type
            };
        }
    }
}