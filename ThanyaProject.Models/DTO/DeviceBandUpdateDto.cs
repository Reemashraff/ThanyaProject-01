using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
    public class DeviceBandUpdateDto
    {
        public string DeviceId { get; set; }

        public int Battery { get; set; }

        public float Lat { get; set; }

        public float Long { get; set; }

        public int HeartRate { get; set; }

        public int OxygenLevel { get; set; }
    }
}
