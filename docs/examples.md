# Examples

Scénarios pratiques et exemples complets pour utiliser l'API.

---

## Scénario 1 : CRUD complet d'un produit

### Étape 1 : Authentification

**Requête :**

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "password"
  }'
```

**Réponse (200 OK) :**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbiIsImp0aSI6IjNmYTg1ZjY0LTU3MTctNDU2Mi1iM2ZjLTJjOTYzZjY2YWZhNiIsImV4cCI6MTczOTEwMjQwMCwiaXNzIjoiQWR2YW5jZWREZXZTYW1wbGUiLCJhdWQiOiJBZHZhbmNlZERldlNhbXBsZVVzZXJzIn0.ABC123..."
}
```

> [!TIP]
> Copiez le token pour les prochaines requetes.

---

### Étape 2 : Créer un produit

**Requête :**

```bash
curl -X POST https://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer VOTRE_TOKEN" \
  -d '{
    "name": "iPhone 15 Pro",
    "price": 999.99,
    "currency": "EUR"
  }'
```

**Réponse (201 Created) :**

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "iPhone 15 Pro",
  "price": 999.99,
  "currency": "EUR",
  "isActive": true
}
```

---

### Étape 3 : Consulter tous les produits

**Requête :**

```bash
curl -X GET https://localhost:5001/api/products \
  -H "Authorization: Bearer VOTRE_TOKEN"
```

**Réponse (200 OK) :**

```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "iPhone 15 Pro",
    "price": 999.99,
    "currency": "EUR",
    "isActive": true
  }
]
```

---

### Étape 4 : Modifier le prix

**Requête :**

```bash
curl -X PUT https://localhost:5001/api/products/3fa85f64-5717-4562-b3fc-2c963f66afa6/price \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer VOTRE_TOKEN" \
  -d '{
    "newPrice": 899.99
  }'
```

**Réponse (204 No Content)**

---

### Étape 5 : Vérifier la modification

**Requête :**

```bash
curl -X GET https://localhost:5001/api/products/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer VOTRE_TOKEN"
```

**Réponse (200 OK) :**

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "iPhone 15 Pro",
  "price": 899.99,
  "currency": "EUR",
  "isActive": true
}
```

---

### Étape 6 : Supprimer le produit

**Requête :**

```bash
curl -X DELETE https://localhost:5001/api/products/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer VOTRE_TOKEN"
```

**Réponse (204 No Content)**

Le produit est maintenant désactivé (`isActive: false`).

---

## Scénario 2 : Gestion des erreurs

### Erreur 401 : Token invalide

**Requête :**

```bash
curl -X GET https://localhost:5001/api/products \
  -H "Authorization: Bearer TOKEN_INVALIDE"
```

**Réponse (401 Unauthorized) :**

```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```

**Solution :** Régénérez un token via `/api/auth/login`.

---

### Erreur 400 : Prix negatif

**Requête :**

```bash
curl -X POST https://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer VOTRE_TOKEN" \
  -d '{
    "name": "Produit invalide",
    "price": -50.00,
    "currency": "EUR"
  }'
```

**Réponse (400 Bad Request) :**

```json
{
  "title": "Erreur métier",
  "detail": "Le prix doit être positif."
}
```

---

### Erreur 404 : Produit introuvable

**Requête :**

```bash
curl -X GET https://localhost:5001/api/products/00000000-0000-0000-0000-000000000000 \
  -H "Authorization: Bearer VOTRE_TOKEN"
```

**Réponse (404 Not Found) :**

```json
{
  "title": "Ressource introuvable",
  "detail": "Produit introuvable."
}
```

---

## Collection Postman

Importez cette collection pour tester facilement :

```json
{
  "info": {
    "name": "AdvancedDevSample API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
      "name": "Auth",
      "item": [
        {
          "name": "Login",
          "request": {
            "method": "POST",
            "header": [
              {
                "key": "Content-Type",
                "value": "application/json"
              }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"username\": \"admin\",\n  \"password\": \"password\"\n}"
            },
            "url": {
              "raw": "https://localhost:5001/api/auth/login",
              "protocol": "https",
              "host": ["localhost"],
              "port": "5001",
              "path": ["api", "auth", "login"]
            }
          }
        }
      ]
    },
    {
      "name": "Products",
      "item": [
        {
          "name": "Get All Products",
          "request": {
            "method": "GET",
            "header": [
              {
                "key": "Authorization",
                "value": "Bearer {{token}}"
              }
            ],
            "url": {
              "raw": "https://localhost:5001/api/products",
              "protocol": "https",
              "host": ["localhost"],
              "port": "5001",
              "path": ["api", "products"]
            }
          }
        },
        {
          "name": "Create Product",
          "request": {
            "method": "POST",
            "header": [
              {
                "key": "Content-Type",
                "value": "application/json"
              },
              {
                "key": "Authorization",
                "value": "Bearer {{token}}"
              }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"name\": \"iPhone 15 Pro\",\n  \"price\": 999.99,\n  \"currency\": \"EUR\"\n}"
            },
            "url": {
              "raw": "https://localhost:5001/api/products",
              "protocol": "https",
              "host": ["localhost"],
              "port": "5001",
              "path": ["api", "products"]
            }
          }
        }
      ]
    }
  ]
}
```

**Pour importer :**
1. Ouvrir Postman
2. Cliquer sur "Import"
3. Coller le JSON ci-dessus
4. Créer une variable `token` dans l'environnement

---

## Variables Postman

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "baseUrl": "https://localhost:5001",
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

---

## PowerShell (Windows)

```powershell
# Login
$response = Invoke-RestMethod -Method POST -Uri "https://localhost:5001/api/auth/login" `
  -ContentType "application/json" `
  -Body '{"username":"admin","password":"password"}'

$token = $response.token

# Get products
Invoke-RestMethod -Method GET -Uri "https://localhost:5001/api/products" `
  -Headers @{"Authorization" = "Bearer $token"}
```
