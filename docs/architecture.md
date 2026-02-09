# Architecture

Vue d'ensemble de l'architecture du projet AdvancedDevSample.

## Architecture en couches

Le projet suit une **architecture hexagonale** (Clean Architecture) avec separation claire des responsabilites.

```
AdvancedDevSample/
├── AdvancedDevSample.Api/           # Couche Presentation
├── AdvancedDevSample.Application/   # Couche Application
├── AdvancedDevSample.Domain/        # Couche Domaine
├── AdvancedDevSample.Infrastructure/# Couche Infrastructure
└── AdvancedDevSample.Test/          # Tests
```

## Couches

### Api (Presentation)

**Responsabilites:**

- Exposition des endpoints REST
- Configuration Swagger/OpenAPI
- Authentification JWT
- Middleware de gestion des erreurs

**Fichiers cles:**

- `Controllers/` - Controleurs REST
- `Program.cs` - Configuration de l'application
- `Middlewares/` - Middleware personnalises

---

### Application

**Responsabilites:**

- DTOs (Data Transfer Objects)
- Services applicatifs
- Orchestration des cas d'utilisation

**Fichiers cles:**

- `DTOs/` - Objets de transfert de donnees
- `Services/` - Services applicatifs

---

### Domain

**Responsabilites:**

- Entites metier
- Value Objects
- Regles de domaine (invariants)
- Interfaces de repository

**Fichiers cles:**

- `Entities/Product.cs` - Entite Produit
- `ValueObjects/Price.cs` - Value Object Prix
- `Interfaces/` - Contrats de repository
- `Exceptions/` - Exceptions metier

---

### Infrastructure

**Responsabilites:**

- Implementation des repositories
- Acces aux donnees (Entity Framework)
- Services externes

---

### Test

**Types de tests:**

| Type | Dossier | Description |
|------|---------|-------------|
| Unitaires | `Domain/` | Tests des entites et value objects |
| Unitaires | `Application/` | Tests des services avec fakes |
| Integration | `API/Integration/` | Tests des endpoints avec WebApplicationFactory |

---

## Flux d'une requete

```
HTTP Request
    |
[Middleware Auth JWT]
    |
[Controller]
    |
[ProductService]
    |
[IProductRepository]
    |
[EfProductRepository]
    |
Database
```

## Principes SOLID

| Principe | Description | Application |
|----------|-------------|-------------|
| **S** - Single Responsibility | Une classe = une responsabilite | `ProductService` gere uniquement les produits |
| **O** - Open/Closed | Ouvert a l'extension, ferme a la modification | Interfaces pour les repositories |
| **L** - Liskov Substitution | Les sous-types respectent les contrats | `EfProductRepository` implemente `IProductRepository` |
| **I** - Interface Segregation | Interfaces specifiques et focalisees | `IProductRepository` vs interface generique |
| **D** - Dependency Inversion | Dependre des abstractions | Injection de `IProductRepository` |

## Clean Code

### Regles appliquees

| Regle | Description |
|-------|-------------|
| Noms explicites | `GetProductById` vs `Get` |
| Fonctions courtes | Maximum 20 lignes par methode |
| Un niveau d'abstraction | Chaque fonction opere a un seul niveau |
| Pas de commentaires inutiles | Le code s'auto-documente |
| Gestion des erreurs | Exceptions specifiques au domaine |

### Metriques de qualite (SonarCloud)

| Metrique | Objectif |
|----------|----------|
| Couverture de code | > 80% |
| Code Smells | 0 bloquants |
| Bugs | 0 |
| Vulnerabilites | 0 |
| Dette technique | < 1 jour |
| Duplications | < 3% |

## CI/CD Pipeline

| Etape | Outil | Description |
|-------|-------|-------------|
| Build | .NET SDK | Compilation du projet |
| Tests | xUnit | Execution des tests unitaires et integration |
| Analyse | SonarCloud | Analyse statique du code |
| Documentation | MkDocs | Generation de la documentation |
| Deploiement | GitHub Pages | Publication de la documentation |
