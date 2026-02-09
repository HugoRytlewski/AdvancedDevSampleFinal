using System.ComponentModel.DataAnnotations;

namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Requête d'authentification pour obtenir un token JWT.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Nom d'utilisateur.
        /// </summary>
        /// <example>admin</example>
        [Required]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Mot de passe.
        /// </summary>
        /// <example>password</example>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
