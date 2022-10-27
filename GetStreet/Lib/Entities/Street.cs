using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entities;

namespace LIB.Entities
{
    [Serializable]
    public class Street
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public ZipCode ZipCode { get; set; }
    }
}
