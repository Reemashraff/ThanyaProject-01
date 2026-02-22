using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class Role : IdentityRole<int>
    {
        public Role() { }
        public ICollection<UserRole>? UsersRole { get; set; }
    }
}