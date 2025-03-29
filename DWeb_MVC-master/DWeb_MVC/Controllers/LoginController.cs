using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DWeb_MVC.Data;

namespace DWeb_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // POST: api/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, request.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var pessoa = await _context.Clientes.FirstOrDefaultAsync(p => p.UserId == user.Id);
                    if (pessoa != null)
                    {
                        var userWithPerson = new
                        {
                            email = request.Email,
                            person = pessoa
                        };
                        return Ok(userWithPerson);
                    }
                }
            }

            return NotFound();
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}