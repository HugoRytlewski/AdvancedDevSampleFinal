using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.ValueObjects;


namespace AdvancedDevSample.Domain.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<ProductDto> GetAllProducts()
        {
            return _repo.GetAll().Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price.Value,
                Currency = p.Price.Currency,
                IsActive = p.IsActive
            });
        }

        public Guid CreateProduct(CreateProductRequest request)
        {
            var price = new Price(request.Price, request.Currency);
            var product = new Product(request.Name, price);
            _repo.Save(product);
            return product.Id;
        }

        public void DeleteProduct(Guid id)
        {
            _repo.Delete(id);
        }

        public void ChangeProductPrice(Guid productId, decimal newPriceValue)
        {
            var product = GetProduct(productId);

            product.ChangePrice(newPriceValue); // Nota: ceci lève une erreur NotImplemented actu dans Product.cs, il faut utiliser l'autre méthode ou corriger Product.cs

            _repo.Save(product);
        }

        private Product GetProduct(Guid id)
        {
            return _repo.GetById(id)
                ?? throw new ApplicationServiceException("Produit non trouvé.");
        }

        public void ChangePrice(Guid id, ChangePriceRequest request)
        {
            var product = GetProduct(id);
            var newPrice = new Price(request.NewPrice, "EUR"); // Assumer EUR ou ajouter la devise à la requête
            product.ChangePrice(newPrice);
            _repo.Save(product);
        }

        public ProductDto GetProductDto(Guid id) {
            var product = GetProduct(id);
            return new ProductDto {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price.Value,
                Currency = product.Price.Currency,
                IsActive = product.IsActive
            };
        }
    }
}
