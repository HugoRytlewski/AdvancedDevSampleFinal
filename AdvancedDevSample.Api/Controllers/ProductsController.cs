using AdvancedDevSample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AdvancedDevSample.Domain.Services;
using AdvancedDevSample.Application.DTOs;
using System.Net; 


namespace AdvancedDevSample.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProductRequest request)
        {
            try
            {
                var id = _productService.CreateProduct(request);
                return CreatedAtAction(nameof(GetPrice), new { id = id }, new { id = id });
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
             _productService.DeleteProduct(id);
             return NoContent();
        }

        [HttpPut("{id}/price")]
        public IActionResult ChangePrice(Guid id, [FromBody] ChangePriceRequest request)
        {
            try
            {
                _productService.ChangePrice(id, request);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetPrice(Guid id) {
            try
            {
                // Retourner le DTO complet maintenant
                var productDto = _productService.GetProductDto(id);
                return Ok(productDto);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }


}
