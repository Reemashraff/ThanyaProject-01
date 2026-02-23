using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class Notification
    {

        [Key]
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public int? InjuryId { get; set; }
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public DateTime DateCreated { get; set; }
        public string Type { get; set; } = "emergency";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public User User { get; set; } = null!;
        public InjuryRecords? Injury { get; set; }



    }
}
