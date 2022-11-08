using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entityes
{
    [Serializable]
    public class User
    {
        public Guid Id { get; set; }
        public string Email;
        public string Password;
        public string UserName;

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is User secondUser))
                return false;

            return this.Email.Equals(secondUser.Email) && this.Password.Equals(secondUser.Password);
        }
    }
}
