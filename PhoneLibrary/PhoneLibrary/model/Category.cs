using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary.model
{
    public class Category : BaseClass
    {
        public string Name { get; set; }

        public override object Clone()
        {
            return new Category { Id = this.Id, Name = this.Name };
        }
    }
}