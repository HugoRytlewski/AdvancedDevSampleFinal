# Architecture

Vue d'ensemble de l'architecture du projet AdvancedDevSample.

## Architecture en couches

Le projet suit une **architecture hexagonale** (Clean Architecture) avec separation claire des responsabilites.

```mermaid
graph TB
    subgraph Presentation["Couche Presentation"]
        API[API Controllers]
        Swagger[Swagger UI]
        Middleware[Middlewares]
    end
    
    subgraph Application["Couche Application"]
        Services[Services Applicatifs]
        DTOs[DTOs]
    end
    
    subgraph Domain["Couche Domaine"]
        Entities[Entites]
        ValueObjects[Value Objects]
        Interfaces[Interfaces Repository]
        DomainExceptions[Exceptions Domaine]
    end
    
    subgraph Infrastructure["Couche Infrastructure"]
        Repositories[Repositories]
        Database[(Base de donnees)]
    end
    
    API --> Services
    Services --> Entities
    Services --> Interfaces
    Repositories --> Interfaces
    Repositories --> Database
```

## Structure des fichiers

```
AdvancedDevSample/
|-- Api/              # Controllers, Swagger, JWT
|-- Application/      # DTOs, Services
|-- Domain/           # Entities, Value Objects
|-- Infrastructure/   # Repositories, EF Core
|-- Test/             # Unit & Integration Tests
```

---

## Flux d'une requete

```mermaid
sequenceDiagram
    participant Client
    participant Middleware
    participant Controller
    participant Service
    participant Repository
    participant Database
    
    Client->>Middleware: HTTP Request
    Middleware->>Middleware: JWT Validation
    Middleware->>Controller: Request validee
    Controller->>Service: Appel service
    Service->>Repository: Acces donnees
    Repository->>Database: Query
    Database-->>Repository: Resultat
    Repository-->>Service: Entite
    Service-->>Controller: DTO
    Controller-->>Client: HTTP Response
```

---

## Cycle de vie d'un produit

```mermaid
stateDiagram-v2
    [*] --> Created: POST /api/products
    Created --> Active: Etat initial
    Active --> PriceChanged: PUT /api/products/{id}/price
    PriceChanged --> Active: Nouveau prix applique
    Active --> Inactive: DELETE /api/products/{id}
    Inactive --> [*]: Soft Delete
```

---

## Diagramme de classes (Domain)

```mermaid
classDiagram
    class Product {
        +Guid Id
        +string Name
        +Price Price
        +bool IsActive
        +ChangePrice(Price newPrice)
        +ChangePrice(decimal newPriceValue)
        +Deactivate()
        +Activate()
    }
    
    class Price {
        +decimal Value
        +string Currency
        +Price(decimal value, string currency)
    }
    
    class IProductRepository {
        <<interface>>
        +Save(Product product)
        +GetById(Guid id) Product?
        +GetAll() IEnumerable~Product~
        +Delete(Guid id)
    }
    
    class DomainException {
        +string Message
    }
    
    Product --> Price : contient
    Product ..> DomainException : lance
```

---

## Pipeline CI/CD

```mermaid
flowchart LR
    subgraph Trigger["Declencheur"]
        Push[Push main]
        PR[Pull Request]
    end
    
    subgraph Build["Build & Test"]
        Restore[Restore]
        Compile[Build]
        Test[Tests]
    end
    
    subgraph Analysis["Analyse"]
        Sonar[SonarCloud]
        Coverage[Couverture]
    end
    
    subgraph Deploy["Deploiement"]
        MkDocs[Build MkDocs]
        Pages[GitHub Pages]
    end
    
    Push --> Restore
    PR --> Restore
    Restore --> Compile
    Compile --> Test
    Test --> Sonar
    Sonar --> Coverage
    Coverage --> MkDocs
    MkDocs --> Pages
```

---

## Principes SOLID

| Principe | Description | Application |
|----------|-------------|-------------|
| **S** - Single Responsibility | Une classe = une responsabilite | `ProductService` gere uniquement les produits |
| **O** - Open/Closed | Ouvert a l'extension, ferme a la modification | Interfaces pour les repositories |
| **L** - Liskov Substitution | Les sous-types respectent les contrats | `EfProductRepository` implemente `IProductRepository` |
| **I** - Interface Segregation | Interfaces specifiques et focalisees | `IProductRepository` vs interface generique |
| **D** - Dependency Inversion | Dependre des abstractions | Injection de `IProductRepository` |

---

## Regles Clean Code

| Regle | Description |
|-------|-------------|
| Noms explicites | `GetProductById` vs `Get` |
| Fonctions courtes | Maximum 20 lignes par methode |
| Un niveau d'abstraction | Chaque fonction opere a un seul niveau |
| Pas de commentaires inutiles | Le code s'auto-documente |
| Gestion des erreurs | Exceptions specifiques au domaine |

---

## Metriques de qualite (SonarCloud)

| Metrique | Objectif |
|----------|----------|
| Couverture de code | > 80% |
| Code Smells | 0 bloquants |
| Bugs | 0 |
| Vulnerabilites | 0 |
| Dette technique | < 1 jour |
| Duplications | < 3% |
