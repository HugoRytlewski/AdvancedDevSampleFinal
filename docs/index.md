# AdvancedDevSample API

Bienvenue dans la documentation de l'API **AdvancedDevSample**.

## 🚀 Démarrage rapide

### Prérequis

- .NET 10.0 SDK
- Un éditeur de code (VS Code, Rider, Visual Studio)

### Lancer l'API

```bash
cd AdvancedDevSample.Api
dotnet run
```

L'API sera disponible sur : `https://localhost:5001`

### Accéder à Swagger

Une fois l'API lancée, ouvrez : [https://localhost:5001/swagger](https://localhost:5001/swagger)

## 🔐 Authentification

L'API utilise **JWT (JSON Web Token)** pour l'authentification.

### Obtenir un token

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "password"}'
```

### Utiliser le token

Ajoutez le token dans le header `Authorization` :

```bash
curl https://localhost:5001/api/products \
  -H "Authorization: Bearer VOTRE_TOKEN"
```

## 📚 Structure du projet

| Couche | Description |
|--------|-------------|
| **Api** | Contrôleurs REST, configuration Swagger |
| **Application** | DTOs, Services applicatifs |
| **Domain** | Entités, Value Objects, Règles métier |
| **Infrastructure** | Repositories, Accès aux données |

## 🧪 Tests

```bash
cd AdvancedDevSample.Test
dotnet test
```
