using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Application.Services
{
    public class FournisseurService
    {
        private readonly IFournisseurRepository _fournisseurRepository;

        public FournisseurService(IFournisseurRepository fournisseurRepository)
        {
            _fournisseurRepository = fournisseurRepository;
        }

        public FournisseurDto CreateFournisseur(CreateFournisseurRequest request)
        {
            var fournisseur = new Fournisseur(request.Name, request.ContactEmail);
            _fournisseurRepository.Save(fournisseur);
            return ToDto(fournisseur);
        }

        public FournisseurDto GetFournisseur(Guid id)
        {
            var fournisseur = _fournisseurRepository.GetById(id);
            if (fournisseur == null) throw new DomainException("Fournisseur non trouvé.");
            return ToDto(fournisseur);
        }

        public IEnumerable<FournisseurDto> GetAllFournisseurs()
        {
            return _fournisseurRepository.GetAll().Select(ToDto);
        }

        public void DeleteFournisseur(Guid id)
        {
             _fournisseurRepository.Delete(id);
        }

        private static FournisseurDto ToDto(Fournisseur fournisseur)
        {
            return new FournisseurDto(fournisseur.Id, fournisseur.Name, fournisseur.ContactEmail);
        }
    }
}
