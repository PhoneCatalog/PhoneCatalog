using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary.model
{
    public abstract class BaseClass : ICloneable
    {
        public int Id { get; set; }
        public abstract object Clone();
    }
}
