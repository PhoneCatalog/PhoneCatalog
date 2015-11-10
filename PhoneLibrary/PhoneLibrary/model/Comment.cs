using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary.model
{
    public class Comment : BaseClass
    {
        public User Author { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int Mark { get; set; }
        public int PhoneId { get; set; }

        public override object Clone()
        {
            return new Comment
            {
                Id = this.Id,
                Author = this.Author == null ? null : (User)this.Author.Clone(),
                Date = this.Date,
                Mark = this.Mark,
                Text = this.Text,
                PhoneId = this.PhoneId
            };
        }
    }
}