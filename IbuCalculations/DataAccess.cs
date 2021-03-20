using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace IbuCalculations
{
    public class DataAccess
    {
        private SqlConnectionStringBuilder _builder;
        private SqlConnection _connection;
        private SqlCommand _command;

        public DataAccess()
        {
            _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = "localhost";
            _builder.InitialCatalog = "BeerDb";
            _builder.UserID = "sa";
            _builder.Password = "AdminAdmin";
            _connection = new SqlConnection(_builder.ConnectionString);
            _command = new SqlCommand("", _connection);
        }

        public void UpsertHopConnection(Hop hop)
        {
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    UpsertHop(hop);
                }
            }
            catch(Exception ex)
            {
                _connection.Close();
                throw ex;
            }
        }

        public void UpsertBeer(Beer beer)
        {
            try
            {
                using (_connection) {
                    _connection.Open();
                    foreach (var hop in beer.Hops)
                    {
                        UpsertHop(hop);
                    }

                    _command.CommandText = "USE BeerDb EXEC upsertBeer @title, @amount, @ibu, @alcoholPercentage, @densityStart, @densityEnd, @maltExtractUsedKg";
                    _command.Parameters.Clear();
                    _command.Parameters.AddWithValue("@title", beer.Name);
                    _command.Parameters.AddWithValue("@amount", beer.Amount);
                    _command.Parameters.AddWithValue("@ibu", beer.Ibu);
                    _command.Parameters.AddWithValue("@alcoholPercentage", beer.AlcoholPercentage);
                    _command.Parameters.AddWithValue("@densityStart", beer.DensityStart);
                    _command.Parameters.AddWithValue("@densityEnd", beer.DensityEnd);
                    _command.Parameters.AddWithValue("@maltExtractUsedKg", beer.MaltExtractKg);
                    _command.ExecuteNonQuery();

                    foreach(var hop in beer.Hops)
                    {
                        UpsertBeerHasHop(hop, beer);
                    }

                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                _connection.Close();
                throw ex;
            }
        }

        public void RemoveHop(string name)
        {

        }

        public void RemoveBeer(string name)
        {

        }

        private void UpsertHop(Hop hop)
        {
            _command.CommandText = "USE BeerDb EXEC upsertHop @name, @alpha";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@name", hop.Name);
            _command.Parameters.AddWithValue("@alpha", hop.AlphaAcid);
            _command.ExecuteNonQuery();
        }

        private void UpsertBeerHasHop(Hop hop, Beer beer)
        {
            _command.CommandText = "USE BeerDb EXEC upsertBeerHasHop @name, @title, @weight, @boilingTime";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@name", hop.Name);
            _command.Parameters.AddWithValue("@title", beer.Name);
            _command.Parameters.AddWithValue("@weight", hop.WeightGrams);
            _command.Parameters.AddWithValue("@boilingTime", hop.BoilingTime);
            _command.ExecuteNonQuery();
        }
    }
}
