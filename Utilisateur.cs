using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace projet_location
{
    public class Utilisateur
    {
        // Propriétés
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public bool EstAdmin { get; set; }

        // Constructeur vide
        public Utilisateur()
        {
        }

        // Constructeur avec paramètres
        public Utilisateur(int idUtilisateur, string nom, string prenom, string email, string telephone, bool estAdmin)
        {
            IdUtilisateur = idUtilisateur;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            Telephone = telephone;
            EstAdmin = estAdmin;
        }

        // Méthode pour hasher un mot de passe avec SHA-256
        public static string HashPassword(string password)
        {
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        // Méthode de connexion
        public static Utilisateur Connexion(string email, string password)
        {
            string hashedPassword = HashPassword(password);

            string query = "SELECT id_utilisateur, nom, prenom, email, telephone, est_admin " +
                          "FROM utilisateurs " +
                          "WHERE email = @email AND mot_de_passe = @motDePasse";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@email", email),
                new MySqlParameter("@motDePasse", hashedPassword)
            };

            DataTable dataTable = BDDConnection.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];

                return new Utilisateur(
                    Convert.ToInt32(row["id_utilisateur"]),
                    row["nom"].ToString(),
                    row["prenom"].ToString(),
                    row["email"].ToString(),
                    row["telephone"].ToString(),
                    Convert.ToBoolean(row["est_admin"])
                );
            }

            return null;
        }

        // Méthode d'inscription
        public static bool Inscription(string nom, string prenom, string email, string password, string telephone, bool estAdmin)
        {
            string hashedPassword = HashPassword(password);

            string query = "INSERT INTO utilisateurs (nom, prenom, email, mot_de_passe, telephone, est_admin) " +
                          "VALUES (@nom, @prenom, @email, @motDePasse, @telephone, @estAdmin)";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@nom", nom),
                new MySqlParameter("@prenom", prenom),
                new MySqlParameter("@email", email),
                new MySqlParameter("@motDePasse", hashedPassword),
                new MySqlParameter("@telephone", telephone),
                new MySqlParameter("@estAdmin", estAdmin ? 1 : 0)
            };

            int rowsAffected = BDDConnection.ExecuteNonQuery(query, parameters);

            return rowsAffected > 0;
        }

        // Méthode pour vérifier si un email existe déjà
        public static bool EmailExiste(string email)
        {
            string query = "SELECT COUNT(id_utilisateur) FROM utilisateurs WHERE email = @email";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@email", email)
            };

            object result = BDDConnection.ExecuteScalar(query, parameters);

            return Convert.ToInt32(result) > 0;
        }
    }
}