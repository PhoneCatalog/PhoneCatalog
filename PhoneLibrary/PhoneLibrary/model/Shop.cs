using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary.model
{
    public class Shop : BaseClass
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public int Price { get; set; }
        public int PhoneId { get; set; }

        public override object Clone()
        {
            return new Shop
            {
                Address = this.Address,
                Id = this.Id,
                Name = this.Name,
                PhoneNumber = this.PhoneNumber,
                Price = this.Price,
                Website = this.Website,
                PhoneId = this.PhoneId
            };
        }
    }
}