using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projet_location
{
    /// <summary>
    /// Interaction logic for AdminDashboardPage.xaml
    /// </summary>
    public partial class AdminDashboardPage : Window
    {
        public AdminDashboardPage()
        {
            InitializeComponent();
            ChargerInfosUtilisateur();
            ChargerMotos();
        }
        // Charger les informations de l'administrateur connecté
        private void ChargerInfosUtilisateur()
        {
            if (App.UtilisateurConnecte != null)
            {
                txtBienvenue.Text = $"Bienvenue, {App.UtilisateurConnecte.Prenom} {App.UtilisateurConnecte.Nom}";
            }
        }

        // Charger toutes les motos
        private void ChargerMotos()
        {
            try
            {
                List<Moto> motos = Moto.GetAllMotos();
                lvMotos.ItemsSource = motos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des motos : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Événement de clic sur le bouton de déconnexion
        private void btnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialiser l'utilisateur connecté
            App.UtilisateurConnecte = null;

            // Rediriger vers la page de connexion
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            this.Close();
        }

        // Événement de clic sur le bouton "Gestion des motos"
        private void btnGestionMotos_Click(object sender, RoutedEventArgs e)
        {
            // Déjà sur cette page, ne rien faire
        }

        // Événement de clic sur le bouton "Gestion des marques"
        private void btnGestionMarques_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de gestion des marques
            GestionMarquePage gestionMarque = new GestionMarquePage();
            gestionMarque.ShowDialog(); // Utilisation de ShowDialog pour bloquer l'interface jusqu'à la fermeture
            ChargerMotos(); // Recharger les motos après la fermeture
        }

        // Événement de clic sur le bouton "Gestion des modèles"
        private void btnGestionModeles_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de gestion des modèles
            GestionModelePage gestionModele = new GestionModelePage();
            gestionModele.ShowDialog(); // Utilisation de ShowDialog pour bloquer l'interface jusqu'à la fermeture
            ChargerMotos(); // Recharger les motos après la fermeture
        }

        // Événement de clic sur le bouton "Gestion des réservations"
        private void btnGestionReservations_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de gestion des réservations
            GestionReservationsPage gestionReservations = new GestionReservationsPage();
            gestionReservations.ShowDialog(); // Utilisation de ShowDialog pour bloquer l'interface jusqu'à la fermeture
        }

        // Événement de clic sur le bouton "Ajouter une moto"
        private void btnAjouterMoto_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de gestion de moto (ajout)
            GestionMotoPage gestionMoto = new GestionMotoPage(-1); // -1 indique un ajout
            gestionMoto.ShowDialog();
            ChargerMotos(); // Recharger les motos après la fermeture
        }

        // Événement de clic sur le bouton "Modifier" d'une moto
        private void btnModifierMoto_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'ID de la moto à modifier
            Button btn = (Button)sender;
            int idMoto = Convert.ToInt32(btn.Tag);

            // Ouvrir la fenêtre de gestion de moto (modification)
            GestionMotoPage gestionMoto = new GestionMotoPage(idMoto);
            gestionMoto.ShowDialog();
            ChargerMotos(); // Recharger les motos après la fermeture
        }

        // Événement de clic sur le bouton "Supprimer" d'une moto
        private void btnSupprimerMoto_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'ID de la moto à supprimer
            Button btn = (Button)sender;
            int idMoto = Convert.ToInt32(btn.Tag);

            // Demander confirmation
            MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette moto ?",
                                                     "Confirmation",
                                                     MessageBoxButton.YesNo,
                                                     MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Tenter de supprimer la moto
                    bool suppressionReussie = Moto.SupprimerMoto(idMoto);

                    if (suppressionReussie)
                    {
                        MessageBox.Show("Moto supprimée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Rafraîchir la liste des motos
                        ChargerMotos();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de la moto.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}