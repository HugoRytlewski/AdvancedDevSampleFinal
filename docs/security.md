# Security

Guide de securite du projet AdvancedDevSample.

---

## OWASP Top 10 - Mitigations

| Risque OWASP | Mitigation appliquee | Implementation |
|--------------|---------------------|----------------|
| **A01: Broken Access Control** | JWT obligatoire pour endpoints sensibles | `[Authorize]` sur ProductsController |
| **A02: Cryptographic Failures** | Cle JWT securisee (256 bits) | `appsettings.json` + variable d'environnement |
| **A03: Injection** | Value Objects + validation | `Price` valide les entrees |
| **A04: Insecure Design** | Clean Architecture + DDD | Separation stricte des couches |
| **A05: Security Misconfiguration** | Configuration explicite | `Program.cs` configure JWT |
| **A07: Authentication Failures** | JWT avec expiration (30 min) | `GenerateJwtToken()` |
| **A08: Software Data Integrity** | Tests (46, couverture 80%) | CI/CD avec SonarCloud |
| **A09: Logging Failures** | Middleware d'exception | `ExceptionHandlingMiddleware` |
| **A10: SSRF** | Pas d'appels externes | Architecture in-memory |

---

## JWT (JSON Web Token)

### Generation

**Algorithme :** HMAC SHA256

**Claims inclus :**

- `sub` : Subject (username)
- `jti` : JWT ID (unique)
- `exp` : Expiration (30 minutes)
- `iss` : Issuer ("AdvancedDevSample")
- `aud` : Audience ("AdvancedDevSampleUsers")

**Code :**

```csharp
private string GenerateJwtToken(string username)
{
    var jwtKey = _configuration["Jwt:Key"] 
        ?? throw new InvalidOperationException("JWT Key is not configured");
    
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        },
        expires: DateTime.UtcNow.AddMinutes(30),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

---

### Validation

**Parametres valides :**

```csharp
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
};
```

**Verifications :**

- ✅ Signature valide (HMAC SHA256)
- ✅ Issuer correct
- ✅ Audience correcte
- ✅ Token non expire

---

### Bonnes pratiques JWT

| Pratique | Statut | Details |
|----------|--------|---------|
| **Cle minimale 256 bits** | ✅ | 32 caracteres minimum |
| **Expiration courte** | ✅ | 30 minutes |
| **HTTPS obligatoire** | ✅ | Production uniquement |
| **Rotation des cles** | ⚠️  | A implementer pour production |
| **Refresh tokens** | ❌ | Simple auth, pas de refresh |

---

## Gestion des secrets

### ❌ NE JAMAIS faire

```csharp
// MAUVAIS : cle hardcodee
var key = "my-secret-key-1234";
```

### ✅ Bonne pratique

**appsettings.json** (developpement) :

```json
{
  "Jwt": {
    "Key": "VotreCleSecrete32CaracteresMinimum"
  }
}
```

**Variables d'environnement** (production) :

```bash
export JWT__KEY="ProductionSecureKey256BitsMinimum"
```

**GitHub Secrets** (CI/CD) :

```yaml
env:
  SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
```

---

## Validation des entrees

### Value Objects

Le pattern **Value Object** assure la validation :

```csharp
public class Price
{
    public decimal Value { get; }
    public string Currency { get; }

    public Price(decimal value, string currency = "EUR")
    {
        if (value <= 0)
            throw new DomainException("Le prix doit être positif.");
        
        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("La devise est obligatoire.");

        Value = value;
        Currency = currency;
    }
}
```

**Avantages :**

- Validation centralisee
- Impossible de creer un `Price` invalide
- Type-safety

---

## Exception Handling

### Middleware personnalise

```csharp
public class ExceptionHandlingMiddleware
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new 
            { 
                title = "Erreur métier", 
                detail = ex.Message 
            });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new 
            { 
                title = "Erreur serveur", 
                detail = "Une erreur interne." 
            });
        }
    }
}
```

**Avantages :**

- Pas de leak d'informations sensibles
- Reponses JSON structurees
- Logs centralises

---

## Rate Limiting

> [!NOTE]
> Non implemente actuellement - recommandation pour production.

**Exemple d'implementation :**

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opts =>
    {
        opts.PermitLimit = 100;
        opts.Window = TimeSpan.FromMinutes(1);
    });
});
```

---

## Audit de securite

### SonarCloud

**Issues corrigees :**

| Issue | Severite | Fichier | Correction |
|-------|----------|---------|------------|
| Hardcoded secret | Blocker | `AuthController.cs` | Configuration externalisee |
| Hardcoded secret | Blocker | `Program.cs` | Configuration externalisee |
| Workflow permissions | Major | `ci.yml` | Permissions au niveau job |

**Quality Gate :**

- ✅ 0 vulnerabilites
- ✅ 0 bugs critiques
- ✅ Couverture > 80%

---

## Checklist securite

### Developpement

- [ ] Pas de secrets hardcodes
- [ ] Validation de toutes les entrees
- [ ] Exceptions bien gerees
- [ ] JWT avec expiration
- [ ] HTTPS en production

### CI/CD

- [ ] SonarCloud actif
- [ ] Secrets dans GitHub Secrets
- [ ] Tests de securite automatiques
- [ ] Quality Gate obligatoire

### Production

- [ ] Variables d'environnement pour secrets
- [ ] HTTPS obligatoire
- [ ] Rate limiting actif
- [ ] Logs centralises
- [ ] Rotation des cles JWT

---

## Reporting de vulnerabilites

Si vous trouvez une vulnerabilite :

1. **NE PAS** creer une issue publique
2. Envoyer un email a : [security@example.com](mailto:security@example.com)
3. Inclure :
   - Description de la vulnerabilite
   - Steps to reproduce
   - Impact potentiel
   - Suggestions de correction

**Delai de reponse :** 48h
