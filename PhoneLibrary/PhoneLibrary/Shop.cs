using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    class Shop
    {
        public Shop(string name, string address, string phonenumber, string website, int minprice, int maxprice)
        {
            this.name = name;
            this.address = address;
            this.phonenumber = phonenumber;
            this.website = website;
            this.minprice = minprice;
            this.maxprice = maxprice;
        }

        public string name { get; set; }
        public string address { get; set; }
        public string phonenumber { get; set; }
        public string website { get; set; }
        public long minprice { get; set; }
        public long maxprice { get; set; }
    }
}
