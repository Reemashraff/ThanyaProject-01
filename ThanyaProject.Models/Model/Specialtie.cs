using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class Specialtie
    {
       
        [Key]
        public int SpecialtyId { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<User>? Users { get; set; }
    }
}
