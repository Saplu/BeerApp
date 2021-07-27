using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using IbuCalculations.Models;

namespace IbuCalculations.Services
{
    public class BeerDAO
    {
        private SqlConnectionStringBuilder _builder;
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlTransaction _transaction;

        public BeerDAO()
        {
            _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = "localhost";
            _builder.InitialCatalog = "BeerDb";
            _builder.UserID = "sa";
            _builder.Password = "AdminAdmin";
            _connection = new SqlConnection(_builder.ConnectionString);
            _command = new SqlCommand("", _connection);
        }

        public List<Beer> GetBeers()
        {
            List<Beer> beers = new List<Beer>();
            try
            {
                _connection.ConnectionString = _builder.ConnectionString;
                using (_connection)
                {
                    Beer beer = new Beer();
                    List<Hop> hops = new List<Hop>();
                    var id = 0;
                    var previous = -1;
                    _connection.Open();

                    _command.CommandText = "SELECT b.id, b.title, b.amount_l, b.ibu, b.alcohol_percentage, b.density_start, b.density_end, " +
                        "b.malt_extract_used_kg, ISNULL(h.title, ''), ISNULL(h.alpha_percentage, 0), ISNULL(bhh.weight_g, 0), ISNULL(bhh.boiling_time_min, 0) FROM Beer b " +
                        "LEFT OUTER JOIN Beer_has_hop bhh ON bhh.beer_id = b.id " +
                        "LEFT OUTER JOIN Hop h ON h.id = bhh.hop_id";

                    var reader = _command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                        if (previous == -1)
                        {
                            previous = id;
                        }
                        if (id != previous && beer.Name != null)
                        {
                            previous = id;
                            beers.Add(beer);
                            beer = new Beer();
                        }

                        beer.Name = reader.GetString(1);
                        beer.Amount = reader.GetDouble(2);
                        beer.Ibu = reader.GetInt32(3);
                        beer.AlcoholPercentage = reader.GetDouble(4);
                        beer.DensityStart = reader.GetDouble(5);
                        beer.DensityEnd = reader.GetDouble(6);
                        beer.MaltExtractKg = reader.GetDouble(7);

                        var hop = new Hop();
                        hop.Name = reader.GetString(8);
                        hop.AlphaAcid = reader.GetDouble(9);
                        hop.Weight = reader.GetInt32(10);
                        hop.BoilingTime = reader.GetInt32(11);
                        if (hop.Name != null && hop.Name.Length != 0) beer.Hops.Add(hop);
                    }
                    beers.Add(beer);
                }
                _connection.Close();
                return beers;
            }
            catch(Exception ex)
            {
                _connection.Close();
                throw ex;
            }
        }

        public void UpsertHopConnection(Hop hop)
        {
            try
            {
                _connection.ConnectionString = _builder.ConnectionString;
                using (_connection)
                {
                    _connection.Open();
                    UpsertHop(hop);
                    _connection.Close();
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
                _connection.ConnectionString = _builder.ConnectionString;
                using (_connection) {
                    _connection.Open();

                    _transaction = _connection.BeginTransaction();
                    _command.Connection = _connection;
                    _command.Transaction = _transaction;

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

                    _command.CommandText = "USE BeerDb EXEC RemoveCurrentHopsFromBeer @title";
                    _command.Parameters.Clear();
                    _command.Parameters.AddWithValue("@title", beer.Name);
                    _command.ExecuteNonQuery();

                    foreach (var hop in beer.Hops)
                    {
                        UpsertBeerHasHop(hop, beer);
                    }
                    _transaction.Commit();
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    _transaction.Rollback();
                    _connection.Close();
                    throw ex;
                }
                catch(Exception ex2)
                {
                    throw ex2;
                }
            }
        }

        public void RemoveHop(string name)
        {

        }

        public int RemoveBeer(string name)
        {
            try
            {
                _connection.ConnectionString = _builder.ConnectionString;
                using (_connection)
                {
                    _connection.Open();
                    _command.CommandText = "USE BeerDb EXEC RemoveBeer @name";
                    _command.Parameters.Clear();
                    _command.Parameters.AddWithValue("@name", name);
                    var result = _command.ExecuteScalar();
                    _connection.Close();
                    return (int)result;
                }
            }
            catch (Exception ex)
            {
                _connection.Close();
                throw ex;
            }
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
