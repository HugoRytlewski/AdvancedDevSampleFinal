using AdvancedDevSample.Api.Middlewares;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text.Json;
using Xunit;

namespace AdvancedDevSample.Test.API.Middlewares
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
        private readonly DefaultHttpContext _httpContext;

        public ExceptionHandlingMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();
        }

        [Fact]
        public async Task DomainException_Returns400BadRequest()
        {
            // Arrange
            var exception = new DomainException("Le prix doit etre positif.");
            
            RequestDelegate next = (HttpContext context) =>
            {
                throw exception;
            };

            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

            // Act
            await middleware.Invoke(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, _httpContext.Response.StatusCode);
            Assert.StartsWith("application/json", _httpContext.Response.ContentType);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            Assert.Contains("Erreur métier", responseBody);
            Assert.Contains("Le prix doit etre positif", responseBody);
        }

        [Fact]
        public async Task ApplicationServiceException_ReturnsCorrectStatusCode()
        {
            // Arrange
            var exception = new ApplicationServiceException("Produit introuvable.")
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };
            
            RequestDelegate next = (HttpContext context) =>
            {
                throw exception;
            };

            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

            // Act
            await middleware.Invoke(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, _httpContext.Response.StatusCode);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            Assert.Contains("Ressource introuvable", responseBody);
            Assert.Contains("Produit introuvable", responseBody);
        }

        [Fact]
        public async Task InfrastructureException_Returns500InternalServerError()
        {
            // Arrange
            var exception = new InfrastructureException("Database connection failed.");
            
            RequestDelegate next = (HttpContext context) =>
            {
                throw exception;
            };

            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

            // Act
            await middleware.Invoke(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, _httpContext.Response.StatusCode);
            Assert.StartsWith("application/json", _httpContext.Response.ContentType);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            Assert.Contains("Erreur technique", responseBody);
        }

        [Fact]
        public async Task GenericException_Returns500InternalServerError()
        {
            // Arrange
            var exception = new InvalidOperationException("Unexpected error");
            
            RequestDelegate next = (HttpContext context) =>
            {
                throw exception;
            };

            var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

            // Act
            await middleware.Invoke(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, _httpContext.Response.StatusCode);
            Assert.StartsWith("application/json", _httpContext.Response.ContentType);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            Assert.Contains("Erreur serveur", responseBody);
            Assert.Contains("Une erreur interne", responseBody);
        }
    }
}
