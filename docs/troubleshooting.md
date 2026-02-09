# Troubleshooting

Solutions aux problemes courants du projet AdvancedDevSample.

---

## Build & Compilation

### Erreur : "The type or namespace name 'X' could not be found"

**Cause :** Dependances manquantes ou non restaurees.

**Solution :**

```bash
dotnet restore
dotnet clean
dotnet build
```

---

### Erreur : "NU1603 Warning"

**Message complet :**

```
warning NU1603: AdvancedDevSample.Api dépend de Microsoft.AspNetCore.Authentication.JwtBearer (>= 10.0.0-preview.1.25081.5) mais Microsoft.AspNetCore.Authentication.JwtBearer 10.0.0-preview.1.25081.5 est introuvable
```

**Cause :** Version preview de package non trouvee, une version superieure est utilisee.

**Impact :** Aucun (warning seulement, pas d'erreur).

**Solution (optionnelle) :**

```bash
# Mettre a jour vers la derniere version stable
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

---

## Authentification JWT

### "JWT Key is not configured"

**Cause :** Fichier `appsettings.json` manquant ou incomplet.

**Solution :**

Creer/verifier `AdvancedDevSample.Api/appsettings.json` :

```json
{
  "Jwt": {
    "Key": "VotreCleSecrete32CaracteresMinimumPourHMAC",
    "Issuer": "AdvancedDevSample",
    "Audience": "AdvancedDevSampleUsers"
  }
}
```

---

### "Unauthorized" sur tous les endpoints

**Cause 1 :** Token manquant

**Solution :** Ajouter le header `Authorization: Bearer VOTRE_TOKEN`

**Cause 2 :** Token expire (apres 30 min)

**Solution :** Regenerer un token via `POST /api/auth/login`

**Cause 3 :** Token malformed

**Solution :** Verifier le format : `Bearer ` + token (avec espace)

---

### "Unable to verify signature"

**Cause :** Cle JWT differente entre generation et validation.

**Solution :**

1. Verifier que `appsettings.json` contient la meme cle
2. Redemarrer l'API

---

## API & Swagger

### "Unable to bind to https://localhost:5001"

**Cause :** Port deja utilise.

**Solution 1 :** Tuer le processus

```bash
# macOS/Linux
lsof -i :5001
kill -9 <PID>

# Windows
netstat -ano | findstr :5001
taskkill /PID <PID> /F
```

**Solution 2 :** Changer le port

Modifier `AdvancedDevSample.Api/Properties/launchSettings.json` :

```json
{
  "profiles": {
    "https": {
      "applicationUrl": "https://localhost:5002"  // Changer ici
    }
  }
}
```

---

### Swagger affiche "Failed to load API definition"

**Cause :** Erreur dans XML comments Swagger.

**Solution :**

1. Verifier les comments XML dans controllers
2. Rebuilder le projet

```bash
dotnet clean
dotnet build
```

---

## Tests

### Tous les tests echouent

**Solution 1 :** Nettoyer et rebuilder

```bash
dotnet clean
dotnet build
dotnet test
```

**Solution 2 :** Supprimer bin/ et obj/

```bash
find . -name "bin" -o -name "obj" | xargs rm -rf
dotnet build
dotnet test
```

---

### "Assert.Equal() Failure: Strings differ"

**Exemple :**

```
Expected: "application/json"
Actual:   "application/json; charset=utf-8"
```

**Cause :** Comparaison stricte de Content-Type.

**Solution :** Utiliser `Assert.StartsWith` au lieu de `Assert.Equal`

```csharp
// ❌ Mauvais
Assert.Equal("application/json", response.ContentType);

// ✅ Bon
Assert.StartsWith("application/json", response.ContentType);
```

---

## Database & Repositories

### "Object reference not set to an instance of an object"

**Cause :** Repository non injecte ou mal configure.

**Solution :** Verifier `Program.cs`

```csharp
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
```

---

## CI/CD & GitHub Actions

### Workflow "build-and-test" echoue

**Solution 1 :** Verifier les logs GitHub Actions

1. Aller sur l'onglet "Actions"
2. Cliquer sur le workflow echoue
3. Lire les logs d'erreur

**Solution 2 :** Tester localement

```bash
dotnet build --configuration Release
dotnet test
```

---

### SonarCloud Quality Gate echoue

**Causes possibles :**

- Coverage < 80%
- Nouveaux bugs/vulnerabilites
- Code smells bloquants

**Solution :**

1. Consulter [SonarCloud Dashboard](https://sonarcloud.io/project/overview?id=HugoRytlewski_AdvancedDevSampleFinal)
2. Corriger les issues identifiees
3. Relancer le pipeline

---

## Performance

### L'API est lente

**Diagnostic :**

```bash
# Mesurer le temps de reponse
curl -w "@curl-format.txt" -o /dev/null -s https://localhost:5001/api/products
```

**Creer `curl-format.txt` :**

```
time_total: %{time_total}s
```

**Solutions :**

1. Verifier les logs pour exceptions
2. Profiler avec dotTrace/dotMemory
3. Ajouter du caching (si necessaire)

---

## MkDocs Documentation

### "mkdocs serve" echoue

**Cause :** Python/mkdocs non installe.

**Solution :**

```bash
# Installer Python 3
brew install python3  # macOS
sudo apt install python3  # Linux

# Installer mkdocs
pip install mkdocs mkdocs-material
```

---

### Badges SonarCloud affichent "Project not found"

**Cause :** Projet pas encore analyse par SonarCloud.

**Solution :** Attendre le premier run du CI/CD sur GitHub.

---

## Erreurs courantes dans le code

### "DomainException: Le prix doit être positif"

**Cause :** Tentative de creer un produit avec prix <= 0.

**Solution :**

```json
{
  "name": "Produit",
  "price": 99.99,  // Doit être > 0
  "currency": "EUR"
}
```

---

### "ApplicationServiceException: Produit introuvable"

**Cause :** ID inexistant dans le repository.

**Solution :** Verifier que l'ID existe via `GET /api/products`

---

## FAQ

### Comment regenerer un token JWT ?

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'
```

### Comment voir tous les produits ?

```bash
curl https://localhost:5001/api/products \
  -H "Authorization: Bearer VOTRE_TOKEN"
```

### Comment lancer les tests avec couverture ?

```bash
dotnet-coverage collect "dotnet test" -f xml -o coverage.xml
```

### Comment consulter le rapport SonarCloud ?

[https://sonarcloud.io/project/overview?id=HugoRytlewski_AdvancedDevSampleFinal](https://sonarcloud.io/project/overview?id=HugoRytlewski_AdvancedDevSampleFinal)

---

## Toujours bloque ?

Si aucune solution ne fonctionne :

1. **Consulter les logs** : `dotnet run --verbosity detailed`
2. **Issues GitHub** : [Ouvrir une issue](https://github.com/HugoRytlewski/AdvancedDevSampleFinal/issues)
3. **Documentation** : [Index](index.md) | [Getting Started](getting-started.md)
