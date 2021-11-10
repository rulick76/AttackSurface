using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttackSurface.Models
{
    public class Vm
    {
        public string vm_id { get; set; }
        public string name { get; set; }
        public List<string> tags { get; set; }
    }
}
