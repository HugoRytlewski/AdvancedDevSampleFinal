# API Reference

Documentation complète des endpoints de l'API AdvancedDevSample.

## 🔐 Authentification

### POST `/api/auth/login`

Authentifie un utilisateur et retourne un token JWT.

**Request Body:**

```json
{
  "username": "admin",
  "password": "password"
}
```

**Responses:**

| Code | Description |
|------|-------------|
| 200 | Authentification réussie, retourne le token |
| 401 | Identifiants incorrects |

**Exemple de réponse (200):**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

## 📦 Produits

!!! note "Authentification requise"
    Tous les endpoints `/api/products` nécessitent un token JWT valide.

### GET `/api/products`

Récupère la liste de tous les produits.

**Responses:**

| Code | Description |
|------|-------------|
| 200 | Liste des produits |
| 401 | Non autorisé |

**Exemple de réponse:**

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

### GET `/api/products/{id}`

Récupère un produit par son identifiant.

**Path Parameters:**

| Paramètre | Type | Description |
|-----------|------|-------------|
| `id` | UUID | Identifiant unique du produit |

**Responses:**

| Code | Description |
|------|-------------|
| 200 | Détails du produit |
| 404 | Produit non trouvé |
| 401 | Non autorisé |

---

### POST `/api/products`

Crée un nouveau produit.

**Request Body:**

```json
{
  "name": "MacBook Pro",
  "price": 2499.99,
  "currency": "EUR"
}
```

**Responses:**

| Code | Description |
|------|-------------|
| 201 | Produit créé avec succès |
| 400 | Données invalides |
| 401 | Non autorisé |

---

### PUT `/api/products/{id}/price`

Modifie le prix d'un produit existant.

**Request Body:**

```json
{
  "newPrice": 149.99
}
```

**Responses:**

| Code | Description |
|------|-------------|
| 204 | Prix modifié avec succès |
| 400 | Prix invalide |
| 404 | Produit non trouvé |
| 401 | Non autorisé |

---

### DELETE `/api/products/{id}`

Supprime un produit.

**Responses:**

| Code | Description |
|------|-------------|
| 204 | Produit supprimé |
| 401 | Non autorisé |

---

## 📄 Swagger UI

Pour une documentation interactive, accédez à Swagger UI une fois l'API lancée :

```
https://localhost:5001/swagger
```
