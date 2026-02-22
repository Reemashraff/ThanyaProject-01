using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class MedicalRecord
    {
       
        [Key]
        public int RecordId { get; set; }
        public int UserId { get; set; }
        public string? BloodType { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? Allergies { get; set; }
        public string? CurrentMedication { get; set; }

        public User User { get; set; } = null!;

    }
}
