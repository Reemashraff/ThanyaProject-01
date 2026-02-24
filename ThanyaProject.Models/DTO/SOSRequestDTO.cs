using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
    public class SOSRequestDTO
    {
        public string DeviceId { get; set; }
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }

        public LocationDto Location { get; set; }
    }
}
