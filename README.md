Library Manager - Application Web ASP.NET Core MVC
Application web de gestion de bibliothèque développée avec ASP.NET Core MVC et Entity Framework Core, suivant une architecture en couches propre et maintenable.

📐 Architecture du Projet
Le projet est divisé en deux solutions distinctes pour séparer les responsabilités :

Library.DAL (Data Access Layer) : Gestion des données, entités et accès SQL Server via Entity Framework Core.
Library.BL (Business Logic Layer) : Logique métier, services et DTOs.
Library.Web (Présentation) : Interface utilisateur ASP.NET Core MVC.
🛠️ Technologies Utilisées
Framework : .NET 6 / 7 / 8
Base de données : SQL Server (LocalDB)
ORM : Entity Framework Core
Pattern : MVC, Repository, Dependency Injection
✨ Fonctionnalités
✅ CRUD complet des livres (Livres, Auteur, ISBN).
✅ Gestion des emprunts (Emprunt, Retour, Statut).
✅ Mise à jour automatique des stocks disponibles.
✅ Validation des données côté serveur.
🚀 Installation et Lancement
Cloner le dépôt
git clone <url-du-repo>
Configuration de la Base de Données
Ouvrez le fichier Library.Web/appsettings.json.
Vérifiez la DefaultConnection (ciblée par défaut sur LocalDB).
Restauration des Packages
Ouvrez la solution dans Visual Studio.
Clic droit sur la solution > "Restaurer les packages NuGet".
Base de Données (Migration)
La base de données est créée automatiquement au démarrage via db.Database.Migrate().
Sinon, exécutez dans la Console du Gestionnaire de Package :
Update-Database
Lancer l'application
Définissez Library.Web comme projet de démarrage.
Appuyez sur F5.
📁 Structure des Dossiers
LibraryManager/├── Library.DAL/           # Accès aux données (Entities, Context, Repositories)├── Library.BL/            # Logique métier (Services, DTOs, Interfaces)├── Library.Web/           # Application Web MVC (Controllers, Views)├── LibraryManager.slnx    # Fichier de solution principal└── README.md
  
