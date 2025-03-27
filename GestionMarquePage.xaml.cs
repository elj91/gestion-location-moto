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
    /// Interaction logic for GestionMarquePage.xaml
    /// </summary>
    public partial class GestionMarquePage : Window
    {
        public GestionMarquePage()
        {
            InitializeComponent();
            ChargerMarques();
        }
        // Charger la liste des marques
        private void ChargerMarques()
        {
            try
            {
                List<Marque> marques = Marque.GetAllMarques();
                lvMarques.ItemsSource = marques;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des marques : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Événement de clic sur le bouton d'ajout de marque
        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            string nomMarque = txtNomMarque.Text.Trim();

            // Validation basique
            if (string.IsNullOrEmpty(nomMarque))
            {
                AfficherErreur("Veuillez entrer un nom de marque.");
                return;
            }

            try
            {
                // Vérifier si la marque existe déjà
                if (Marque.MarqueExiste(nomMarque))
                {
                    AfficherErreur("Cette marque existe déjà.");
                    return;
                }

                // Ajouter la marque
                bool ajoutReussi = Marque.AjouterMarque(nomMarque);

                if (ajoutReussi)
                {
                    // Effacer le champ et masquer le message d'erreur
                    txtNomMarque.Text = "";
                    txtErreur.Visibility = Visibility.Collapsed;

                    // Rafraîchir la liste des marques
                    ChargerMarques();

                    MessageBox.Show("Marque ajoutée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    AfficherErreur("Erreur lors de l'ajout de la marque.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur("Erreur : " + ex.Message);
            }
        }

        // Événement de clic sur le bouton de suppression de marque
        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'ID de la marque à supprimer
            Button btn = (Button)sender;
            int idMarque = Convert.ToInt32(btn.Tag);

            // Demander confirmation
            MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette marque ? Toutes les motos associées seront également supprimées.",
                                                     "Confirmation",
                                                     MessageBoxButton.YesNo,
                                                     MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Tenter de supprimer la marque
                    bool suppressionReussie = Marque.SupprimerMarque(idMarque);

                    if (suppressionReussie)
                    {
                        // Rafraîchir la liste des marques
                        ChargerMarques();

                        MessageBox.Show("Marque supprimée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de la marque.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Événement de clic sur le bouton Fermer
        private void btnFermer_Click(object sender, RoutedEventArgs e)
        {
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