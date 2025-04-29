using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Demo03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet("currentuser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            return Ok(new
            {
                id = user.Id,
                name = user.UserName
            });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    _logger.LogWarning("GetUsers called by unauthenticated user");
                    return Unauthorized();
                }

                var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
                _logger.LogInformation($"Current user: {currentUser.UserName}, Roles: {string.Join(", ", currentUserRoles)}");

                if (currentUserRoles.Count == 0)
                {
                    _logger.LogWarning($"User {currentUser.UserName} has no roles assigned");
                    return Ok(new { success = false, message = "User has no roles assigned" });
                }

                if (currentUserRoles.Contains("Manager"))
                {
                    // For manager: return all users
                    var users = await _userManager.Users.ToListAsync();
                    _logger.LogInformation($"Found {users.Count} users");
                    return Ok(new { success = true, users = users.Select(u => new { id = u.Id, name = u.UserName }) });
                }
                else if (currentUserRoles.Contains("Teacher"))
                {
                    // For teacher: return only students
                    var students = await _userManager.GetUsersInRoleAsync("Student");
                    _logger.LogInformation($"Found {students.Count} students");
                    return Ok(new { success = true, users = students.Select(u => new { id = u.Id, name = u.UserName }) });
                }
                else if (currentUserRoles.Contains("Student"))
                {
                    // For students: return only teachers
                    var teachers = await _userManager.GetUsersInRoleAsync("Teacher");
                    _logger.LogInformation($"Found {teachers.Count} teachers");
                    return Ok(new { success = true, users = teachers.Select(u => new { id = u.Id, name = u.UserName }) });
                }
                else
                {
                    _logger.LogWarning($"User {currentUser.UserName} has unsupported roles: {string.Join(", ", currentUserRoles)}");
                    return Ok(new { success = false, message = "User has unsupported roles" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUsers endpoint");
                return StatusCode(500, new { success = false, message = "An error occurred while fetching users" });
            }
        }
    }
} 