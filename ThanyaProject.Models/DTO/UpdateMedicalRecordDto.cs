using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.Model;

namespace ThanyaProject.Models.DTO
{
   public class UpdateMedicalRecordDto
    {
        public string? BloodType { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? Allergies { get; set; }
        public string? CurrentMedication { get; set; }
        public string? Weight { get; set; }
        public string? Summery { get; set; }
        //public int? ImageId { get; set; }
        //public string? ImageUrl { get; set; }
        //public Image? Image { get; set; }
       
    }
}
