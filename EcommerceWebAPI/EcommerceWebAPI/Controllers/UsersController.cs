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

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<Users>> GetUserByEmail(string email, string password)
        {
            var user = await _userService.GetUserByEmail(email, password);
            if (user == null)
            {
                return NotFound("User not found with entered email !");
            }
            return Ok(user);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Users>> GetPerticularUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound($"No user found with id {id} !");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<Users>> AddUser(Users user)
        {
            var newUser = await _userService.AddUser(user);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = newUser.Email }, newUser);
        }

        [HttpPut("{email}")]
        public async Task<ActionResult<Users>> UpdateUser(string email, Users user)
        {
            var updatedUser = await _userService.UpdateUser(email, user);
            return Ok(updatedUser);
        }
    }
}
