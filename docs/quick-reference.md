# Quick Reference

Reference rapide des commandes, endpoints et structures essentielles.

---

## Commandes

### Build & Run

```bash
# Build le projet
dotnet build

# Lancer l'API
cd AdvancedDevSample.Api
dotnet run

# Lancer en mode watch (auto-reload)
dotnet watch run

# Build en Release
dotnet build --configuration Release
```

### Tests

```bash
# Lancer tous les tests
dotnet test

# Tests avec couverture
dotnet-coverage collect "dotnet test" -f xml -o coverage.xml

# Tests avec verbosity
dotnet test --verbosity detailed

# Tests d'un projet specifique
cd AdvancedDevSample.Test
dotnet test
```

### Nettoyage

```bash
# Nettoyer les artefacts de build
dotnet clean

# Supprimer bin/ et obj/
find . -name "bin" -o -name "obj" | xargs rm -rf
```

---

## Endpoints API

### Base URL

```
https://localhost:5001
```

### Authentification

| Methode | Endpoint | Description |
|---------|----------|-------------|
| `POST` | `/api/auth/login` | Obtenir un token JWT |

**Body :**
```json
{
  "username": "admin",
  "password": "password"
}
```

**Response :**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

### Produits

> Tous les endpoints produits necessitent un token JWT valide.

| Methode | Endpoint | Description |
|---------|----------|-------------|
| `GET` | `/api/products` | Liste tous les produits |
| `GET` | `/api/products/{id}` | Details d'un produit |
| `POST` | `/api/products` | Creer un produit |
| `PUT` | `/api/products/{id}/price` | Modifier le prix |
| `DELETE` | `/api/products/{id}` | Supprimer (soft delete) |

---

## DTOs

### LoginRequest

```csharp
{
  "username": "string",
  "password": "string"
}
```

### CreateProductRequest

```csharp
{
  "name": "string",          // Requis, non vide
  "price": 99.99,            // Requis, > 0
  "currency": "EUR"          // Requis (EUR, USD, GBP)
}
```

### ChangePriceRequest

```csharp
{
  "newPrice": 149.99         // Requis, > 0
}
```

### ProductDto

```csharp
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "iPhone 15 Pro",
  "price": 999.99,
  "currency": "EUR",
  "isActive": true
}
```

---

## Codes HTTP

| Code | Signification | Quand? |
|------|---------------|--------|
| **200** | OK | Requete reussie (GET) |
| **201** | Created | Ressource creee (POST) |
| **204** | No Content | Modification/Suppression reussie |
| **400** | Bad Request | Donnees invalides |
| **401** | Unauthorized | Token manquant ou invalide |
| **404** | Not Found | Ressource introuvable |
| **500** | Internal Server Error | Erreur serveur |

---

## Variables d'environnement

### appsettings.json

```json
{
  "Jwt": {
    "Key": "VotreCleSecrete32CaracteresMinimum",
    "Issuer": "AdvancedDevSample",
    "Audience": "AdvancedDevSampleUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

## Architecture en 1 coup d'oeil

```
Client
  ↓
API Controller (+ JWT Validation)
  ↓
Application Service
  ↓
Domain Entity / Value Object
  ↓
Repository Interface
  ↓
Infrastructure Repository
  ↓
Database (In-Memory)
```

---

## Raccourcis Swagger

| Action | Raccourci |
|--------|-----------|
| Authorize | Cliquer sur le cadenas |
| Try it out | Bouton bleu sur l'endpoint |
| Execute | Bouton "Execute" apres "Try it out" |

**Header Authorization :**
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## Liens utiles

| Ressource | Lien |
|-----------|------|
| **API Swagger** | [https://localhost:5001/swagger](https://localhost:5001/swagger) |
| **GitHub** | [Repository](https://github.com/HugoRytlewski/AdvancedDevSampleFinal) |
| **SonarCloud** | [Dashboard](https://sonarcloud.io/project/overview?id=HugoRytlewski_AdvancedDevSampleFinal) |
| **CI/CD** | [GitHub Actions](https://github.com/HugoRytlewski/AdvancedDevSampleFinal/actions) |

---

## Tests rapides

```bash
# Test complet
dotnet test

# Nombre de tests
# Total: 46 tests
# - Domain: 15 tests
# - Application: 12 tests
# - API Unit: 9 tests  
# - API Integration: 10 tests

# Couverture
# ~80% de couverture de code
```
