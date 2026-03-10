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
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new Role { Name = "Admin" });
            if (!await roleManager.RoleExistsAsync("Doctor"))
                await roleManager.CreateAsync(new Role { Name = "Doctor" });

            if (!await roleManager.RoleExistsAsync("Paramedic"))
                await roleManager.CreateAsync(new Role { Name = "Paramedic" });

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new Role { Name = "User" });
        }
        public static async Task SeedAdminAsync(UserManager<User> userManager)
        {
            var adminEmail = "admin@thanya.com";

            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                var newAdmin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,

                    FirstName = "System",
                    LastName = "Admin",
                    Age = 30,
                    Gender = "Male",
                    Phone = "01116363262",
                    National_ID = 123456789,
                    Address = "Alex",

                    Latitude = 30.0444m,
                    Longitude = 31.2357m,

                    Specialty_ID = 1,
                    Hospital_ID = 1,
                    UserType = 0
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }

    }
    
}
