📚 Library Manager - Application Web ASP.NET Core MVC

Application web de gestion de bibliothèque développée avec ASP.NET Core MVC et Entity Framework Core, basée sur une architecture en couches propre, modulaire et maintenable.

📐 Architecture du Projet

Le projet est structuré en 3 couches principales :

🔹 Library.DAL (Data Access Layer)
Gestion de la base de données
Entités (Models)
Accès aux données via Entity Framework Core
🔹 Library.BL (Business Logic Layer)
Logique métier
Services
DTOs (Data Transfer Objects)
🔹 Library.Web (Présentation)
Interface utilisateur (ASP.NET Core MVC)
Contrôleurs, Vues
🛠️ Technologies Utilisées
Framework : .NET 6 / 7 / 8
Base de données : SQL Server (LocalDB)
ORM : Entity Framework Core
Architecture : MVC
Design Patterns :
Repository Pattern
Dependency Injection
✨ Fonctionnalités
📘 Gestion des Livres
CRUD complet (Créer, Lire, Modifier, Supprimer)
Gestion des informations : Titre, Auteur, ISBN
🔄 Gestion des Emprunts
Emprunter un livre
Retourner un livre
Gestion du statut (Disponible / Emprunté)
📦 Gestion du Stock
Mise à jour automatique des quantités disponibles
✅ Validation
Validation des données côté serveur
📌 Bonnes Pratiques Utilisées
Séparation claire des responsabilités (DAL / BL / Web)
Injection de dépendances pour un code testable
Utilisation de DTOs pour sécuriser les échanges
Architecture scalable et maintenable
📎 Améliorations Futures (Optionnel)
Authentification & Autorisation (Identity)
API REST (Web API)
Pagination & recherche avancée
Dashboard statistiques
1️⃣ Cloner le projet
