using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    /// <summary>
    /// Gère les opérations liées aux commandes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CommandeController : ControllerBase
    {
        private readonly CommandeService _commandeService;

        public CommandeController(CommandeService commandeService)
        {
            _commandeService = commandeService;
        }

        /// <summary>
        /// Crée une nouvelle commande pour un client.
        /// </summary>
        /// <param name="request">Les informations pour créer la commande.</param>
        /// <returns>La commande créée.</returns>
        /// <response code="201">Commande créée avec succès.</response>
        /// <response code="400">Si la requête est invalide.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CommandeDto), 201)]
        [ProducesResponseType(400)]
        public IActionResult CreateCommande([FromBody] CreateCommandeRequest request)
        {
            try
            {
                var commande = _commandeService.CreateCommande(request);
                return CreatedAtAction(nameof(GetCommande), new { id = commande.Id }, commande);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Récupère une commande par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de la commande.</param>
        /// <returns>La commande demandée.</returns>
        /// <response code="200">Commande trouvée.</response>
        /// <response code="404">Commande non trouvée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CommandeDto), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetCommande(Guid id)
        {
            try
            {
                var commande = _commandeService.getCommande(id);
                return Ok(commande);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Ajoute un article à une commande existante.
        /// </summary>
        /// <param name="id">L'identifiant de la commande.</param>
        /// <param name="request">Les détails de l'article à ajouter.</param>
        /// <returns>Aucun contenu.</returns>
        /// <response code="204">Article ajouté avec succès.</response>
        /// <response code="400">Si la commande n'est pas modifiable ou produit invalide.</response>
        [HttpPost("{id}/items")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddItem(Guid id, [FromBody] AddItemRequest request)
        {
            try
            {
                _commandeService.AddItem(id, request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retire un article d'une commande.
        /// </summary>
        /// <param name="id">L'identifiant de la commande.</param>
        /// <param name="productId">L'identifiant du produit à retirer.</param>
        /// <returns>Aucun contenu.</returns>
        /// <response code="204">Article retiré avec succès.</response>
        /// <response code="400">Si l'opération échoue.</response>
        [HttpDelete("{id}/items/{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult RemoveItem(Guid id, Guid productId)
        {
            try
            {
                _commandeService.RemoveItem(id, productId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Valide une commande (passage au statut Validated).
        /// </summary>
        /// <param name="id">L'identifiant de la commande.</param>
        /// <returns>Aucun contenu.</returns>
        /// <response code="204">Commande validée avec succès.</response>
        /// <response code="400">Si la validation échoue.</response>
        [HttpPost("{id}/validate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult ValidateCommande(Guid id)
        {
            try
            {
                _commandeService.ValidateCommande(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Annule une commande.
        /// </summary>
        /// <param name="id">L'identifiant de la commande.</param>
        /// <returns>Aucun contenu.</returns>
        /// <response code="204">Commande annulée avec succès.</response>
        /// <response code="400">Si l'annulation est impossible.</response>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CancelCommande(Guid id)
        {
            try
            {
                _commandeService.CancelCommande(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
