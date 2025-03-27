using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projet_location
{
    public class Modele
    {
        // Propriétés
        public int IdModele { get; set; }
        public string NomModele { get; set; }
        public int IdMarque { get; set; }
        public string NomMarque { get; set; } // Pour l'affichage

        // Constructeur vide
        public Modele()
        {
        }

        // Constructeur avec paramètres
        public Modele(int idModele, string nomModele, int idMarque, string nomMarque = null)
        {
            IdModele = idModele;
            NomModele = nomModele;
            IdMarque = idMarque;
            NomMarque = nomMarque;
        }

        // Récupérer tous les modèles
        public static List<Modele> GetAllModeles()
        {
            List<Modele> modeles = new List<Modele>();

            string query = "SELECT modele.id_modele, modele.nom_modele, modele.id_marque, marque.nom_marque " +
                          "FROM modele " +
                          "INNER JOIN marque ON modele.id_marque = marque.id_marque " +
                          "ORDER BY marque.nom_marque, modele.nom_modele";

            DataTable dataTable = BDDConnection.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                modeles.Add(new Modele(
                    Convert.ToInt32(row["id_modele"]),
                    row["nom_modele"].ToString(),
                    Convert.ToInt32(row["id_marque"]),
                    row["nom_marque"].ToString()
                ));
            }

            return modeles;
        }

        // Récupérer les modèles par marque
        public static List<Modele> GetModelesByMarque(int idMarque)
        {
            List<Modele> modeles = new List<Modele>();

            string query = "SELECT modele.id_modele, modele.nom_modele, modele.id_marque, marque.nom_marque " +
                          "FROM modele " +
                          "INNER JOIN marque ON modele.id_marque = marque.id_marque " +
                          "WHERE modele.id_marque = @idMarque " +
                          "ORDER BY modele.nom_modele";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idMarque", idMarque)
            };

            DataTable dataTable = BDDConnection.ExecuteQuery(query, parameters);

            foreach (DataRow row in dataTable.Rows)
            {
                modeles.Add(new Modele(
                    Convert.ToInt32(row["id_modele"]),
                    row["nom_modele"].ToString(),
                    Convert.ToInt32(row["id_marque"]),
                    row["nom_marque"].ToString()
                ));
            }

            return modeles;
        }

        // Ajouter un nouveau modèle
        public static bool AjouterModele(string nomModele, int idMarque)
        {
            string query = "INSERT INTO modele (nom_modele, id_marque) VALUES (@nomModele, @idMarque)";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@nomModele", nomModele),
                new MySqlParameter("@idMarque", idMarque)
            };

            int rowsAffected = BDDConnection.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }

        // Supprimer un modèle
        public static bool SupprimerModele(int idModele)
        {
            string query = "DELETE FROM modele WHERE id_modele = @idModele";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idModele", idModele)
            };

            int rowsAffected = BDDConnection.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }

        // Vérifier si un modèle existe déjà pour une marque
        public static bool ModeleExiste(string nomModele, int idMarque)
        {
            string query = "SELECT COUNT(id_modele) FROM modele WHERE nom_modele = @nomModele AND id_marque = @idMarque";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@nomModele", nomModele),
                new MySqlParameter("@idMarque", idMarque)
            };

            object result = BDDConnection.ExecuteScalar(query, parameters);

            return Convert.ToInt32(result) > 0;
        }
    }
}
