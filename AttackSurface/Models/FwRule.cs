using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttackSurface.Models
{
    public class FwRule
    {
        public string fw_id { get; set; }
        public string source_tag { get; set; }
        public string dest_tag { get; set; }
    }
}
