using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
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
        public string? Weight { get; set; }
        public string? Summery { get; set; } 
        public ICollection<Image> MedicalImages { get; set; } = new List<Image>();

        public User User { get; set; } = null!;

    }
}
