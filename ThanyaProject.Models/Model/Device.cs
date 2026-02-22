using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class Device
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public int Battery { get; set; }
        public string Status { get; set; }
        public float Long { get; set; }
        public float Lat { get; set; }
        public DateTime LastUpdate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}

