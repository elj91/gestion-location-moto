# MotoLocation - Système de Gestion de Location de Motos

## 📋 Présentation

MotoLocation est une application desktop professionnelle développée en C# WPF qui permet la gestion complète d'un service de location de motos. Cette application offre une interface moderne et intuitive pour administrer un parc de motos et gérer les réservations des clients.



## ✨ Fonctionnalités

### Pour les Administrateurs
- Gestion complète du parc de motos (ajout, modification, suppression)
- Administration des marques et modèles
- Suivi des réservations en cours et passées
- Tableau de bord avec statistiques (nombre de motos, réservations en cours, revenus)
- Gestion des utilisateurs

### Pour les Clients
- Visualisation des motos disponibles
- Système de réservation avec vérification en temps réel
- Gestion des réservations personnelles
- Interface intuitive pour sélectionner les dates de location

## 🛠️ Technologies Utilisées

- **Frontend**: C# WPF 
- **Backend**: C# .NET Framework
- **Base de données**: MySQL
- **Authentification**: Système sécurisé avec hachage SHA-256


## 🗄️ Structure de la Base de Données

L'application utilise une base de données relationnelle avec les tables suivantes:
- `utilisateurs`: Informations sur les clients et administrateurs
- `marque`: Catalogue des marques de motos
- `modele`: Liste des modèles par marque
- `moto`: Inventaire des motos disponibles avec caractéristiques
- `reservation`: Enregistrement des locations avec dates et tarifs

## 📥 Installation

### Prérequis
- Windows 7/8/10/11
- .NET Framework 4.7.2 ou supérieur
- MySQL Server

### Instructions
1. Clonez le dépôt: `git clone https://github.com/elj91/gestion-location-moto.git`
2. Importez le script SQL `script.sql` pour initialiser la base de données
3. Ouvrez le projet dans Visual Studio
4. Configurez la connexion à la base de données dans `BDDConnection.cs`
5. Compilez et exécutez l'application

## 👥 Comptes de démonstration

| Type | Email | Mot de passe |
|------|-------|--------------|
| Admin | admin@gmail.com | admin |
| Client | aa@gmail.com | aa |

## 📸 Captures d'écran

### Dashboard Administrateur!

![admin](https://github.com/user-attachments/assets/1b67f69e-9df8-4d19-860b-e4eea80cee75)


### Interface Client
![client](https://github.com/user-attachments/assets/98c8618f-a3e4-4742-979c-c81a6cd081ae)

### Système de Réservation
![reservation](https://github.com/user-attachments/assets/304e4a94-6e59-43fa-873e-338b606f1867)


## 📝 Licence

Ce projet a été développé dans le cadre d'une formation BTS. Tous droits réservés.

## 📞 Auteur

JAFFEL Elyes
