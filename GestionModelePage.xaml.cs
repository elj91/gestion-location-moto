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
    /// Interaction logic for GestionModelePage.xaml
    /// </summary>
    public partial class GestionModelePage : Window
    {
        public GestionModelePage()
        {
            InitializeComponent();
            ChargerMarques();
            ChargerModeles();
        }
        // Charger la liste des marques pour le ComboBox
        private void ChargerMarques()
        {
            try
            {
                List<Marque> marques = Marque.GetAllMarques();
                cmbMarque.ItemsSource = marques;

                // Sélectionner la première marque par défaut si disponible
                if (marques.Count > 0)
                {
                    cmbMarque.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des marques : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Charger la liste des modèles
        private void ChargerModeles()
        {
            try
            {
                List<Modele> modeles = Modele.GetAllModeles();
                lvModeles.ItemsSource = modeles;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des modèles : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Événement de clic sur le bouton d'ajout de modèle
        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier qu'une marque est sélectionnée
            if (cmbMarque.SelectedItem == null)
            {
                AfficherErreur("Veuillez sélectionner une marque.");
                return;
            }

            string nomModele = txtNomModele.Text.Trim();
            int idMarque = (int)cmbMarque.SelectedValue;

            // Validation basique
            if (string.IsNullOrEmpty(nomModele))
            {
                AfficherErreur("Veuillez entrer un nom de modèle.");
                return;
            }

            try
            {
                // Vérifier si le modèle existe déjà pour cette marque
                if (Modele.ModeleExiste(nomModele, idMarque))
                {
                    AfficherErreur("Ce modèle existe déjà pour cette marque.");
                    return;
                }

                // Ajouter le modèle
                bool ajoutReussi = Modele.AjouterModele(nomModele, idMarque);

                if (ajoutReussi)
                {
                    // Effacer le champ et masquer le message d'erreur
                    txtNomModele.Text = "";
                    txtErreur.Visibility = Visibility.Collapsed;

                    // Rafraîchir la liste des modèles
                    ChargerModeles();

                    MessageBox.Show("Modèle ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    AfficherErreur("Erreur lors de l'ajout du modèle.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur("Erreur : " + ex.Message);
            }
        }

        // Événement de clic sur le bouton de suppression de modèle
        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'ID du modèle à supprimer
            Button btn = (Button)sender;
            int idModele = Convert.ToInt32(btn.Tag);

            // Demander confirmation
            MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce modèle ? Toutes les motos associées seront également supprimées.",
                                                     "Confirmation",
                                                     MessageBoxButton.YesNo,
                                                     MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Tenter de supprimer le modèle
                    bool suppressionReussie = Modele.SupprimerModele(idModele);

                    if (suppressionReussie)
                    {
                        // Rafraîchir la liste des modèles
                        ChargerModeles();

                        MessageBox.Show("Modèle supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression du modèle.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
