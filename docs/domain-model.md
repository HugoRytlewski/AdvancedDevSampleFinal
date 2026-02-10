# Modèle de Domaine

Cette page décrit les entités principales du domaine et leurs relations.

## Diagramme de Classes

```mermaid
classDiagram
    class Client {
        +Guid Id
        +string Name
        +string Email
        +string Address
        +UpdateAddress(string newAddress)
    }

    class Fournisseur {
        +Guid Id
        +string Name
        +string ContactEmail
    }

    class Product {
        +Guid Id
        +string Name
        +Price Price
        +bool IsActive
        +Guid? FournisseurId
        +ChangePrice(Price newPrice)
        +Deactivate()
        +Activate()
    }

    class Commande {
        +Guid Id
        +Guid ClientId
        +DateTime Date
        +CommandeStatus Status
        +List~LigneCommande~ Lignes
        +AddItem(Product, int)
        +RemoveItem(Product)
        +Validate()
        +Cancel()
    }

    class LigneCommande {
        +Guid ProductId
        +int Quantity
        +Price UnitPrice
        +decimal Total
    }

    Client "1" --> "*" Commande : Passe
    Commande "*" --> "1" Client : Appartient à
    Commande "1" *-- "*" LigneCommande : Contient
    LigneCommande "*" --> "1" Product : Référence
    Product "*" --> "0..1" Fournisseur : Fourni par
```

## Entités

### Client
Représente un client pouvant passer des commandes.
- **Propriétés** : Nom, Email, Adresse.
- **Règles** : L'email et l'adresse sont obligatoires.

### Fournisseur
Représente un fournisseur de produits.
- **Propriétés** : Nom, Email de contact.
- **Relations** : Un produit peut être lié à un fournisseur optionnel.

### Commande
Représente une transaction commerciale.
- **Cycle de vie** : `Created` -> `Validated` -> `Shipped` -> `Delivered` (ou `Cancelled`).
- **Règles** :
    - On ne peut modifier une commande que si elle est au statut `Created`.
    - Une commande doit contenir au moins une ligne pour être validée.

### Product
L'entité centrale du catalogue.
- **Nouveauté** : Ajout de la propriété `FournisseurId` pour lier un produit à son fournisseur.
