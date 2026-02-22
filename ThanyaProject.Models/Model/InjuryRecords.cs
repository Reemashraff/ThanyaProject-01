using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class InjuryRecords
    {
        
        [Key]
        public int InjuryId { get; set; }
        public int UserId { get; set; }
        public int? DoctorId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }

        public User User { get; set; } = null!;
        public Doctor? Doctor { get; set; }

    }
}
