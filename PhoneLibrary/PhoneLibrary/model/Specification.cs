using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary.model
{
    public class Specification : BaseClass
    {
        public string Name { get; set; }

        public Category Category { get; set; }

        public override object Clone()
        {
            return new Specification
            {
                Category = this.Category == null ? null : (Category)this.Category.Clone(),
                Id = this.Id,
                Name = this.Name,
            };
        }
    }
}