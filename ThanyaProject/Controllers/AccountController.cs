using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
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
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            JwtService jwtService,
            IWebHostEnvironment env,
             AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _env = env;
            _context = context;

        }




        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Registeration model)
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

         
            var medicalRecord = new MedicalRecord
            {
                UserId = user.Id,
                BloodType = model.BloodType,
                ChronicDiseases = model.ChronicDiseases,
                Allergies = model.Allergies,
                CurrentMedication = model.CurrentMedication
            };

            _context.MedicalRecords.Add(medicalRecord);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "User Registered Successfully",
                Email = user.Email,
                Role = roleName
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
                Secure = false,
                SameSite = SameSiteMode.Lax,
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
            Response.Cookies.Delete("token");
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful!" });
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

            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "success",
                medicalRecord = new
                {
                    bloodType = medical.BloodType,
                    chronicDiseases = medical.ChronicDiseases,
                    allergies = medical.Allergies,
                    currentMedication = medical.CurrentMedication
                }
            });
        }

        [HttpGet("Show medical")]
        [Authorize]
        public async Task<IActionResult> GetMedicalRecord()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var medical = await _context.MedicalRecords
                .FirstOrDefaultAsync(m => m.UserId == userId);
            if (medical == null)
                return NotFound("Medical record not found");
            return Ok(new
            {
                bloodType = medical.BloodType,
                chronicDiseases = medical.ChronicDiseases,
                allergies = medical.Allergies,
                currentMedication = medical.CurrentMedication
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

            var medical = _context.MedicalRecords
                .FirstOrDefault(m => m.UserId == user.Id);

            return Ok(new
            {
                id = user.Id,
                name = user.FirstName + " " + user.LastName,
                email = user.Email,
                role = roles.FirstOrDefault(),

                medicalRecord = new
                {
                    bloodType = medical?.BloodType,
                    chronicDiseases = medical?.ChronicDiseases,
                    allergies = medical?.Allergies,
                    currentMedication = medical?.CurrentMedication
                }
            });
        }
    }
}