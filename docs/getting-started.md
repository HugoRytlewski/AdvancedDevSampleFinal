# Getting Started

Guide complet pour installer et demarrer le projet AdvancedDevSample.

---

## Prerequis

Avant de commencer, assurez-vous d'avoir installe :

| Logiciel | Version minimale | Lien |
|----------|-----------------|------|
| **.NET SDK** | 10.0 | [Telecharger](https://dotnet.microsoft.com/download) |
| **Git** | 2.0+ | [Telecharger](https://git-scm.com/) |
| **IDE** | Visual Studio 2024 / VS Code / Rider | Au choix |

**Verification :**

```bash
dotnet --version  # Doit afficher 10.0.x
git --version     # Doit afficher 2.x ou superieur
```

---

## Installation

### 1. Cloner le repository

```bash
git clone https://github.com/HugoRytlewski/AdvancedDevSampleFinal.git
cd AdvancedDevSampleFinal
```

### 2. Restaurer les dependances

```bash
dotnet restore
```

### 3. Configurer JWT

Creer ou modifier `appsettings.json` dans `AdvancedDevSample.Api/` :

```json
{
  "Jwt": {
    "Key": "VotreCleSecrete-MinimumVingtCaracteresOuPlus-PourHMACSHA256",
    "Issuer": "AdvancedDevSample",
    "Audience": "AdvancedDevSampleUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

> [!IMPORTANT]
> La cle JWT doit faire au moins **32 caracteres** pour HMAC SHA256.

---

## Premier lancement

### 1. Build le projet

```bash
dotnet build
```

**Sortie attendue :**

```
La generation a reussi.
    0 Erreur(s)
```

### 2. Lancer les tests

```bash
dotnet test
```

**Sortie attendue :**

```
Reussi!  - echec : 0, reussite : 46, total : 46
```

### 3. Lancer l'API

```bash
cd AdvancedDevSample.Api
dotnet run
```

**Sortie attendue :**

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

---

## Premier test avec Swagger

### 1. Acceder a Swagger UI

Ouvrez votre navigateur : **[https://localhost:5001/swagger](https://localhost:5001/swagger)**

### 2. Obtenir un token JWT

1. Cliquez sur **POST /api/auth/login**
2. Cliquez sur "Try it out"
3. Entrez les credentials :

```json
{
  "username": "admin",
  "password": "password"
}
```

4. Cliquez sur "Execute"
5. **Copiez le token** dans la reponse

### 3. Configurer l'authentification

1. Cliquez sur le bouton **Authorize** (cadenas en haut)
2. Entrez : `Bearer VOTRE_TOKEN` (coller le token)
3. Cliquez sur "Authorize"

### 4. Tester un endpoint protege

1. Cliquez sur **GET /api/products**
2. Cliquez sur "Try it out" → "Execute"
3. Vous devriez recevoir une reponse **200 OK**

---

## Structure du projet

```
AdvancedDevSampleFinal/
│
├── AdvancedDevSample.Api/          # API REST
│   ├── Controllers/                # Endpoints
│   ├── Middlewares/                # Exception handling
│   └── Program.cs                  # Configuration
│
├── AdvancedDevSample.Application/  # Services applicatifs
│   ├── DTOs/                       # Data Transfer Objects
│   └── Services/                   # Logique applicative
│
├── AdvancedDevSampleDomain/        # Coeur metier
│   ├── Entities/                   # Product
│   ├── ValueObjects/               # Price
│   ├── Interfaces/                 # IProductRepository
│   └── Exceptions/                 # Exceptions metier
│
├── AdvancedDevSample.Infrastructure/
│   └── Repositories/               # Implementation repositories
│
├── AdvancedDevSample.Test/         # Tests (46 tests)
│   ├── API/                        # Tests d'integration
│   ├── Application/                # Tests services
│   └── Domain/                     # Tests entites
│
├── docs/                           # Documentation MkDocs
└── .github/workflows/              # CI/CD GitHub Actions
```

---

## Troubleshooting

### Erreur : "JWT Key is not configured"

**Solution :** Verifiez que `appsettings.json` contient la cle JWT (voir etape 3 de l'installation).

### Erreur : "Unable to bind to https://localhost:5001"

**Solution :** Le port est deja utilise.

```bash
# Modifier le port dans launchSettings.json
# OU arreter le processus qui utilise le port 5001
lsof -i :5001
kill -9 <PID>
```

### Erreur : "Unauthorized" sur tous les endpoints

**Solution :** Verifiez que vous avez bien clique sur "Authorize" dans Swagger et entre le token avec le prefixe `Bearer `.

### Les tests echouent

**Solution :**

```bash
# Nettoyer et rebuilder
dotnet clean
dotnet build
dotnet test
```

---

## Prochaines etapes

<div class="grid" markdown>

<div class="card" markdown>

### Explorer l'API

[Consulter la reference API](api.md){ .md-button }

</div>

<div class="card" markdown>

### Contribuer

[Guide de contribution](contributing.md){ .md-button }

</div>

<div class="card" markdown>

### Reference rapide

[Commandes essentielles](quick-reference.md){ .md-button }

</div>

</div>
