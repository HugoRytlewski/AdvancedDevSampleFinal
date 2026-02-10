# API Reference

Documentation complete des endpoints de l'API AdvancedDevSample.

## Vue d'ensemble

```mermaid
graph LR
    subgraph Auth
        Login
    end
    
    subgraph Products
        P_GetAll[GetAll]
        P_GetById[GetById]
        P_Create[Create]
    end

    subgraph Clients
        C_Create[Create]
        C_GetAll[GetAll]
    end

    subgraph Orders
        O_Create[Create]
        O_AddItem[AddItem]
        O_Validate[Validate]
    end
    
    Login --> |JWT Token| Products
    Login --> |JWT Token| Clients
    Login --> |JWT Token| Orders
```

---

## Authentification

### POST /api/auth/login

```mermaid
sequenceDiagram
    participant Client
    participant Auth
    participant JWT
    
    Client->>Auth: POST /login {username, password}
    Auth->>Auth: Validation credentials
    alt Credentials valides
        Auth->>JWT: Generation token
        JWT-->>Auth: Token JWT
        Auth-->>Client: 200 {token}
    else Credentials invalides
        Auth-->>Client: 401 Unauthorized
    end
```

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
| 200 | Authentification reussie, retourne le token |
| 401 | Identifiants incorrects |

---

## Produits

> **Note:** Tous les endpoints `/api/products` necessitent un token JWT valide.

### GET /api/products

Recupere la liste de tous les produits.

**Responses:**

| Code | Description |
|------|-------------|
| 200 | Liste des produits |
| 401 | Non autorise |

**Exemple de reponse:**

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

### GET /api/products/{id}

Recupere un produit par son identifiant.

**Path Parameters:**

| Parametre | Type | Description |
|-----------|------|-------------|
| `id` | UUID | Identifiant unique du produit |

**Responses:**

| Code | Description |
|------|-------------|
| 200 | Details du produit |
| 404 | Produit non trouve |
| 401 | Non autorise |

---

### POST /api/products

```mermaid
sequenceDiagram
    participant Client
    participant API
    participant Service
    participant Domain
    
    Client->>API: POST {name, price, currency}
    API->>Service: CreateProduct(request)
    Service->>Domain: new Product(name, price)
    alt Donnees valides
        Domain-->>Service: Product cree
        Service-->>API: Product ID
        API-->>Client: 201 Created
    else Donnees invalides
        Domain-->>Service: DomainException
        Service-->>API: Exception
        API-->>Client: 400 Bad Request
    end
```

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
| 201 | Produit cree avec succes |
| 400 | Donnees invalides |
| 401 | Non autorise |

---

### PUT /api/products/{id}/price

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
| 204 | Prix modifie avec succes |
| 400 | Prix invalide |
| 404 | Produit non trouve |
| 401 | Non autorise |

---

### DELETE /api/products/{id}

Supprime un produit (soft delete).

**Responses:**

| Code | Description |
|------|-------------|
| 204 | Produit supprime |
| 401 | Non autorise |

---

---

## Clients

### POST /api/client

Crée un nouveau client.

**Request Body:**

```json
{
  "name": "Jean Dupont",
  "email": "jean.dupont@example.com",
  "address": "10 Rue de la Paix, Paris"
}
```

**Responses:**

| Code | Description |
|------|-------------|
| 201 | Client créé |
| 400 | Données invalides |

---

## Fournisseurs

### POST /api/fournisseur

Crée un nouveau fournisseur.

**Request Body:**

```json
{
  "name": "TechSupply Co.",
  "contactEmail": "contact@techsupply.com"
}
```

---

## Commandes

### POST /api/commande

Initie une nouvelle commande pour un client.

**Request Body:**

```json
{
  "clientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### POST /api/commande/{id}/items

Ajoute un produit à la commande.

**Request Body:**

```json
{
  "productId": "7fa85f64-5717-4562-b3fc-2c963f66afa6",
  "quantity": 2
}
```

### POST /api/commande/{id}/validate

Valide la commande et change son statut.

---

## Codes d'erreur

```mermaid
flowchart TD
    Request[Requete HTTP] --> Auth{Token JWT?}
    Auth -->|Non| E401[401 Unauthorized]
    Auth -->|Oui| Validation{Donnees valides?}
    Validation -->|Non| E400[400 Bad Request]
    Validation -->|Oui| Exists{Ressource existe?}
    Exists -->|Non| E404[404 Not Found]
    Exists -->|Oui| Success[200/201/204 Success]
```

| Code HTTP | Signification | Action recommandee |
|-----------|---------------|-------------------|
| 400 | Bad Request | Verifier les donnees envoyees |
| 401 | Unauthorized | Verifier le token JWT |
| 404 | Not Found | Verifier l'identifiant |
| 500 | Server Error | Contacter l'administrateur |

---

## Swagger UI

Pour une documentation interactive, accedez a Swagger UI une fois l'API lancee :

```
https://localhost:5001/swagger
```
