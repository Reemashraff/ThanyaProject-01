using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using ThanyaProject.BL.Service;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.DAL.Data;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Enums;
using ThanyaProject.Models.Model;
using ThanyaProject.Services;

namespace ThanyaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;
        private readonly IImageService _imageService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            JwtService jwtService,
            IWebHostEnvironment env,
             AppDbContext context,
             IImageService imageService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _env = env;
            _context = context;
            _imageService = imageService;
        }




        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] Registeration model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
                return BadRequest("Email already exists");


            var roleName = "User";

         
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = roleName
                });
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                Phone = model.PhoneNumber,
                Age = DateTime.Now.Year - model.DateOfBirth.Year,
                Gender = model.Gender,
                UserType = 1
            };

          
            var result = await _userManager.CreateAsync(user, model.Password);

           
            if (!result.Succeeded)
                return BadRequest(result.Errors);

           
            await _userManager.AddToRoleAsync(user, roleName);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.CreateToken(user, roles);

            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

           
            var medicalRecord = new MedicalRecord
            {
                UserId = user.Id,
                BloodType = model.BloodType,
                ChronicDiseases = model.ChronicDiseases,
                Allergies = model.Allergies,
                CurrentMedication = model.CurrentMedication,
                Weight = model.Weight,
                Summery = model.Summery
            };

            _context.MedicalRecords.Add(medicalRecord);

            await _context.SaveChangesAsync();
            if (model.MedicalImages != null && model.MedicalImages.Any())
            {
                foreach (var file in model.MedicalImages)
                {
                    var uploadResult =
                        await _imageService.UploadImageAsync(file);

                    var image = new Image
                    {
                        Url = uploadResult.SecureUrl.ToString(),
                        PublicId = uploadResult.PublicId,
                        MedicalRecordId = medicalRecord.RecordId
                    };

                    _context.Images.Add(image);
                }

                await _context.SaveChangesAsync();
            }   

            return Ok(new
            {
                status = "success",
                token = token,
                data = new
                {
                    email = user.Email,
                    role = roleName
                }
            });
        }




        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized("Invalid Email or Password");

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                model.Password,
                false);

            if (!result.Succeeded)
                return Unauthorized("Invalid Email or Password");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.CreateToken(user, roles);

            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new
            {
                status = "success",
                data = new
                {
                    user = new
                    {
                        id = user.Id,
                        name = user.FirstName + " " + user.LastName,
                        email = user.Email,
                        role = roles.FirstOrDefault(),
                        last_login = DateTime.UtcNow
                    }
                }
            });
        }


        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful!" });
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateDataUser([FromBody] UpdateUserDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new
                {
                    status = "error",
                    message = "User not found"
                });

            if (!string.IsNullOrWhiteSpace(model.FirstName))
                user.FirstName = model.FirstName;

            if (!string.IsNullOrWhiteSpace(model.LastName))
                user.LastName = model.LastName;

            if (!string.IsNullOrWhiteSpace(model.Phone))
                user.Phone = model.Phone;

            if (!string.IsNullOrWhiteSpace(model.Address))
                user.Address = model.Address;

            if (!string.IsNullOrWhiteSpace(model.Gender))
                user.Gender = model.Gender;

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var emailExists = await _userManager.Users
                    .AnyAsync(x => x.Email == model.Email && x.Id != user.Id);

                if (emailExists)
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Email already exists"
                    });
                }

                user.Email = model.Email;
                user.UserName = model.Email;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    status = "error",
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new
            {
                status = "success",
                message = "User updated successfully",
                data = new
                {
                    id = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    phone = user.Phone,
                    address = user.Address,
                    gender = user.Gender
                }
            });
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new
                {
                    status = "error",
                    message = "User not found"
                });
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = "Passwords do not match"
                });
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    status = "error",
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Password changed successfully"
            });
        }

        [HttpPut("medical/update")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateMedicalRecord([FromBody] MedicalRecordDto model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var medical = await _context.MedicalRecords
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (medical == null)
                return NotFound("Medical record not found");

            if (!string.IsNullOrWhiteSpace(model.BloodType))
            {
                if (!BloodTypeCase.TryParseBloodType(model.BloodType, out BloodType bloodEnum))
                {
                    return BadRequest("Invalid blood type. Valid types: A+, A-, B+, B-, AB+, AB-, O+, O- or _Positive/_Negative.");
                }

                medical.BloodType = bloodEnum.ToString();
            }

            if (!string.IsNullOrWhiteSpace(model.ChronicDiseases))
                medical.ChronicDiseases = model.ChronicDiseases;

            if (!string.IsNullOrWhiteSpace(model.Allergies))
                medical.Allergies = model.Allergies;

            if (!string.IsNullOrWhiteSpace(model.CurrentMedication))
                medical.CurrentMedication = model.CurrentMedication;
            if (!string.IsNullOrWhiteSpace(model.Weight))
                medical.Weight = model.Weight;
            if (!string.IsNullOrWhiteSpace(model.Summery))
                medical.Summery = model.Summery;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "success",
                medicalRecord = new
                {
                    bloodType = medical.BloodType,
                    chronicDiseases = medical.ChronicDiseases,
                    allergies = medical.Allergies,
                    currentMedication = medical.CurrentMedication,
                    weight = medical.Weight,
                    summery = medical.Summery
                }
            });
        }
        //[HttpPatch("medical/update-images")]
        //[Authorize]
        //public async Task<IActionResult> UpdateMedicalImages([FromForm] UpdateImageMedical model)
        //{
        //    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        //    var medical = await _context.MedicalRecords
        //        .Include(x => x.MedicalImages)
        //        .FirstOrDefaultAsync(x => x.UserId == userId);

        //    if (medical == null)
        //        return NotFound("Medical record not found");

        //    // =========================
        //    // 1️⃣ DELETE IMAGES
        //    // =========================
        //    if (model.DeleteImageIds != null && model.DeleteImageIds.Count != 0)
        //    {
        //        var imagesToDelete = medical.MedicalImages
        //            .Where(x => x.MedicalRecordId == medical.RecordId)
        //            .Where(x => model.DeleteImageIds!.Contains(x.Id))
        //            .ToList();

        //        foreach (var img in imagesToDelete)
        //        {
        //            await _imageService.DeleteImageAsync(img.PublicId);
        //            _context.Images.Remove(img);
        //        }
        //    }
        //    await _context.SaveChangesAsync();

        //    // =========================
        //    // 2️⃣ ADD NEW IMAGES
        //    // =========================
        //    if (model.NewImages != null && model.NewImages.Any())
        //    {
        //        foreach (var file in model.NewImages)
        //        {
        //            var upload = await _imageService.UploadImageAsync(file);

        //            var image = new Image
        //            {
        //                Url = upload.SecureUrl.ToString(),
        //                PublicId = upload.PublicId,
        //                MedicalRecordId = medical.RecordId
        //            };

        //            _context.Images.Add(image);
        //        }
        //    }

        //    await _context.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        status = "success",
        //        message = "Medical images updated successfully"
        //    });
        //}

        [HttpPost("medical/add-images")]
        [Authorize]
        public async Task<IActionResult> AddMedicalImages([FromForm] AddNewImageDto model)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var medical = await _context.MedicalRecords
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (medical == null)
                return NotFound("Medical record not found");

            if (model.Images == null || !model.Images.Any())
                return BadRequest("No images uploaded");

            foreach (var file in model.Images)
            {
                var upload = await _imageService.UploadImageAsync(file);

                var image = new Image
                {
                    Url = upload.SecureUrl.ToString(),
                    PublicId = upload.PublicId,
                    MedicalRecordId = medical.RecordId
                };

                _context.Images.Add(image);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "success",
                message = "Images added successfully"
            });
        }
        [HttpDelete("medical/delete-image/{imageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteMedicalImage(int imageId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var medical = await _context.MedicalRecords
                .Include(m => m.MedicalImages)
                .FirstOrDefaultAsync(m => m.UserId == userId);
            if (medical == null)
                return NotFound("Medical record not found");
            var image = medical.MedicalImages.FirstOrDefault(i => i.Id == imageId);
            if (image == null)
                return NotFound("Image not found");
            await _imageService.DeleteImageAsync(image.PublicId);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                status = "success",
                message = "Image deleted successfully"
            });
        }

        [HttpGet("Show medical")]
        [Authorize]
        public async Task<IActionResult> GetMedicalRecord()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var medical = await _context.MedicalRecords
            .Include(m => m.MedicalImages)
            .FirstOrDefaultAsync(m => m.UserId == userId);
            if (medical == null)
                return NotFound("Medical record not found");
            return Ok(new
            {
                bloodType = medical.BloodType,
                chronicDiseases = medical.ChronicDiseases,
                allergies = medical.Allergies,
                currentMedication = medical.CurrentMedication,
                weight = medical.Weight,
                summery = medical.Summery,
                image = medical.MedicalImages.Select(x => new
                {
                    x.Id,
                    x.Url
                })
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var medical = await _context.MedicalRecords
            .Include(m => m.MedicalImages)
            .FirstOrDefaultAsync(m => m.UserId == user.Id);
 
            return Ok(new
            {
                id = user.Id,
                name = user.FirstName + " " + user.LastName,
                email = user.Email,
                age = user.Age,
                gender = user.Gender,
                role = roles.FirstOrDefault(),

                medicalRecord = new
                {
                    bloodType = medical?.BloodType,
                    chronicDiseases = medical?.ChronicDiseases,
                    allergies = medical?.Allergies,
                    currentMedication = medical?.CurrentMedication,
                    weight = medical?.Weight,
                    summery = medical?.Summery,
                    image = medical.MedicalImages.Select(x => new
                    {
                        x.Id,
                        x.Url
                    })

                }
            });
        }
    }
}