using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projet_location
{
    public class Marque
    {
        // Propriétés
        public int IdMarque { get; set; }
        public string NomMarque { get; set; }

        // Constructeur vide
        public Marque()
        {
        }

        // Constructeur avec paramètres
        public Marque(int idMarque, string nomMarque)
        {
            IdMarque = idMarque;
            NomMarque = nomMarque;
        }

        // Récupérer toutes les marques
        public static List<Marque> GetAllMarques()
        {
            List<Marque> marques = new List<Marque>();

            string query = "SELECT id_marque, nom_marque FROM marque ORDER BY nom_marque";

            DataTable dataTable = BDDConnection.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                marques.Add(new Marque(
                    Convert.ToInt32(row["id_marque"]),
                    row["nom_marque"].ToString()
                ));
            }

            return marques;
        }

        // Ajouter une nouvelle marque
        public static bool AjouterMarque(string nomMarque)
        {
            string query = "INSERT INTO marque (nom_marque) VALUES (@nomMarque)";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@nomMarque", nomMarque)
            };

            int rowsAffected = BDDConnection.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }

        // Supprimer une marque
        public static bool SupprimerMarque(int idMarque)
        {
            string query = "DELETE FROM marque WHERE id_marque = @idMarque";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@idMarque", idMarque)
            };

            int rowsAffected = BDDConnection.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }

        // Vérifier si une marque existe
        public static bool MarqueExiste(string nomMarque)
        {
            string query = "SELECT COUNT(id_marque) FROM marque WHERE nom_marque = @nomMarque";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@nomMarque", nomMarque)
            };

            object result = BDDConnection.ExecuteScalar(query, parameters);

            return Convert.ToInt32(result) > 0;
        }
    }
}