using AdvancedDevSample.Api.Controllers;
using AdvancedDevSample.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace AdvancedDevSample.Test.API.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly IConfiguration _configuration;

        public AuthControllerTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "test-secret-key-minimum-32-characters-long-for-hmac-sha256"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            _controller = new AuthController(_configuration);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "admin",
                Password = "password"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            
            var tokenProperty = okResult.Value.GetType().GetProperty("token");
            Assert.NotNull(tokenProperty);
            var token = tokenProperty.GetValue(okResult.Value) as string;
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public void Login_WithInvalidUsername_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "wronguser",
                Password = "password"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Identifiants incorrects.", unauthorizedResult.Value);
        }

        [Fact]
        public void Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "admin",
                Password = "wrongpassword"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Identifiants incorrects.", unauthorizedResult.Value);
        }

        [Fact]
        public void GeneratedToken_ContainsCorrectClaims()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "admin",
                Password = "password"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenProperty = okResult.Value?.GetType().GetProperty("token");
            var token = tokenProperty?.GetValue(okResult.Value) as string;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "admin");
            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);
            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Equal("TestAudience", jwtToken.Audiences.First());
        }

        [Fact]
        public void GeneratedToken_ExpiresIn30Minutes()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "admin",
                Password = "password"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenProperty = okResult.Value?.GetType().GetProperty("token");
            var token = tokenProperty?.GetValue(okResult.Value) as string;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var expirationTime = jwtToken.ValidTo;
            var expectedExpiration = DateTime.UtcNow.AddMinutes(30);

            // Allow 1 minute tolerance
            Assert.True(expirationTime > DateTime.UtcNow.AddMinutes(29));
            Assert.True(expirationTime < DateTime.UtcNow.AddMinutes(31));
        }
    }
}
