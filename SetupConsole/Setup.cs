using System;
using System.Data.SqlClient;
using System.IO;

namespace SetupConsole
{
    class Setup
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
                        "amount_l FLOAT NOT NULL, " +
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

                    Console.WriteLine("Tables created! Press anything to continue.");
                    Console.ReadLine();

                    var commandString = "";
                    using (var reader = new StreamReader("../../../../RemoveCurrentHopsFromBeer.sql"))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            commandString = String.Concat(commandString, line, " \r\n");
                        }
                    }

                    command = new SqlCommand(commandString, conn);
                    command.ExecuteNonQuery();
                    commandString = "";
                    using (var reader = new StreamReader("../../../../UpsertBeer.sql"))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            commandString = String.Concat(commandString, line, " \r\n");
                        }
                    }

                    command = new SqlCommand(commandString, conn);
                    command.ExecuteNonQuery();
                    commandString = "";

                    using (var reader = new StreamReader("../../../../UpsertBeerHasHop.sql"))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            commandString = String.Concat(commandString, line, " \r\n");
                        }
                    }

                    command = new SqlCommand(commandString, conn);
                    command.ExecuteNonQuery();
                    commandString = "";

                    using (var reader = new StreamReader("../../../../UpsertHop.sql"))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            commandString = String.Concat(commandString, line, " \r\n");
                        }
                    }

                    command = new SqlCommand(commandString, conn);
                    command.ExecuteNonQuery();
                    commandString = "";

                    using (var reader = new StreamReader("../../../../RemoveBeer.sql"))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            commandString = String.Concat(commandString, line, " \r\n");
                        }
                    }

                    command = new SqlCommand(commandString, conn);
                    command.ExecuteNonQuery();
                    conn.Close();

                    Console.WriteLine("Procedures created, you are good to go!");
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
