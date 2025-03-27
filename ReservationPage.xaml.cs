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
using System.Windows;
using System.Windows.Controls;

namespace projet_location
{
    /// <summary>
    /// Logique d'interaction pour ReservationPage.xaml
    /// </summary>
    public partial class ReservationPage : Window
    {
        private int _idMoto;
        private Moto _moto;
        private int _nombreJours = 0;
        private decimal _prixTotal = 0;
        public ReservationPage(int idMoto)
        {
            InitializeComponent();
            _idMoto = idMoto;

            // Initialiser les dates par défaut
            dpDateDebut.SelectedDate = DateTime.Today.AddDays(1);
            dpDateFin.SelectedDate = DateTime.Today.AddDays(3);

            ChargerMoto();
            CalculerPrixTotal();
        }
        // Charger les informations de la moto
        private void ChargerMoto()
        {
            try
            {
                _moto = Moto.GetMotoById(_idMoto);

                if (_moto != null)
                {
                    // Afficher les détails de la moto
                    txtMarque.Text = _moto.NomMarque;
                    txtModele.Text = _moto.NomModele;
                    txtAnnee.Text = _moto.Annee.ToString();
                    txtCylindree.Text = _moto.Cylindree + " cm³";
                    txtPrixJournalier.Text = _moto.PrixJournalier.ToString("C");
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

        // Calculer le prix total de la réservation
        private void CalculerPrixTotal()
        {
            if (_moto != null && dpDateDebut.SelectedDate.HasValue && dpDateFin.SelectedDate.HasValue)
            {
                DateTime dateDebut = dpDateDebut.SelectedDate.Value;
                DateTime dateFin = dpDateFin.SelectedDate.Value;

                // Vérifier que la date de fin est après la date de début
                if (dateFin >= dateDebut)
                {
                    // Calculer le nombre de jours
                    _nombreJours = (int)(dateFin - dateDebut).TotalDays + 1; // +1 car on compte le jour de début et le jour de fin

                    // Calculer le prix total
                    _prixTotal = _moto.PrixJournalier * _nombreJours;

                    // Mettre à jour l'affichage
                    txtNombreJours.Text = _nombreJours.ToString();
                    txtPrixTotal.Text = _prixTotal.ToString("C");

                    // Masquer le message d'erreur
                    txtErreur.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // Afficher un message d'erreur
                    AfficherErreur("La date de fin doit être postérieure ou égale à la date de début.");

                    // Réinitialiser les valeurs
                    _nombreJours = 0;
                    _prixTotal = 0;
                    txtNombreJours.Text = "0";
                    txtPrixTotal.Text = "0,00 €";
                }
            }
        }

        // Événement déclenché lorsqu'une date est sélectionnée
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculerPrixTotal();
        }

        // Événement de clic sur le bouton Confirmer
        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            if (_moto == null)
            {
                AfficherErreur("Erreur : impossible de récupérer les informations de la moto.");
                return;
            }

            if (!dpDateDebut.SelectedDate.HasValue || !dpDateFin.SelectedDate.HasValue)
            {
                AfficherErreur("Veuillez sélectionner des dates valides.");
                return;
            }

            DateTime dateDebut = dpDateDebut.SelectedDate.Value;
            DateTime dateFin = dpDateFin.SelectedDate.Value;

            // Vérifier que les dates sont dans le futur
            if (dateDebut < DateTime.Today)
            {
                AfficherErreur("La date de début doit être dans le futur.");
                return;
            }

            // Vérifier que la date de fin est après la date de début
            if (dateFin < dateDebut)
            {
                AfficherErreur("La date de fin doit être postérieure ou égale à la date de début.");
                return;
            }

            try
            {
                // Vérifier la disponibilité de la moto pour la période demandée
                if (!Moto.EstDisponible(_idMoto, dateDebut, dateFin))
                {
                    AfficherErreur("La moto n'est pas disponible pour la période sélectionnée.");
                    return;
                }

                // Récupérer l'utilisateur connecté
                if (App.UtilisateurConnecte == null)
                {
                    MessageBox.Show("Vous devez être connecté pour effectuer une réservation.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Créer la réservation
                bool reservationReussie = Reservation.CreerReservation(
                    App.UtilisateurConnecte.IdUtilisateur,
                    _idMoto,
                    dateDebut,
                    dateFin,
                    _prixTotal
                );

                if (reservationReussie)
                {
                    MessageBox.Show("Réservation effectuée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    AfficherErreur("Erreur lors de la création de la réservation.");
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