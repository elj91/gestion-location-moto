using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace projet_location
{
    /// <summary>
    /// Interaction logic for ClientDashboardPage.xaml
    /// </summary>
    public partial class ClientDashboardPage : Window
    {
        public ClientDashboardPage()
        {
            InitializeComponent();
            ChargerInfosUtilisateur();
            ChargerMotosDisponibles();
        }
        // Charger les informations de l'utilisateur connecté
        private void ChargerInfosUtilisateur()
        {
            if (App.UtilisateurConnecte != null)
            {
                txtBienvenue.Text = $"Bienvenue, {App.UtilisateurConnecte.Prenom} {App.UtilisateurConnecte.Nom}";
            }
        }

        // Charger les motos disponibles
        private void ChargerMotosDisponibles()
        {
            try
            {
                List<Moto> motos = Moto.GetMotosDisponibles();
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

        // Événement de clic sur le bouton "Liste des motos"
        private void btnListeMotos_Click(object sender, RoutedEventArgs e)
        {
            // Déjà sur cette page, ne rien faire
        }

        // Événement de clic sur le bouton "Mes réservations"
        private void btnMesReservations_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre des réservations du client
            MesReservationsPage mesReservations = new MesReservationsPage();
            mesReservations.ShowDialog();
            ChargerMotosDisponibles(); // Recharger les motos disponibles après la fermeture
        }

        // Événement de clic sur le bouton "Réserver"
        private void btnReserver_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'ID de la moto à réserver
            Button btn = (Button)sender;
            int idMoto = Convert.ToInt32(btn.Tag);

            // Ouvrir la fenêtre de réservation
            ReservationPage reservation = new ReservationPage(idMoto);
            reservation.ShowDialog();
            ChargerMotosDisponibles(); // Recharger les motos disponibles après la fermeture
        }
    }
}