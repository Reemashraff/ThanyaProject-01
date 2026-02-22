using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class User : IdentityUser<int>
    {
       
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int National_ID { get; set; }
        public string Address { get; set; } = null!;
        [Column(TypeName = "decimal(9,6)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(9,6)")]
        public decimal Longitude { get; set; }
        public string? QRcode { get; set; }
        public string Role { get; set; }= null!;
        public int Specialty_ID { get; set; }
        public string? Description { get; set; }
        public int Hospital_ID { get; set; }
        public int UserType { get; set; }
        public Hospital? Hospital { get; set; }
        public Specialtie? Specialty { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; }
        public ICollection<InjuryRecords>? InjuryRecords { get; set; }
        public ICollection<EmergancyContact>? EmergencyContacts { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<Device>? Devices { get; set; }
        public ICollection<Order>? Orders { get; set; }

    }
}
