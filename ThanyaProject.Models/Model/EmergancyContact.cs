using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class EmergancyContact
    {
   
        [Key]
        public int ContactId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Relation { get; set; } = null!;
        public bool IsPreferred { get; set; } = false;
        public string Phone { get; set; } = null!;

        public User User { get; set; } = null!;


    }
}
