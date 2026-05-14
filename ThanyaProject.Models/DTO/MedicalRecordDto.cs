using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace ThanyaProject.Models.DTO
    {
        public class MedicalRecordDto
        {
        [Required(ErrorMessage = "Blood type is required.")]
            public string? BloodType { get; set; }= null!;

            public string? ChronicDiseases { get; set; }

            public string? Allergies { get; set; }

            public string? CurrentMedication { get; set; }
        }
    }

