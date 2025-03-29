using DWeb_MVC.Data;
using DWeb_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[ApiController]
[Route("api/register")]
public class RegisterController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RegisterController> _logger;

    public RegisterController(UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager, 
        ApplicationDbContext context, 
        ILogger<RegisterController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password.");

            user.EmailConfirmed = true;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                _logger.LogError("Error while updating user");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var p = new Clientes
            {
                UserId = user.Id,
                Nome = request.Nome,
                Telemovel = request.Telemovel,
                Email = request.Email,
                Morada = request.Morada,
                CodPostal = request.CodPostal
            };

            try
            {
                _context.Clientes.Add(p);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating user");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            return Ok("Register created successfully.");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return BadRequest(ModelState);
    }

    public class RegisterRequest
    {
        public string Nome { get; set; }
        public string Telemovel { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Morada { get; set; }
        public string CodPostal { get; set; }
    }
}
