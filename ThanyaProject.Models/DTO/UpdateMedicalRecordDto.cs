using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
   public class UpdateMedicalRecordDto
    {
        public string? BloodType { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? Allergies { get; set; }
        public string? CurrentMedication { get; set; }
        public string? Weight { get; set; }
    }
}
