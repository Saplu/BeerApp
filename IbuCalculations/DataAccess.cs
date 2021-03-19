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
        private StringBuilder _sb;

        public DataAccess()
        {
            _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = "localhost";
            _builder.InitialCatalog = "BeerDb";
            _builder.UserID = "sa";
            _builder.Password = "AdminAdmin";
            _connection = new SqlConnection(_builder.ConnectionString);
            _command = new SqlCommand("", _connection);
            _sb = new StringBuilder();
        }

        public void UpsertHop(Hop hop)
        {
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    _command.CommandText = "USE BeerDb EXEC upsertHop @name, @alpha";
                    _command.Parameters.AddWithValue("@name", hop.Name);
                    _command.Parameters.AddWithValue("@alpha", hop.AlphaAcid);
                    _command.ExecuteNonQuery();
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
            foreach(var hop in beer.Hops)
            {
                UpsertHop(hop);
            }
        }

        public void RemoveHop(string name)
        {

        }

        public void RemoveBeer(string name)
        {

        }
    }
}
