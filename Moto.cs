using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projet_location
{
    public class Moto
    {
        // Propriétés
        public int IdMoto { get; set; }
        public int IdModele { get; set; }
        public string NomModele { get; set; } // Pour l'affichage
        public string NomMarque { get; set; } // Pour l'affichage
        public int Annee { get; set; }
        public int Cylindree { get; set; }
        public decimal PrixJournalier { get; set; }
        public string Immatriculation { get; set; }
        public bool Disponible { get; set; }

        // Constructeur vide
        public Moto()
        {
        }

        // Constructeur avec paramètres
        public Moto(int idMoto, int idModele, string nomModele, string nomMarque, int annee,
                   int cylindree, decimal prixJournalier, string immatriculation, bool disponible)
        {
            IdMoto = idMoto;
            IdModele = idModele;
            NomModele = nomModele;
            NomMarque = nomMarque;
            Annee = annee;
            Cylindree = cylindree;
            PrixJournalier = prixJournalier;
            Immatriculation = immatriculation;
            Disponible = disponible;
        }

        // Récupérer toutes les motos
        public static List<Moto> GetAllMotos()
        {
            List<Moto> motos = new List<Moto>();

            string query = "SELECT moto.id_moto, moto.id_modele, modele.nom_modele, marque.nom_marque, " +
                          "moto.annee, moto.cylindree, moto.prix_journalier, moto.immatriculation, moto.disponible " +
                          "FROM moto " +
                          "INNER JOIN modele ON moto.id_modele = modele.id_modele " +
                          "INNER JOIN marque ON modele.id_marque = marque.id_marque " +
                          "ORDER BY marque.nom_marque, modele.nom_modele";

            DataTable dataTable = BDDConnection.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                motos.Add(new Moto(
                    Convert.ToInt32(row["id_moto"]),
                    Convert.ToInt32(row["id_modele"]),
                    row["nom_modele"].ToString(),
                    row["nom_marque"].ToString(),
                    Convert.ToInt32(row["annee"]),
                    Convert.ToInt32(row["cylindree"]),
                    Convert.ToDecimal(row["prix_journalier"]),
                    row["immatriculation"].ToString(),
                    Convert.ToBoolean(row["disponible"])
                ));
            }

            return motos;
        }

        // Récupérer les motos disponibles
        public static List<Moto> GetMotosDisponibles()
        {
            List<Moto> motos = new List<Moto>();

            string query = "SELECT moto.id_moto, moto.id_modele, modele.nom_modele, marque.nom_marque, " +
                          "moto.annee, moto.cylindree, moto.prix_journalier, moto.immatriculation, moto.disponible " +
                          "FROM moto " +
                          "INNER JOIN modele ON moto.id_modele = modele.id_modele " +
                          "INNER JOIN marque ON modele.id_marque = marque.id_marque " +
                          "WHERE moto.disponible = 1 " +
                          "ORDER BY marque.nom_marque, modele.nom_modele";

            DataTable dataTable = BDDConnection.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                motos.Add(new Moto(
                    Convert.ToInt32(row["id_moto"]),
                    Convert.ToInt32(row["id_modele"]),
                    row["nom_modele"].ToString(),
                    row["nom_marque"].ToString(),
                    Convert.ToInt32(row["annee"]),
                    Convert.ToInt32(row["cylindree"]),
                    Convert.ToDecimal(row["prix_journalier"]),
                    row["immatriculation"].ToString(),
                    Convert.ToBoolean(row["disponible"])
                ));
            }

            return motos;
        }

        // Vérifier si une moto est disponible pour une période donnée
        public static bool EstDisponible(int idMoto, DateTime dateDebut, DateTime dateFin)
        {
            // D'abord vérifier si la moto est en état "disponible"
            string query1 = "SELECT disponible FROM moto WHERE id_moto = @idMoto";

            MySqlParameter[] parameters1 = new MySqlParameter[]
            {
                new MySqlParameter("@idMoto", idMoto)
            };

            DataTable dataTable = BDDConnection.ExecuteQuery(query1, parameters1);

            if (dataTable.Rows.Count == 0 || !Convert.ToBoolean(dataTable.Rows[0]["disponible"]))
            {
                return false;
            }

            // Ensuite vérifier s'il n'y a pas de réservation qui chevauche la période demandée
            string query2 = "SELECT COUNT(id_reservation) FROM reservation " +
                           "WHERE id_moto = @idMoto " +
                           "AND ((date_debut BETWEEN @dateDebut AND @dateFin) " +
                           "OR (date_fin BETWEEN @dateDebut AND @dateFin) " +
                           "OR (@dateDebut BETWEEN date_debut AND date_fin) " +
                           "OR (@dateFin BETWEEN date_debut AND date_fin))";

            MySqlParameter[] parameters2 = new MySqlParameter[]
            {
                new MySqlParameter("@idMoto", idMoto),
                new MySqlParameter("@dateDebut", dateDebut),
                new MySqlParameter("@dateFin", dateFin)
            };

            object result = BDDConnection.ExecuteScalar(query2, parameters2);

            return Convert.ToInt32(result) == 0;
        }

        // Obtenir une moto par son ID
        public static Moto GetMotoById(int idMoto)
        {
            string query = "SELECT moto.id_moto, moto.id_modele, modele.nom_modele, marque.nom_marque, " +
                          "moto.annee, moto.cylindree, moto.prix_journalier, moto.immatriculation, moto.disponible " +
                          "FROM moto " +
                          "INNER JOIN modele ON moto.id_modele = modele.id_modele " +
                          "INNER JOIN marque ON modele.id_marque = marque.id_marque " +
                          "WHERE moto.id_moto = @idMoto";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idMoto", idMoto)
            };

            DataTable dataTable = BDDConnection.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];

                return new Moto(
                    Convert.ToInt32(row["id_moto"]),
                    Convert.ToInt32(row["id_modele"]),
                    row["nom_modele"].ToString(),
                    row["nom_marque"].ToString(),
                    Convert.ToInt32(row["annee"]),
                    Convert.ToInt32(row["cylindree"]),
                    Convert.ToDecimal(row["prix_journalier"]),
                    row["immatriculation"].ToString(),
                    Convert.ToBoolean(row["disponible"])
                );
            }

            return null;
        }

        // Ajouter une nouvelle moto
        public static bool AjouterMoto(int idModele, int annee, int cylindree, decimal prixJournalier, string immatriculation, bool disponible)
        {
            string query = "INSERT INTO moto (id_modele, annee, cylindree, prix_journalier, immatriculation, disponible) " +
                          "VALUES (@idModele, @annee, @cylindree, @prixJournalier, @immatriculation, @disponible)";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idModele", idModele),
                new MySqlParameter("@annee", annee),
                new MySqlParameter("@cylindree", cylindree),
                new MySqlParameter("@prixJournalier", prixJournalier),
                new MySqlParameter("@immatriculation", immatriculation),
                new MySqlParameter("@disponible", disponible ? 1 : 0)
            };

            int rowsAffected = BDDConnection.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }

        // Modifier une moto existante
        public static bool ModifierMoto(int idMoto, int idModele, int annee, int cylindree, decimal prixJournalier, string immatriculation, bool disponible)
        {
            string query = "UPDATE moto SET id_modele = @idModele, annee = @annee, cylindree = @cylindree, " +
                          "prix_journalier = @prixJournalier, immatriculation = @immatriculation, disponible = @disponible " +
                          "WHERE id_moto = @idMoto";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idMoto", idMoto),
                new MySqlParameter("@idModele", idModele),
                new MySqlParameter("@annee", annee),
                new MySqlParameter("@cylindree", cylindree),
                new MySqlParameter("@prixJournalier", prixJournalier),
                new MySqlParameter("@immatriculation", immatriculation),
                new MySqlParameter("@disponible", disponible ? 1 : 0)
            };

            int rowsAffected = BDDConnection.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }

        // Supprimer une moto
        public static bool SupprimerMoto(int idMoto)
        {
            string query = "DELETE FROM moto WHERE id_moto = @idMoto";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idMoto", idMoto)
            };

            int rowsAffected = BDDConnection.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }

        // Vérifier si une immatriculation existe déjà
        public static bool ImmatriculationExiste(string immatriculation, int idMotoExclue = -1)
        {
            string query;
            MySqlParameter[] parameters;

            if (idMotoExclue > 0)
            {
                // Pour l'édition (exclure la moto actuelle)
                query = "SELECT COUNT(id_moto) FROM moto WHERE immatriculation = @immatriculation AND id_moto != @idMoto";
                parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@immatriculation", immatriculation),
                    new MySqlParameter("@idMoto", idMotoExclue)
                };
            }
            else
            {
                // Pour la création
                query = "SELECT COUNT(id_moto) FROM moto WHERE immatriculation = @immatriculation";
                parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@immatriculation", immatriculation)
                };
            }

            object result = BDDConnection.ExecuteScalar(query, parameters);

            return Convert.ToInt32(result) > 0;
        }
    }
}
