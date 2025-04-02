using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TasksServer.Models;
using TasksServer.Repositories.Abstractions;

namespace TasksServer.Controllers;

[ApiController]
public class AuthController(IConfiguration configuration, IUserRepository usersRepository) : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Username = request.Email,
            PasswordHash = hashedPassword
        };
        // Store user in memory
        usersRepository.AddUser(user);
        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = usersRepository.GetUser(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { Message = "Invalid credentials" });
        }
        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenExpiry = refreshToken.Expiry;
        return Ok(new { Token = token, RefreshToken = refreshToken.Token });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshRequest request)
    {
        var user = usersRepository.GetUserByRefreshToken(request.RefreshToken);
        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            return Unauthorized(new { Message = "Invalid refresh token" });
        }
        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenExpiry = refreshToken.Expiry;
        return Ok(new { Token = token, RefreshToken = refreshToken.Token });
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim("id", user.Id)]),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = "TasksClient",
            Issuer = "TasksServer",
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static (string Token, DateTime Expiry) GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        var refreshToken = Convert.ToBase64String(randomBytes);
        var expiryDate = DateTime.UtcNow.AddDays(7); // Set expiry for 7 days (or customize as needed)

        return (Token: refreshToken, Expiry: expiryDate);
    }

}
