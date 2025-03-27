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
    /// Logique d'interaction pour GestionMotoPage.xaml
    /// </summary>
    public partial class GestionMotoPage : Window
    {
        private int _idMoto; // -1 pour un ajout, > 0 pour une modification
        private Moto _moto;

        // Classe pour afficher les modèles avec leur marque dans le ComboBox
        private class ModeleCombo
        {
            public int IdModele { get; set; }
            public string NomComplet { get; set; } // Format: "Marque - Modèle"
        }
        public GestionMotoPage(int idMoto)
        {
            InitializeComponent();
            _idMoto = idMoto;

            if (_idMoto > 0)
            {
                // Mode modification
                txtTitre.Text = "Modifier une moto";
                ChargerMoto();
            }
            else
            {
                // Mode ajout
                txtTitre.Text = "Ajouter une moto";
            }

            ChargerModeles();
        }
        // Charger les modèles pour le ComboBox
        private void ChargerModeles()
        {
            try
            {
                List<Modele> modeles = Modele.GetAllModeles();

                // Transformer la liste pour l'affichage dans le ComboBox
                List<ModeleCombo> modelesCombo = modeles.Select(m => new ModeleCombo
                {
                    IdModele = m.IdModele,
                    NomComplet = $"{m.NomMarque} - {m.NomModele}"
                }).ToList();

                cmbModele.ItemsSource = modelesCombo;

                // Sélectionner le premier modèle par défaut en mode ajout
                if (_idMoto <= 0 && modelesCombo.Count > 0)
                {
                    cmbModele.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des modèles : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Charger les informations de la moto à modifier
        private void ChargerMoto()
        {
            try
            {
                _moto = Moto.GetMotoById(_idMoto);

                if (_moto != null)
                {
                    // Sélectionner le modèle correspondant
                    cmbModele.SelectedValue = _moto.IdModele;

                    // Remplir les autres champs
                    txtAnnee.Text = _moto.Annee.ToString();
                    txtCylindree.Text = _moto.Cylindree.ToString();
                    txtPrixJournalier.Text = _moto.PrixJournalier.ToString("F2");
                    txtImmatriculation.Text = _moto.Immatriculation;
                    chkDisponible.IsChecked = _moto.Disponible;
                }
                else
                {
                    MessageBox.Show("La moto demandée n'a pas été trouvée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement de la moto : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        // Événement de clic sur le bouton Enregistrer
        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier qu'un modèle est sélectionné
            if (cmbModele.SelectedItem == null)
            {
                AfficherErreur("Veuillez sélectionner un modèle.");
                return;
            }

            // Récupération des valeurs saisies
            string anneeText = txtAnnee.Text.Trim();
            string cylindreeText = txtCylindree.Text.Trim();
            string prixText = txtPrixJournalier.Text.Trim();
            string immatriculation = txtImmatriculation.Text.Trim();
            bool disponible = chkDisponible.IsChecked ?? true;

            // Validation des champs
            if (string.IsNullOrEmpty(anneeText) || string.IsNullOrEmpty(cylindreeText) ||
                string.IsNullOrEmpty(prixText) || string.IsNullOrEmpty(immatriculation))
            {
                AfficherErreur("Veuillez remplir tous les champs.");
                return;
            }

            // Conversion et validation des valeurs numériques
            if (!int.TryParse(anneeText, out int annee) || annee < 1900 || annee > DateTime.Now.Year + 1)
            {
                AfficherErreur("Veuillez entrer une année valide.");
                return;
            }

            if (!int.TryParse(cylindreeText, out int cylindree) || cylindree <= 0)
            {
                AfficherErreur("Veuillez entrer une cylindrée valide.");
                return;
            }

            if (!decimal.TryParse(prixText.Replace(',', '.'), out decimal prix) || prix <= 0)
            {
                AfficherErreur("Veuillez entrer un prix journalier valide.");
                return;
            }

            // Récupérer l'ID du modèle sélectionné
            int idModele = (int)cmbModele.SelectedValue;

            try
            {
                // Vérifier si l'immatriculation existe déjà (sauf pour la moto actuelle en cas de modification)
                if (Moto.ImmatriculationExiste(immatriculation, _idMoto))
                {
                    AfficherErreur("Cette immatriculation existe déjà.");
                    return;
                }

                bool operationReussie;

                if (_idMoto > 0)
                {
                    // Mode modification
                    operationReussie = Moto.ModifierMoto(_idMoto, idModele, annee, cylindree, prix, immatriculation, disponible);
                }
                else
                {
                    // Mode ajout
                    operationReussie = Moto.AjouterMoto(idModele, annee, cylindree, prix, immatriculation, disponible);
                }

                if (operationReussie)
                {
                    MessageBox.Show(_idMoto > 0 ? "Moto modifiée avec succès." : "Moto ajoutée avec succès.",
                                   "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    AfficherErreur(_idMoto > 0 ? "Erreur lors de la modification de la moto." : "Erreur lors de l'ajout de la moto.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur("Erreur : " + ex.Message);
            }
        }

        // Événement de clic sur le bouton Annuler
        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
