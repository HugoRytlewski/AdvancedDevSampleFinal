# AdvancedDevSample API

Documentation technique de l'API AdvancedDevSample - Une API REST .NET suivant les principes Clean Architecture.

---

## Demarrage rapide

<div class="grid" markdown>

<div class="card" markdown>

### Prerequis

- .NET 10.0 SDK
- Visual Studio / VS Code / Rider

</div>

<div class="card" markdown>

### Lancer l'API

```bash
cd AdvancedDevSample.Api
dotnet run
```

</div>

</div>

L'API sera disponible sur : `https://localhost:5001`

Une fois lancee, accedez a **Swagger UI** : [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

## Authentification

L'API utilise **JWT (JSON Web Token)** pour securiser les endpoints.

=== "Obtenir un token"

    ```bash
    curl -X POST https://localhost:5001/api/auth/login \
      -H "Content-Type: application/json" \
      -d '{"username": "admin", "password": "password"}'
    ```

=== "Utiliser le token"

    ```bash
    curl https://localhost:5001/api/products \
      -H "Authorization: Bearer VOTRE_TOKEN"
    ```

---

## Structure du projet

```
AdvancedDevSample/
|-- Api/              # Controllers, Swagger, JWT
|-- Application/      # DTOs, Services
|-- Domain/           # Entities, Value Objects
|-- Infrastructure/   # Repositories, EF Core
|-- Test/             # Unit & Integration Tests
```

| Couche | Responsabilite |
|--------|----------------|
| **Api** | Exposition REST, authentification |
| **Application** | Orchestration, DTOs |
| **Domain** | Logique metier, invariants |
| **Infrastructure** | Persistance, acces donnees |

---

## Tests

```bash
cd AdvancedDevSample.Test
dotnet test
```

| Type de test | Nombre | Description |
|--------------|--------|-------------|
| **Unitaires (Domain)** | 15 | Entities, Value Objects, Constructeurs |
| **Unitaires (Application)** | 12 | Services, DTOs, Validation |
| **Unitaires (API)** | 9 | Controllers, Middlewares |
| **Integration (API)** | 10 | Endpoints REST End-to-End |
| **Total** | **46** | Couverture **~80%** |

---

## Qualite du code

Le projet integre **SonarCloud** pour l'analyse continue.

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=alert_status&token=229cc30d3067cefd82d4841a3e3b8bb079de8afb)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=coverage&token=229cc30d3067cefd82d4841a3e3b8bb079de8afb)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=HugoRytlewski_AdvancedDevSampleFinal&metric=bugs&token=229cc30d3067cefd82d4841a3e3b8bb079de8afb)](https://sonarcloud.io/summary/new_code?id=HugoRytlewski_AdvancedDevSampleFinal)

[Voir le rapport complet](sonarcloud.md){ .md-button .md-button--primary }

---

## Principes appliques

| Principe | Application |
|----------|-------------|
| **SOLID** | Injection de dependances, interfaces segregees |
| **Clean Architecture** | Separation stricte des couches |
| **DDD** | Entities riches, Value Objects |
| **Clean Code** | Nommage explicite, fonctions courtes |
