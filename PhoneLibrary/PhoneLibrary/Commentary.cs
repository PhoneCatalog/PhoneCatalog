using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    class Commentary
    {

        public Commentary(string author, DateTime date, string text, int rating)
        {
            this.author = author;
            this.date = date;
            this.text = text;
            this.rating = rating;
        }

        public string author { get; set; }
        public DateTime date { get; set; }
        public string text { get; set; }
        public int rating { get; set; }
    }
}
