using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AttackSurface.Models
{
    public class Statistics
    {
        private static readonly object objLock = new object();
        private static Statistics _instance;

        public Statistics()
        {

        }

        public static Statistics Instance
        {
            get
            {
                lock(objLock)
                {
                    if (_instance == null)
                    {
                        lock (objLock)
                        {
                            _instance = new Statistics();
                        }
                    }
                    return _instance;

                }
            }
        }
        public int Vm_count { get; set; }
        public int request_count { get; set; }
        public double average_request_time { get; set; }
        [JsonIgnore]
        public double TotalRequestsTime { get; set; }
       
        
    }
}
