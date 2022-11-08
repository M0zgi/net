using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    [Serializable]
    public class Auth
    {
        public string? Email;
        public string? Password;
        public string? Name;
        public string? msg;
        //public List<string> msg = new List<string>();
    }
}
