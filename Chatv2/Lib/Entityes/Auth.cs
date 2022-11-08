using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entityes
{
    [Serializable]
    public class Auth
    {
        public string? Email;
        public string? Password;
        public string? Username;
        public string? msg;
    }
}
