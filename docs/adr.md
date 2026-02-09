# Architecture Decision Records (ADR)

Décisions techniques majeures du projet AdvancedDevSample.

---

## ADR-001 : Clean Architecture

**Date :** 2026-01-15  
**Statut :** Accepté  
**Décideurs :** Équipe technique

### Contexte

Nous devions choisir un pattern architectural pour structurer le projet.

**Options considérées :**

1. Architecture en couches traditionnelle (3-tier)
2. Clean Architecture (Hexagonale)
3. Microservices

### Décision

Nous avons choisi **Clean Architecture** (Hexagonale).

### Justification

**Avantages :**

- **Testabilité** : Domaine indépendant, facile à tester
- **Maintenabilité** : Séparation claire des responsabilités
- **Flexibilité** : Facile de changer infrastructure (DB, framework)
- **DDD** : S'aligne naturellement avec Domain-Driven Design

**Inconvénients :**

- Complexité initiale (plus de fichiers)
- Courbe d'apprentissage

**Pourquoi pas 3-tier ?**

- Couplage fort entre couches
- Logique métier souvent dans la couche service

**Pourquoi pas microservices ?**

- Over-engineering pour un projet de cette taille
- Complexite operationnelle

### Consequences

- **Positives** : Tests isoles, domaine riche
- **Negatives** : Plus de fichiers/dossiers a maintenir

---

## ADR-002 : JWT vs OAuth2

**Date :** 2026-01-20  
**Statut :** Accepte  
**Decideurs :** Equipe technique

### Contexte

Nous devions choisir un mecanisme d'authentification pour l'API.

**Options considerees :**

1. JWT (JSON Web Token)
2. OAuth2
3. Session cookies

### Decision

Nous avons choisi **JWT**.

### Justification

**Avantages JWT :**

- **Stateless** : Pas de stockage serveur
- **Simple** : Implementation directe avec .NET
- **Adapte aux API** : Standard pour REST APIs
- **Performance** : Pas de round-trip DB pour valider

**Pourquoi pas OAuth2 ?**

- Over-engineering pour une API simple
- Necessite un authorization server
- Complexite non justifiee

**Pourquoi pas sessions ?**

- Stateful (necessite Redis/DB)
- Moins adapte aux API REST

### Consequences

- **Positives** : Simple, performant
- **Negatives** : Pas de revocation immediate (attendre expiration)

### Notes

Pour une vraie production :

- Ajouter refresh tokens
- Implementer revocation (blacklist)

---

## ADR-003 : In-Memory Repository

**Date :** 2026-01-22  
**Statut :** Accepte (dev only)  
**Decideurs :** Equipe technique

### Contexte

Nous devions choisir la strategie de persistance pour le developpement.

**Options considerees :**

1. In-memory (List<T>)
2. Entity Framework Core + SQLite
3. Entity Framework Core + SQL Server

### Decision

Nous avons choisi **In-Memory Repository** pour le developpement.

### Justification

**Avantages :**

- **Simplicite** : Pas de setup DB
- **Tests rapides** : Pas de I/O
- **Portabilite** : Fonctionne partout
- **Clean Architecture** : Interchangeable grace a `IProductRepository`

**Inconvenients :**

- Donnees perdues au restart
- Pas realiste pour production

### Consequences

- **Positives** : Dev rapide, tests isoles
- **Negatives** : Necessite EF Core pour production

### Migration vers production

```csharp
// Remplacer InMemoryProductRepository par EfProductRepository
services.AddScoped<IProductRepository, EfProductRepository>();
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
```

---

## ADR-004 : Value Objects pour Price

**Date :** 2026-01-25  
**Statut :** Accepte  
**Decideurs :** Equipe technique

### Contexte

Nous devions modeliser le prix d'un produit.

**Options considerees :**

1. `decimal Price` (propriete simple)
2. `Price` Value Object
3. `Money` library externe (NodaMoney)

### Decision

Nous avons choisi **Value Object `Price`**.

### Justification

**Avantages Value Object :**

- **Validation centralisee** : Prix toujours > 0
- **Immuabilite** : Pas de modification accidentelle
- **Type safety** : `Price` != `decimal`
- **Logique metier** : Peut contenir devise

**Pourquoi pas propriete simple ?**

- Validation dispersee
- Pas de garantie d'integrite

**Pourquoi pas library externe ?**

- Dependance externe
- Over-engineering pour besoin simple

### Implementation

```csharp
public class Price
{
    public decimal Value { get; }
    public string Currency { get; }

    public Price(decimal value, string currency = "EUR")
    {
        if (value <= 0)
            throw new DomainException("Le prix doit être positif.");
        
        Value = value;
        Currency = currency;
    }
}
```

### Consequences

- **Positives** : Validation automatique, code robuste
- **Negatives** : Plus de code qu'un simple `decimal`

---

## ADR-005 : Soft Delete

**Date :** 2026-01-28  
**Statut :** Accepte  
**Decideurs :** Equipe technique

### Contexte

Nous devions implementer la suppression de produits.

**Options considerees :**

1. Hard delete (suppression physique)
2. Soft delete (`IsActive` flag)
3. Archive table (table separee)

### Decision

Nous avons choisi **Soft Delete** avec `IsActive`.

### Justification

**Avantages :**

- **Recuperation** : Donnees pas perdues
- **Audit** : Historique preserve
- **Simple** : Juste un flag boolean

**Pourquoi pas hard delete ?**

- Perte de donnees irreversible
- Pas d'historique

**Pourquoi pas archive table ?**

- Complexite supplementaire
- Queries plus complexes

### Implementation

```csharp
public void Deactivate()
{
    IsActive = false;
}

public void Activate()
{
    IsActive = true;
}
```

### Consequences

- **Positives** : Donnees preservees, recuperables
- **Negatives** : Queries doivent filtrer `WHERE IsActive = true`

---

## Template ADR

Pour ajouter une nouvelle decision :

```markdown
## ADR-XXX : Titre de la decision

**Date :** YYYY-MM-DD  
**Statut :** Propose / Accepte / Rejete / Obsolete  
**Decideurs :** Noms

### Contexte

Quel probleme cherchons-nous a resoudre ?

### Decision

Quelle solution avons-nous choisie ?

### Justification

Pourquoi cette solution ? Quelles alternatives ?

### Consequences

Quels impacts (positifs et negatifs) ?
```

---

## Statuts possibles

| Statut | Signification |
|--------|---------------|
| **Propose** | En discussion |
| **Accepte** | Approuve et applique |
| **Rejete** | Refuse apres discussion |
| **Obsolete** | Remplace par une nouvelle ADR |
