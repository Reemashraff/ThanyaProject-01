using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class Hospital
    {
     
        [Key]
        public int HospitalId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        [Column(TypeName = "decimal(9,6)")]

        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(9,6)")]

        public decimal Longitude { get; set; }
        public string Type { get; set; } = null!;
        public bool HasICU { get; set; }
        public int Capacity { get; set; }
        public bool EmergencyDept { get; set; }
        public string Email { get; set; } = null!;

        public ICollection<User>? Users { get; set; }

    }
}
