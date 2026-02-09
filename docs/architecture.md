# Architecture

Vue d'ensemble de l'architecture du projet AdvancedDevSample.

## 🏗️ Architecture en couches

Le projet suit une **architecture hexagonale** (Clean Architecture) avec séparation claire des responsabilités.

```
AdvancedDevSample/
├── AdvancedDevSample.Api/           # Couche Présentation
├── AdvancedDevSample.Application/   # Couche Application
├── AdvancedDevSample.Domain/        # Couche Domaine
├── AdvancedDevSample.Infrastructure/# Couche Infrastructure
└── AdvancedDevSample.Test/          # Tests
```

## 📦 Couches

### Api (Présentation)

**Responsabilités:**

- Exposition des endpoints REST
- Configuration Swagger/OpenAPI
- Authentification JWT
- Middleware de gestion des erreurs

**Fichiers clés:**

- `Controllers/` - Contrôleurs REST
- `Program.cs` - Configuration de l'application
- `Middlewares/` - Middleware personnalisés

---

### Application

**Responsabilités:**

- DTOs (Data Transfer Objects)
- Services applicatifs
- Orchestration des cas d'utilisation

**Fichiers clés:**

- `DTOs/` - Objets de transfert de données
- `Services/` - Services applicatifs

---

### Domain

**Responsabilités:**

- Entités métier
- Value Objects
- Règles de domaine (invariants)
- Interfaces de repository

**Fichiers clés:**

- `Entities/Product.cs` - Entité Produit
- `ValueObjects/Price.cs` - Value Object Prix
- `Interfaces/` - Contrats de repository
- `Exceptions/` - Exceptions métier

---

### Infrastructure

**Responsabilités:**

- Implémentation des repositories
- Accès aux données (Entity Framework)
- Services externes

---

### Test

**Types de tests:**

| Type | Dossier | Description |
|------|---------|-------------|
| Unitaires | `Domain/` | Tests des entités et value objects |
| Unitaires | `Application/` | Tests des services avec fakes |
| Intégration | `API/Integration/` | Tests des endpoints avec WebApplicationFactory |

---

## 🔄 Flux d'une requête

```
HTTP Request
    ↓
[Middleware Auth JWT]
    ↓
[Controller]
    ↓
[ProductService]
    ↓
[IProductRepository]
    ↓
[EfProductRepository]
    ↓
Database
```

## 🛡️ Principes appliqués

- **DDD** (Domain-Driven Design) - Entités riches, Value Objects
- **SOLID** - Séparation des responsabilités
- **Dependency Injection** - Couplage faible via interfaces
