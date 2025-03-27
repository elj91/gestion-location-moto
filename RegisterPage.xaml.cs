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
using System.Windows.Shapes;

namespace projet_location
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        // Événement de clic sur le bouton d'inscription
        private void btnInscrire_Click(object sender, RoutedEventArgs e)
        {
            // Récupération des valeurs saisies
            string nom = txtNom.Text.Trim();
            string prenom = txtPrenom.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telephone = txtTelephone.Text.Trim();
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;
            bool estAdmin = chkAdmin.IsChecked ?? false;

            // Validation des champs
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(telephone) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                AfficherErreur("Veuillez remplir tous les champs.");
                return;
            }

            // Vérification que les mots de passe correspondent
            if (password != confirmPassword)
            {
                AfficherErreur("Les mots de passe ne correspondent pas.");
                return;
            }

            try
            {
                // Vérifier si l'email existe déjà
                if (Utilisateur.EmailExiste(email))
                {
                    AfficherErreur("Cet email est déjà utilisé. Veuillez en choisir un autre.");
                    return;
                }

                // Inscription de l'utilisateur
                bool inscriptionReussie = Utilisateur.Inscription(nom, prenom, email, password, telephone, estAdmin);

                if (inscriptionReussie)
                {
                    // Connexion automatique
                    Utilisateur utilisateur = Utilisateur.Connexion(email, password);
                    App.UtilisateurConnecte = utilisateur;

                    // Afficher un message de succès avant la redirection
                    MessageBox.Show("Inscription réussie !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Redirection vers le dashboard approprié
                    if (utilisateur.EstAdmin)
                    {
                        AdminDashboardPage adminDashboard = new AdminDashboardPage();
                        adminDashboard.Show();
                        this.Close();
                    }
                    else
                    {
                        ClientDashboardPage clientDashboard = new ClientDashboardPage();
                        clientDashboard.Show();
                        this.Close();
                    }
                }
                else
                {
                    AfficherErreur("Erreur lors de l'inscription. Veuillez réessayer.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur("Erreur lors de l'inscription : " + ex.Message);
            }
        }

        // Événement de clic sur le bouton de retour à la connexion
        private void btnRetourConnexion_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
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