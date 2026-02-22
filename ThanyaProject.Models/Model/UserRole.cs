using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class UserRole : IdentityUserRole<int>
    {

        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }

}
