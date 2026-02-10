using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    /// <summary>
    /// Gère les opérations liées aux fournisseurs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FournisseurController : ControllerBase
    {
        private readonly FournisseurService _fournisseurService;

        public FournisseurController(FournisseurService fournisseurService)
        {
            _fournisseurService = fournisseurService;
        }

        /// <summary>
        /// Crée un nouveau fournisseur.
        /// </summary>
        /// <param name="request">Les informations du fournisseur à créer.</param>
        /// <returns>Le fournisseur créé.</returns>
        /// <response code="201">Fournisseur créé avec succès.</response>
        [HttpPost]
        [ProducesResponseType(typeof(FournisseurDto), 201)]
        public IActionResult CreateFournisseur([FromBody] CreateFournisseurRequest request)
        {
            var fournisseur = _fournisseurService.CreateFournisseur(request);
            return CreatedAtAction(nameof(GetFournisseur), new { id = fournisseur.Id }, fournisseur);
        }

        /// <summary>
        /// Récupère un fournisseur par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du fournisseur.</param>
        /// <returns>Le fournisseur demandé.</returns>
        /// <response code="200">Fournisseur trouvé.</response>
        /// <response code="404">Fournisseur non trouvé.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FournisseurDto), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetFournisseur(Guid id)
        {
            try
            {
                var fournisseur = _fournisseurService.GetFournisseur(id);
                return Ok(fournisseur);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Récupère la liste de tous les fournisseurs.
        /// </summary>
        /// <returns>Une liste de fournisseurs.</returns>
        /// <response code="200">Liste récupérée avec succès.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FournisseurDto>), 200)]
        public IActionResult GetAllFournisseurs()
        {
            return Ok(_fournisseurService.GetAllFournisseurs());
        }

        /// <summary>
        /// Supprime un fournisseur.
        /// </summary>
        /// <param name="id">L'identifiant du fournisseur à supprimer.</param>
        /// <returns>Aucun contenu.</returns>
        /// <response code="204">Fournisseur supprimé avec succès.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult DeleteFournisseur(Guid id)
        {
            _fournisseurService.DeleteFournisseur(id);
            return NoContent();
        }
    }
}
