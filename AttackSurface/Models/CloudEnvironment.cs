using AttackSurface.Models;
using System.Collections.Generic;

namespace AttackSurface
{
    internal class CloudEnvironment
    {
        public List<Vm> vms { get; set; }
        public List<FwRule> fw_rules { get; set; }
    }
}