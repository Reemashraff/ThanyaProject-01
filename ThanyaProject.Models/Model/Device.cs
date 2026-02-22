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
        [Key]
        public int DeviceId { get; set; }

        public string DeviceName { get; set; } = null!;

        public string SerialNumber { get; set; } = null!;

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;
       
    }
}

