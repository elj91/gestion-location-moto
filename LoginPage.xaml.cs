using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace projet_location
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        // Événement de clic sur le bouton de connexion
        private void btnConnexion_Click(object sender, RoutedEventArgs e)
        {
            // Récupération des valeurs saisies
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;

            // Validation basique
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                AfficherErreur("Veuillez remplir tous les champs.");
                return;
            }

            try
            {
                // Tentative de connexion
                Utilisateur utilisateur = Utilisateur.Connexion(email, password);

                if (utilisateur != null)
                {
                    // Connexion réussie

                    // Stockage de l'utilisateur connecté pour la session
                    App.UtilisateurConnecte = utilisateur;

                    // Redirection vers le dashboard approprié
                    if (utilisateur.EstAdmin)
                    {
                        // Ouverture du dashboard admin
                        AdminDashboardPage adminDashboard = new AdminDashboardPage();
                        adminDashboard.Show();
                        this.Close();
                    }
                    else
                    {
                        // Ouverture du dashboard client
                        ClientDashboardPage clientDashboard = new ClientDashboardPage();
                        clientDashboard.Show();
                        this.Close();
                    }
                }
                else
                {
                    // Connexion échouée
                    AfficherErreur("Email ou mot de passe incorrect.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur("Erreur lors de la connexion : " + ex.Message);
            }
        }

        // Événement de clic sur le bouton d'inscription
        private void btnInscrire_Click(object sender, RoutedEventArgs e)
        {
            // Ouverture de la fenêtre d'inscription
            RegisterPage registerPage = new RegisterPage();
            registerPage.Show();
            this.Close();
        }

        // Méthode pour afficher un message d'erreur
        private void AfficherErreur(string message)
        {
            txtErreur.Text = message;
            txtErreur.Visibility = Visibility.Visible;
        }
    }
}