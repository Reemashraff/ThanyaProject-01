using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
    public class SosHistoryDto
    {
        public string Id { get; set; }
        public string DeviceName { get; set; }
        public string Time { get; set; }
        public bool Resolved { get; set; }
        public string Details { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
