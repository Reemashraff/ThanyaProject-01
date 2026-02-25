using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
    public class DevicrDashboardDto
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public int Battery { get; set; }
        public string Status { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
