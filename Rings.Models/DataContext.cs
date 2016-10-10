using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;
using System.Configuration;

namespace Rings.Models
{
    public class DataContext
    {
        private string connectionstring;
         
        public DataContext(string applicationid)
        { 
            string connectionstr = ConfigurationManager.ConnectionStrings["CentralDB"].ConnectionString;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionstr))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;

                command.CommandText = "select connectionstring from corporation where applicationid=@applicationid";
                command.Parameters.Add("applicationid", NpgsqlTypes.NpgsqlDbType.Text).Value = applicationid;

                this.connectionstring = command.ExecuteScalar().ToString();

                connection.Close();
            }
        }

        public string ConnectionString { get { return this.connectionstring; } }
         
    }
}
