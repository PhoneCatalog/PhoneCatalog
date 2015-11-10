using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneLibrary.model
{

    public class User : BaseClass
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public override object Clone()
        {
            return new User
            {
                Email = this.Email,
                Id = this.Id,
                Login = this.Login,
                Password = this.Password,
                PhoneNumber = this.PhoneNumber
            };
        }
    }
}
