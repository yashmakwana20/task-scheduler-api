using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.DTOs;
using TaskManagement.Models;
using TaskManagement.Services.Common;

namespace TaskManagement.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly JwtService _jwt;

        public AuthController(AppDbContext context, JwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new Users
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                UserRole = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Ok(new { IsError = true, Message = "Invalid credentials" });
            }

            var token = _jwt.GenerateToken(user);
            var refreshToken = _jwt.GenerateRefreshToken(user);
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return Ok(new { AccessToken = token, RefreshToken = refreshToken });
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(TokenRequest objTokenReq)
        {
            var storedToken = await _context.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == objTokenReq.RefreshToken);

            if (storedToken == null)
                return Unauthorized("Invalid Refresh Token");

            if (storedToken.IsRevoked)
                return Unauthorized("Token Already Revoked");

            if (storedToken.ExpiryDate < DateTime.UtcNow)
                return Unauthorized("Token Expired");

            storedToken.IsRevoked = true;

            var newAccessToken = _jwt.GenerateToken(storedToken.User);
            var newRefreshToken = _jwt.GenerateRefreshToken(storedToken.User);

            _context.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                UserId = storedToken.UserId,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut(TokenRequest objReqToken)
        {
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == objReqToken.RefreshToken);

            if (storedToken == null)
                return BadRequest("Token Not Found");

            storedToken.IsRevoked = true;
            await _context.SaveChangesAsync();

            return Ok("Logged Out Successfully");

        }
    }
}
