# Contributing

Guide de contribution au projet AdvancedDevSample.

---

### Branchement

Merci de contribuer ! Ce guide vous aide à respecter les standards du projet.

### Prérequis

- Avoir lu le [Getting Started](getting-started.md)
- Avoir forké le repository
- Avoir cloné votre fork localement

---

## Process de contribution

### 1. Créer une branche

```bash
git checkout -b feature/nom-de-la-feature
# OU
git checkout -b fix/nom-du-bug
```

**Convention de nommage :**

- `feature/` : Nouvelle fonctionnalité
- `fix/` : Correction de bug
- `docs/` : Documentation
- `refactor/` : Refactoring
- `test/` : Ajout de tests

---

### 2. Faire vos modifications

Suivez les [standards de code](#standards-de-code) decrits ci-dessous.

---

### 3. Écrire des tests

> [!IMPORTANT]
> Toute nouvelle fonctionnalité doit inclure des tests.

**Types de tests :**

### Stratégie de Tests

Nous utilisons deux outils complémentaires pour garantir la qualité du projet :

| Outil | Type | Objectif | Quand l'utiliser ? |
|-------|------|----------|--------------------|
| **xUnit** | Fonctionnel | Vérifier que le code *marche* (logique métier, règles) | Au développement de chaque feature |
| **k6** | Performance | Vérifier que le code *tient la charge* (vitesse, stabilité) | Avant une release ou sur CI/CD |

#### 1. Tests Fonctionnels (xUnit)

C'est la base de la pyramide de tests. Ils s'exécutent rapidement et valident la logique interne.

- **Unitaires (Domain)** : Tester les entités et value objects
- **Unitaires (Application)** : Tester les services
- **Unitaires (API)** : Tester les controllers et middlewares
- **Intégration** : Tester les endpoints end-to-end

#### 2. Tests de Charge (k6)

Simulez des utilisateurs réels pour détecter les goulots d'étranglement.

- **Load Testing** : Simuler 50 utilisateurs simultanés
- **Stress Testing** : Trouver le point de rupture
- **Spike Testing** : Tester les pics de trafic

---

### Exemples d'implémentation

#### xUnit (Logique)

```csharp
[Fact]
public void Login_WithValidCredentials_ReturnsToken()
{
    // Vérifie que si j'envoie "admin", je reçois un token
    var result = _controller.Login(request);
    Assert.NotNull(result.Token);
}
```

#### k6 (Performance)

```javascript
export default function() {
    // Vérifie que si 50 personnes se connectent, ça prend < 200ms
    let res = http.post('https://api/login', payload);
    check(res, { 'status is 200': (r) => r.status === 200 });
}
```

---

### Résumé pour les contributeurs

1. **J'ajoute une règle métier ?** → J'écris un test **xUnit**.
2. **J'optimise une requête lente ?** → Je vérifie avec **k6**.

**Exemple :**

```csharp
[Fact]
public void MethodName_Should_ExpectedBehavior_When_Condition()
{
    // Arrange
    var entity = new Product("Test", new Price(100m, "EUR"));

    // Act
    entity.ChangePrice(150m);

    // Assert
    Assert.Equal(150m, entity.Price.Value);
}
```

---

### 4. Vérifier que tout fonctionne

```bash
# Build
dotnet build

# Tests
dotnet test

# Vérifier la couverture (objectif: 80%+)
dotnet-coverage collect "dotnet test" -f xml -o coverage.xml
```

---

### 5. Commit avec Conventional Commits

**Format :**

```
<type>(<scope>): <description>

[corps optionnel]

[footer optionnel]
```

**Types :**

- `feat` : Nouvelle fonctionnalité
- `fix` : Correction de bug
- `docs` : Documentation
- `style` : Formatage (sans changement de logique)
- `refactor` : Refactoring
- `test` : Ajout de tests
- `chore` : Tâches de maintenance

**Exemples :**

```bash
git commit -m "feat(product): add discount calculation method"
git commit -m "fix(auth): correct JWT expiration validation"
git commit -m "docs(api): add examples for product endpoints"
git commit -m "test(product): add tests for ChangePrice edge cases"
```

---

### 6. Pousser et creer une Pull Request

```bash
git push origin feature/nom-de-la-feature
```

Puis sur GitHub :

1. Cliquer sur "Compare & pull request"
2. Remplir le template de PR
3. Attendre la review

---

## Standards de code

### Conventions C#

| Élément | Convention | Exemple |
|---------|-----------|---------|
| **Classe** | PascalCase | `ProductService` |
| **Méthode** | PascalCase | `GetById()` |
| **Variable locale** | camelCase | `productId` |
| **Propriété** | PascalCase | `Price` |
| **Constante** | PascalCase | `MaxRetries` |
| **Interface** | I + PascalCase | `IProductRepository` |

---

### Regles Clean Code

#### 1. Noms explicites

```csharp
// ❌ Mauvais
var p = GetP(id);

// ✅ Bon
var product = GetProductById(id);
```

#### 2. Fonctions courtes

```csharp
// ❌ Mauvais : fonction trop longue (>20 lignes)
public void ProcessProduct(Product p) { /* 50 lignes */ }

// ✅ Bon : decompose en petites fonctions
public void ProcessProduct(Product product)
{
    ValidateProduct(product);
    CalculatePrice(product);
    SaveProduct(product);
}
```

#### 3. Un seul niveau d'abstraction

```csharp
// ❌ Mauvais : melange de niveaux
public void CreateProduct(CreateProductRequest request)
{
    var product = new Product(request.Name, new Price(request.Price, request.Currency));
    _repository.Save(product); // Niveau bas
    SendEmailNotification(product); // Niveau haut
}

// ✅ Bon : meme niveau d'abstraction
public Guid CreateProduct(CreateProductRequest request)
{
    var product = BuildProduct(request);
    PersistProduct(product);
    NotifyProductCreation(product);
    return product.Id;
}
```

#### 4. Gestion des exceptions

```csharp
// ❌ Mauvais : exception generique
throw new Exception("Erreur");

// ✅ Bon : exception specifique au domaine
throw new DomainException("Le prix doit être positif.");
```

---

### Architecture

#### Separation des couches

```
API → Application → Domain ← Infrastructure
```

**Regles :**

- **Domain** ne depend de RIEN (entites, value objects, interfaces)
- **Application** depend du Domain (services, DTOs)
- **Infrastructure** depend du Domain (implementation repositories)
- **API** depend de Application et Infrastructure (controllers, middleware)

#### Injection de dependances

```csharp
// ✅ Bon : injecter l'interface, pas l'implementation
public ProductService(IProductRepository repository)
{
    _repository = repository;
}
```

---

## Checklist avant PR

- [ ] Le code build sans erreur
- [ ] Tous les tests passent (46+ tests)
- [ ] J'ai ajoute des tests pour ma feature
- [ ] La couverture reste > 80%
- [ ] J'ai respecte les conventions de nommage
- [ ] Mes commits suivent Conventional Commits
- [ ] J'ai update la documentation si necessaire
- [ ] Pas de secrets hardcodes

---

## Review process

### Ce que nous verifions

1. **Qualite du code**
   - Respect des conventions
   - Clean Code
   - Pas de code duplique

2. **Tests**
   - Tous les tests passent
   - Nouveaux tests pour nouvelle feature
   - Couverture maintenue

3. **Architecture**
   - Respect de Clean Architecture
   - Separation des responsabilites
   - SOLID

4. **SonarCloud**
   - Quality Gate passe
   - Pas de nouveaux bugs/vulnerabilites

---

## Ajouter une nouvelle feature

Exemple : ajouter une propriete `Description` a `Product`.

### 1. Modifier l'entite (Domain)

```csharp
public class Product
{
    public string Name { get; private set; }
    public string Description { get; private set; } // Nouveau

    public Product(string name, string description, Price price)
    {
        SetName(name);
        SetDescription(description);
        Price = price;
    }

    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("La description ne peut pas être vide.");
        Description = description;
    }
}
```

### 2. Ajouter les tests (Test/Domain)

```csharp
[Fact]
public void Product_Should_Throw_When_Description_Is_Empty()
{
    Assert.Throws<DomainException>(() =>
        new Product("Test", "", new Price(100m, "EUR"))
    );
}
```

### 3. Mettre a jour le DTO (Application)

```csharp
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } // Nouveau
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public bool IsActive { get; set; }
}
```

### 4. Mettre a jour le service (Application)

```csharp
public ProductDto GetProductDto(Guid id)
{
    var product = _repository.GetById(id);
    return new ProductDto
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description, // Nouveau
        Price = product.Price.Value,
        Currency = product.Price.Currency,
        IsActive = product.IsActive
    };
}
```

### 5. Mettre a jour le controller + Swagger

```csharp
/// <summary>
/// Crée un nouveau produit
/// </summary>
/// <param name="request">Description incluse</param>
[HttpPost]
[ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
public IActionResult CreateProduct([FromBody] CreateProductRequest request)
{
    // ...
}
```

### 6. Verifier

```bash
dotnet build
dotnet test
```

---

## Contact

Pour toute question :

- **GitHub Issues** : [Ouvrir une issue](https://github.com/HugoRytlewski/AdvancedDevSampleFinal/issues)
- **Discussions** : [Forum](https://github.com/HugoRytlewski/AdvancedDevSampleFinal/discussions)
