using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;
using System.Configuration;
using System.Web;

namespace Rings.Models
{
    public class DataContext
    {
        private string connectionstring;
        private string postgres_connectionstring;

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

            this.postgres_connectionstring = this.connectionstring;

            //每个登录用户拥有自己的数据库角色
            if (PluginContext.Current != null && PluginContext.Current.Account.Id > 1)
            {
                string role = "myuid" + PluginContext.Current.Account.Id;
                string[] ss = this.connectionstring.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string uid = "uid=postgres";
                string pwd = "pwd=password";
                foreach (string s in ss)
                {
                    if (s.ToLower().StartsWith("uid"))
                    {
                        uid = s;
                    }
                    if (s.ToLower().StartsWith("pwd"))
                    {
                        pwd = s;
                    }
                }
                this.connectionstring = this.connectionstring.Replace(uid, "uid=" + role).Replace(pwd, "pwd=mypassword" + PluginContext.Current.Account.Id);
            }
        }

        public string ConnectionString { get { return this.connectionstring; } }
        public string PostgresConnectionString { get { return this.postgres_connectionstring; } }
    }
}
