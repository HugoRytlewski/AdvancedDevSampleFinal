using AdvancedDevSample.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdvancedDevSample.Api.Controllers
{
    /// <summary>
    /// Contrôleur pour l'authentification et la génération de tokens JWT.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Authentifie un utilisateur et retourne un token JWT.
        /// </summary>
        /// <param name="request">Les identifiants de connexion (username et password).</param>
        /// <returns>Un token JWT valide pour 30 minutes.</returns>
        /// <remarks>
        /// Exemple de requête:
        /// 
        ///     POST /api/auth/login
        ///     {
        ///         "username": "admin",
        ///         "password": "password"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Authentification réussie. Retourne le token JWT.</response>
        /// <response code="401">Identifiants incorrects.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simulation d'une vérification utilisateur (Hardcoded pour l'exemple)
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = GenerateJwtToken(request.Username);
                return Ok(new { token });
            }

            return Unauthorized("Identifiants incorrects.");
        }

        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "votre-cle-super-secrete-de-32-caracteres-minimum"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
