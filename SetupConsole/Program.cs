using System;
using System.Data.SqlClient;

namespace SetupConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder();

                builder.DataSource = "localhost";
                builder.UserID = "sa";
                builder.Password = "AdminAdmin";

                using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
                {
                    conn.Open();
                    Console.WriteLine("Connection opened!");

                    var sql = "CREATE DATABASE BeerDb";
                    var command = new SqlCommand(sql, conn);
                    command.ExecuteNonQuery();

                    Console.WriteLine("Database created! Press anything to continue.");
                    Console.ReadLine();

                    command.CommandText = "USE BeerDb " +
                        "CREATE TABLE Hop " +
                        "(id INT PRIMARY KEY IDENTITY, " +
                        "title VARCHAR(100) NOT NULL, " +
                        "alpha_percentage FLOAT NOT NULL) " +

                        "CREATE Table Beer " +
                        "(id INT PRIMARY KEY IDENTITY, " +
                        "title VARCHAR(100) NOT NULL, " +
                        "amount_l INT NOT NULL, " +
                        "ibu INT, " +
                        "alcohol_percentage FLOAT, " +
                        "create_date DATETIME, " +
                        "modified_date DATETIME, " +
                        "density_start FLOAT, " +
                        "density_end FLOAT, " +
                        "malt_extract_used_kg FLOAT) " +

                        "CREATE TABLE Beer_has_hop " +
                        "(id INT PRIMARY KEY IDENTITY, " +
                        "beer_id INT NOT NULL FOREIGN KEY REFERENCES beer(id), " +
                        "hop_id INT NOT NULL FOREIGN KEY REFERENCES hop(id), " +
                        "weight_g INT NOT NULL, " +
                        "boiling_time_min INT NOT NULL)";
                    command.ExecuteNonQuery();

                    Console.WriteLine("Tables created!");
                    Console.ReadLine();
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
