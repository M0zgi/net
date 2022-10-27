using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.Entities;

namespace Lib.Entities
{
    [Serializable]
    public class ZipCode
    {
        public Guid Id { get; set; }
        public string? Zip { get; set; }
        public ICollection<Street> Streets { get; set; }
    }
}
