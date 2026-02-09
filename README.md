# AdvancedDevSample

## 📋 Description

**AdvancedDevSample** est une API Web construite avec **.NET 10** qui démontre les principes de la **Clean Architecture** et du **Domain-Driven Design (DDD)**. Ce projet sert d'exemple pour une structuration robuste d'application backend, séparant clairement les responsabilités entre la présentation, la logique métier et l'infrastructure.

L'application gère des produits et leurs prix, offrant des fonctionnalités pour consulter et modifier les informations de tarification via une API RESTful.

## 🏗ure Architecture

La solution suit une approche **Clean Architecture** divisée en 4 couches principales :

1.  **AdvancedDevSample.Api (Présentation)** :
    *   Point d'entrée de l'application (ASP.NET Core Web API).
    *   Contient les `Controllers` qui exposent les endpoints REST.
    *   Gère la configuration (Swagger, DI) et les Middlewares (ex: `ExceptionHandlingMiddleware`).
2.  **AdvancedDevSample.Application (Couche Application)** :
    *   Orchestre la logique métier via des Services (`ProductService`).
    *   Définit les DTOs (Data Transfer Objects) pour les échanges de données.
    *   Dépend uniquement du Domaine.
3.  **AdvancedDevSampleDomain (Cœur du Domaine)** :
    *   Contient les entités métier (`Product`), les Value Objects et les Exceptions personnalisées.
    *   Définit les interfaces essentielles (ex: `IProductRepository`) pour l'inversion de dépendance.
    *   Ne dépend d'aucune autre couche technique.
4.  **AdvancedDevSample.Infrastructure (Infrastructure)** :
    *   Implémente les interfaces définies dans le Domaine (ex: `EfProductRepository`).
    *   Gère l'accès aux données avec **Entity Framework Core**.

## 🚀 Fonctionnalités Clés

*   **Gestion des Prix Produits** :
    *   `GET /api/products/{id}` : Récupère le prix actuel d'un produit.
    *   `PUT /api/products/{id}/price` : Met à jour le prix d'un produit.
*   **Documentation OpenAPI** : Interface Swagger intégrée pour explorer et tester l'API.
*   **Gestion des Erreurs** : Middleware global pour normaliser les réponses d'erreur (404 NotFound, 400 BadRequest).

## 🛠 Technologies

*   **Framework** : .NET 10.0
*   **Type** : ASP.NET Core Web API
*   **ORM** : Entity Framework Core
*   **Documentation** : Swashbuckle (Swagger UI)

## 🏁 Pour Commencer

### Prérequis

Assurez-vous d'avoir installé :
*   [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
*   Un IDE comme Visual Studio 2022, JetBrains Rider ou VS Code.

### Installation

1.  Clonez le dépôt :
    ```bash
    git clone <votre-url-repo>
    cd AdvancedDevSample
    ```

2.  Restaurez les dépendances :
    ```bash
    dotnet restore
    ```

### Exécution

Pour lancer l'API en local :

```bash
dotnet run --project AdvancedDevSample.Api
```

L'application démarrera généralement sur `http://localhost:5000` ou `https://localhost:5001`.
Accédez à l'interface Swagger pour tester les endpoints :
👉 `https://localhost:7198/swagger/index.html` (le port peut varier selon votre configuration `launchSettings.json`).

### Tests

Pour exécuter les tests unitaires :

```bash
dotnet test
```

## 📄 Licence

Ce projet est sous licence [MIT](LICENSE).
