using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.Model;

namespace ThanyaProject.DAL.Data
{
    public class DbSeed
    {
        public static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Doctor"))
                await roleManager.CreateAsync(new Role { Name = "Doctor" });

            if (!await roleManager.RoleExistsAsync("Paramedic"))
                await roleManager.CreateAsync(new Role { Name = "Paramedic" });

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new Role { Name = "User" });
        }

    }
}
