using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Converter pour afficher ou masquer le bouton d'annulation en fonction de l'état de la réservation
    /// </summary>
    public class EtatToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string etat = value as string;

            // On peut annuler uniquement les réservations à venir
            if (etat == "À venir")
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Logique d'interaction pour GestionReservationsPage.xaml
    /// </summary>
    public partial class GestionReservationsPage : Window
    {
        public GestionReservationsPage()
        {
            InitializeComponent();
            ChargerReservations();
        }
        // Charger toutes les réservations
        private void ChargerReservations()
        {
            try
            {
                List<Reservation> reservations = Reservation.GetAllReservations();
                lvReservations.ItemsSource = reservations;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des réservations : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Événement de clic sur le bouton d'annulation d'une réservation
        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'ID de la réservation à annuler
            Button btn = (Button)sender;
            int idReservation = Convert.ToInt32(btn.Tag);

            // Demander confirmation
            MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir annuler cette réservation ?",
                                                     "Confirmation",
                                                     MessageBoxButton.YesNo,
                                                     MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Vérifier si la réservation peut être annulée
                    if (!Reservation.PeutEtreAnnulee(idReservation))
                    {
                        MessageBox.Show("Cette réservation ne peut pas être annulée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Tenter d'annuler la réservation
                    bool annulationReussie = Reservation.AnnulerReservation(idReservation);

                    if (annulationReussie)
                    {
                        // Rafraîchir la liste des réservations
                        ChargerReservations();

                        MessageBox.Show("Réservation annulée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'annulation de la réservation.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
