using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projet_location
{
    public class Reservation
    {
        // Propriétés
        public int IdReservation { get; set; }
        public int IdUtilisateur { get; set; }
        public string NomUtilisateur { get; set; } // Pour l'affichage (Nom + Prénom)
        public int IdMoto { get; set; }
        public string DetailsMoto { get; set; } // Pour l'affichage (Marque + Modèle)
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public DateTime DateReservation { get; set; }
        public decimal PrixTotal { get; set; }
        public string Etat { get; set; } // "À venir", "En cours", "Terminée"

        // Constructeur vide
        public Reservation()
        {
        }

        // Constructeur avec paramètres
        public Reservation(int idReservation, int idUtilisateur, string nomUtilisateur,
                          int idMoto, string detailsMoto, DateTime dateDebut, DateTime dateFin,
                          DateTime dateReservation, decimal prixTotal)
        {
            IdReservation = idReservation;
            IdUtilisateur = idUtilisateur;
            NomUtilisateur = nomUtilisateur;
            IdMoto = idMoto;
            DetailsMoto = detailsMoto;
            DateDebut = dateDebut;
            DateFin = dateFin;
            DateReservation = dateReservation;
            PrixTotal = prixTotal;

            // Déterminer l'état de la réservation
            DateTime now = DateTime.Now;
            if (dateDebut > now)
            {
                Etat = "À venir";
            }
            else if (dateFin < now)
            {
                Etat = "Terminée";
            }
            else
            {
                Etat = "En cours";
            }
        }

        // Récupérer toutes les réservations
        public static List<Reservation> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            string query = "SELECT r.id_reservation, r.id_utilisateur, u.nom, u.prenom, r.id_moto, " +
                          "marque.nom_marque, modele.nom_modele, moto.immatriculation, " +
                          "r.date_debut, r.date_fin, r.date_reservation, r.prix_total " +
                          "FROM reservation r " +
                          "INNER JOIN utilisateurs u ON r.id_utilisateur = u.id_utilisateur " +
                          "INNER JOIN moto ON r.id_moto = moto.id_moto " +
                          "INNER JOIN modele ON moto.id_modele = modele.id_modele " +
                          "INNER JOIN marque ON modele.id_marque = marque.id_marque " +
                          "ORDER BY r.date_debut DESC";

            DataTable dataTable = BDDConnection.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                string nomComplet = row["prenom"].ToString() + " " + row["nom"].ToString();
                string detailsMoto = row["nom_marque"].ToString() + " " + row["nom_modele"].ToString() +
                                    " (" + row["immatriculation"].ToString() + ")";

                reservations.Add(new Reservation(
                    Convert.ToInt32(row["id_reservation"]),
                    Convert.ToInt32(row["id_utilisateur"]),
                    nomComplet,
                    Convert.ToInt32(row["id_moto"]),
                    detailsMoto,
                    Convert.ToDateTime(row["date_debut"]),
                    Convert.ToDateTime(row["date_fin"]),
                    Convert.ToDateTime(row["date_reservation"]),
                    Convert.ToDecimal(row["prix_total"])
                ));
            }

            return reservations;
        }

        // Récupérer les réservations d'un utilisateur
        public static List<Reservation> GetReservationsByUser(int idUtilisateur)
        {
            List<Reservation> reservations = new List<Reservation>();

            string query = "SELECT r.id_reservation, r.id_utilisateur, u.nom, u.prenom, r.id_moto, " +
                          "marque.nom_marque, modele.nom_modele, moto.immatriculation, " +
                          "r.date_debut, r.date_fin, r.date_reservation, r.prix_total " +
                          "FROM reservation r " +
                          "INNER JOIN utilisateurs u ON r.id_utilisateur = u.id_utilisateur " +
                          "INNER JOIN moto ON r.id_moto = moto.id_moto " +
                          "INNER JOIN modele ON moto.id_modele = modele.id_modele " +
                          "INNER JOIN marque ON modele.id_marque = marque.id_marque " +
                          "WHERE r.id_utilisateur = @idUtilisateur " +
                          "ORDER BY r.date_debut DESC";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idUtilisateur", idUtilisateur)
            };

            DataTable dataTable = BDDConnection.ExecuteQuery(query, parameters);

            foreach (DataRow row in dataTable.Rows)
            {
                string nomComplet = row["prenom"].ToString() + " " + row["nom"].ToString();
                string detailsMoto = row["nom_marque"].ToString() + " " + row["nom_modele"].ToString() +
                                    " (" + row["immatriculation"].ToString() + ")";

                reservations.Add(new Reservation(
                    Convert.ToInt32(row["id_reservation"]),
                    Convert.ToInt32(row["id_utilisateur"]),
                    nomComplet,
                    Convert.ToInt32(row["id_moto"]),
                    detailsMoto,
                    Convert.ToDateTime(row["date_debut"]),
                    Convert.ToDateTime(row["date_fin"]),
                    Convert.ToDateTime(row["date_reservation"]),
                    Convert.ToDecimal(row["prix_total"])
                ));
            }

            return reservations;
        }

        // Créer une nouvelle réservation
        public static bool CreerReservation(int idUtilisateur, int idMoto, DateTime dateDebut, DateTime dateFin, decimal prixTotal)
        {
            string query = "INSERT INTO reservation (id_utilisateur, id_moto, date_debut, date_fin, prix_total) " +
                          "VALUES (@idUtilisateur, @idMoto, @dateDebut, @dateFin, @prixTotal)";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idUtilisateur", idUtilisateur),
                new MySqlParameter("@idMoto", idMoto),
                new MySqlParameter("@dateDebut", dateDebut),
                new MySqlParameter("@dateFin", dateFin),
                new MySqlParameter("@prixTotal", prixTotal)
            };

            int rowsAffected = BDDConnection.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }

        // Annuler une réservation
        public static bool AnnulerReservation(int idReservation)
        {
            string query = "DELETE FROM reservation WHERE id_reservation = @idReservation";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idReservation", idReservation)
            };

            int rowsAffected = BDDConnection.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }

        // Vérifier si une réservation peut être annulée (pas en cours ou terminée)
        public static bool PeutEtreAnnulee(int idReservation)
        {
            string query = "SELECT date_debut, date_fin FROM reservation WHERE id_reservation = @idReservation";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idReservation", idReservation)
            };

            DataTable dataTable = BDDConnection.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                DateTime dateDebut = Convert.ToDateTime(dataTable.Rows[0]["date_debut"]);
                DateTime dateFin = Convert.ToDateTime(dataTable.Rows[0]["date_fin"]);
                DateTime now = DateTime.Now;

                // On peut annuler si la réservation n'a pas encore commencé
                return dateDebut > now;
            }

            return false;
        }

        // Calculer le prix total d'une location
        public static decimal CalculerPrixTotal(int idMoto, DateTime dateDebut, DateTime dateFin)
        {
            // Récupérer le prix journalier de la moto
            string query = "SELECT prix_journalier FROM moto WHERE id_moto = @idMoto";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idMoto", idMoto)
            };

            DataTable dataTable = BDDConnection.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                decimal prixJournalier = Convert.ToDecimal(dataTable.Rows[0]["prix_journalier"]);
                int nombreJours = (int)(dateFin - dateDebut).TotalDays + 1; // +1 car on compte le jour de début et le jour de fin

                return prixJournalier * nombreJours;
            }

            return 0;
        }
    }
}
