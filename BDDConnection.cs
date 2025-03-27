using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace projet_location
{
    public class BDDConnection
    {
        // Chaîne de connexion à la base de données
        // Remplacez ces valeurs par celles fournies par votre hébergeur
        private static string server = "localhost"; // Remplacer par le serveur de votre hébergeur
        private static string database = "loc_moto"; // Remplacer par le nom de votre base de données
        private static string uid = "ej"; // Remplacer par votre identifiant
        private static string password = "ej"; // Remplacer par votre mot de passe
        

        private static string connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};CharSet=utf8mb4;Convert Zero Datetime=True;";
        // Obtenir une connexion à la base de données
        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la connexion à la base de données : " + ex.Message);
            }
        }

        // Exécuter une requête SQL sans retour de données (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'exécution de la requête : " + ex.Message);
            }
        }

        // Exécuter une requête SQL avec retour d'un DataTable (SELECT)
        public static DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'exécution de la requête : " + ex.Message);
            }
        }

        // Exécuter une requête SQL avec retour d'une seule valeur (COUNT, SUM, etc.)
        public static object ExecuteScalar(string query, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'exécution de la requête : " + ex.Message);
            }
        }
    }
}