using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    /// <summary>
    /// Gère les opérations liées aux clients.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Crée un nouveau client.
        /// </summary>
        /// <param name="request">Les informations du client à créer.</param>
        /// <returns>Le client créé.</returns>
        /// <response code="201">Client créé avec succès.</response>
        /// <response code="400">Si les données sont invalides.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ClientDto), 201)]
        [ProducesResponseType(400)]
        public IActionResult CreateClient([FromBody] CreateClientRequest request)
        {
            var client = _clientService.CreateClient(request);
            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        /// <summary>
        /// Récupère un client par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du client.</param>
        /// <returns>Le client demandé.</returns>
        /// <response code="200">Client trouvé.</response>
        /// <response code="404">Client non trouvé.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClientDto), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetClient(Guid id)
        {
            try
            {
                var client = _clientService.GetClient(id);
                return Ok(client);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Récupère la liste de tous les clients.
        /// </summary>
        /// <returns>Une liste de clients.</returns>
        /// <response code="200">Liste récupérée avec succès.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClientDto>), 200)]
        public IActionResult GetAllClients()
        {
            return Ok(_clientService.GetAllClients());
        }

        /// <summary>
        /// Met à jour l'adresse d'un client.
        /// </summary>
        /// <param name="id">L'identifiant du client.</param>
        /// <param name="request">La nouvelle adresse.</param>
        /// <returns>Aucun contenu.</returns>
        /// <response code="204">Adresse mise à jour avec succès.</response>
        /// <response code="404">Client non trouvé.</response>
        [HttpPut("{id}/address")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateClientAddress(Guid id, [FromBody] UpdateClientAddressRequest request)
        {
            try
            {
                _clientService.UpdateClientAddress(id, request.NewAddress);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Supprime un client.
        /// </summary>
        /// <param name="id">L'identifiant du client à supprimer.</param>
        /// <returns>Aucun contenu.</returns>
        /// <response code="204">Client supprimé avec succès.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult DeleteClient(Guid id)
        {
            _clientService.DeleteClient(id);
            return NoContent();
        }
    }
}
