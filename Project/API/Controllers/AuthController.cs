using API.Entities.Models;
using API.Entities.ViewModels.User;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _manager;
    private readonly IConfiguration _configuration;

    public AuthController(
        ILogger<AuthController> logger,
        IMapper mapper,
        UserManager<ApplicationUser> manager,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _mapper = mapper;
        _manager = manager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserRegister userRegister)
    {
        _logger.LogInformation($"Registration attempt for {userRegister.Email}");

        try
        {
            var user = _mapper.Map<ApplicationUser>(userRegister);
            user.UserName = userRegister.Email;
            var result = await _manager.CreateAsync(user, userRegister.Password);

            if (result.Succeeded == false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _manager.AddToRoleAsync(user, "User");
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)}");
            return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login(UserLogin userLogin)
    {
        _logger.LogInformation($"Login Attempt for {userLogin.Email} ");
        try
        {
            var user = await _manager.FindByEmailAsync(userLogin.Email);
            var passwordValid = await _manager.CheckPasswordAsync(user, userLogin.Password);

            if (user == null || passwordValid == false)
            {
                return Unauthorized(userLogin);
            }

            string tokenString = await GenerateToken(user);

            var response = new AuthResponse
            {
                Email = userLogin.Email,
                Token = tokenString,
                UserId = user.Id,
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)}");
            return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
        }
    }

    private async Task<string> GenerateToken(ApplicationUser user)
    {
        var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]));
        var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
        var roles = await _manager.GetRolesAsync(user);
        var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();
        var userClaims = await _manager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(CustomClaimTypes.Uid, user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWTSettings:Issuer"],
            audience: _configuration["JWTSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JWTSettings:Duration"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
