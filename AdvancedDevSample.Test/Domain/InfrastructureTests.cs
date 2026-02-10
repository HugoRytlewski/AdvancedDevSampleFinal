using Xunit;
using AdvancedDevSample.Domain.Entities;
using System.Reflection;
using System;

namespace AdvancedDevSample.Test.Domain
{
    public class InfrastructureTests
    {
        [Fact]
        public void Product_ProtectedConstructor_ShouldInitialize()
        {
            var product = (Product)Activator.CreateInstance(typeof(Product), true)!;
            Assert.NotNull(product);
        }

        [Fact]
        public void Client_ProtectedConstructor_ShouldInitialize()
        {
            var client = (Client)Activator.CreateInstance(typeof(Client), true)!;
            Assert.NotNull(client);
        }

        [Fact]
        public void Fournisseur_ProtectedConstructor_ShouldInitialize()
        {
            var fournisseur = (Fournisseur)Activator.CreateInstance(typeof(Fournisseur), true)!;
            Assert.NotNull(fournisseur);
        }

        [Fact]
        public void Commande_ProtectedConstructor_ShouldInitialize()
        {
            var commande = (Commande)Activator.CreateInstance(typeof(Commande), true)!;
            Assert.NotNull(commande);
        }

        [Fact]
        public void LigneCommande_ProtectedConstructor_ShouldInitialize()
        {
            var ligne = (LigneCommande)Activator.CreateInstance(typeof(LigneCommande), true)!;
            Assert.NotNull(ligne);
        }
    }
}
