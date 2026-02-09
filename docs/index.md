# AdvancedDevSample API

Documentation technique de l'API AdvancedDevSample.

## Demarrage rapide

### Prerequis

- .NET 10.0 SDK
- Un editeur de code (VS Code, Rider, Visual Studio)

### Lancer l'API

```bash
cd AdvancedDevSample.Api
dotnet run
```

L'API sera disponible sur : `https://localhost:5001`

### Acceder a Swagger

Une fois l'API lancee, ouvrez : [https://localhost:5001/swagger](https://localhost:5001/swagger)

## Authentification

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

## Structure du projet

| Couche | Description |
|--------|-------------|
| **Api** | Controleurs REST, configuration Swagger |
| **Application** | DTOs, Services applicatifs |
| **Domain** | Entites, Value Objects, Regles metier |
| **Infrastructure** | Repositories, Acces aux donnees |

## Tests

```bash
cd AdvancedDevSample.Test
dotnet test
```

## Qualite du code

### SonarCloud

Le projet utilise SonarCloud pour l'analyse statique du code.

**Metriques analysees :**

- Couverture de code
- Bugs potentiels
- Code smells
- Vulnerabilites de securite
- Dette technique
- Duplications de code

**Acces au dashboard :**

```
https://sonarcloud.io/project/overview?id=HugoRytlewski_AdvancedDevSampleFinal
```

### Principes Clean Code appliques

| Principe | Application |
|----------|-------------|
| **Single Responsibility** | Chaque classe a une seule responsabilite |
| **Open/Closed** | Extension via interfaces, modification fermee |
| **Liskov Substitution** | Les implementations respectent les contrats |
| **Interface Segregation** | Interfaces specifiques (IProductRepository) |
| **Dependency Inversion** | Injection de dependances via constructeur |

### Conventions de nommage

| Element | Convention | Exemple |
|---------|------------|---------|
| Classes | PascalCase | `ProductService` |
| Methodes | PascalCase | `GetAllProducts()` |
| Variables | camelCase | `productList` |
| Constantes | UPPER_SNAKE | `MAX_RETRY_COUNT` |
| Interfaces | I + PascalCase | `IProductRepository` |
