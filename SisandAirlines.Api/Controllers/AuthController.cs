using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SisandAirlines.Api.Models.Auth;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces;

namespace SisandAirlines.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthController(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existing = await _userRepository.GetByEmailAsync(request.Email);
            if (existing is not null)
                return BadRequest("E-mail já cadastrado.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _unitOfWork.BeginAsync();
            try
            {
                var user = new User(
                    request.FullName,
                    request.Email,
                    request.Cpf,
                    request.BirthDate,
                    passwordHash
                );

                await _userRepository.AddAsync(user);
                await _unitOfWork.CommitAsync();

                return Created("", null);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
                return Unauthorized("Usuário ou senha inválidos.");

            var isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValidPassword)
                return Unauthorized("Usuário ou senha inválidos.");

            var token = GenerateJwtToken(user, out DateTime expiresAt);

            var response = new AuthResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                FullName = user.FullName,
                Email = user.Email,
                 UserId = user.Id  
            };

            return Ok(response);
        }

        private string GenerateJwtToken(User user, out DateTime expiresAt)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings["Key"]!;
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiresInMinutes = int.Parse(jwtSettings["ExpiresInMinutes"]!);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("fullName", user.FullName)
            };

            expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
