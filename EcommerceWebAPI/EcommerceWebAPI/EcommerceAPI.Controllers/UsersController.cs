using EcommerceWebAPI.Models;
using EcommerceWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<Users>> GetUserByEmail(string email, string password)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email, password);
                if (user == null)
                {
                    return NotFound("User not found with entered email !");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user by email {Email}", email);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Users>> GetPerticularUser(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound($"No user found with id {id} !");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user by id {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Users>> AddUser(Users user)
        {
            try
            {
                var newUser = await _userService.AddUser(user);
                return CreatedAtAction(nameof(GetUserByEmail), new { email = newUser.Email }, newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new user.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{email}")]
        public async Task<ActionResult<Users>> UpdateUser(string email, Users user)
        {
            try
            {
                var updatedUser = await _userService.UpdateUser(email, user);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user with email {Email}", email);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
