using AdvancedDevSample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AdvancedDevSample.Domain.Services;
using AdvancedDevSample.Application.DTOs;
using System.Net;

namespace AdvancedDevSample.Api.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des produits.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/products")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Récupère la liste de tous les produits.
        /// </summary>
        /// <returns>Liste des produits.</returns>
        /// <response code="200">Retourne la liste des produits.</response>
        /// <response code="401">Non autorisé - Token JWT manquant ou invalide.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetAll()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }

        /// <summary>
        /// Crée un nouveau produit.
        /// </summary>
        /// <param name="request">Les données du produit à créer.</param>
        /// <returns>L'identifiant du produit créé.</returns>
        /// <response code="201">Produit créé avec succès.</response>
        /// <response code="400">Données invalides (prix négatif, nom vide, etc.).</response>
        /// <response code="401">Non autorisé.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Create([FromBody] CreateProductRequest request)
        {
            try
            {
                var id = _productService.CreateProduct(request);
                return CreatedAtAction(nameof(GetById), new { id = id }, new { id = id });
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Supprime un produit par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant unique du produit.</param>
        /// <response code="204">Produit supprimé avec succès.</response>
        /// <response code="401">Non autorisé.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Delete(Guid id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }

        /// <summary>
        /// Modifie le prix d'un produit existant.
        /// </summary>
        /// <param name="id">L'identifiant unique du produit.</param>
        /// <param name="request">Le nouveau prix.</param>
        /// <response code="204">Prix modifié avec succès.</response>
        /// <response code="400">Prix invalide (négatif ou nul).</response>
        /// <response code="404">Produit non trouvé.</response>
        /// <response code="401">Non autorisé.</response>
        [HttpPut("{id}/price")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Récupère un produit par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant unique du produit.</param>
        /// <returns>Les détails du produit.</returns>
        /// <response code="200">Retourne le produit demandé.</response>
        /// <response code="404">Produit non trouvé.</response>
        /// <response code="401">Non autorisé.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetById(Guid id)
        {
            try
            {
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
