using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
    public class ContactUsDto
    {
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;


    }
}
