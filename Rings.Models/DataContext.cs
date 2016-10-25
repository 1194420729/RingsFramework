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
        private int corporationid;
        private string applicationid;

        public DataContext(string applicationid)
        {
            this.applicationid = applicationid;

            DataTable dt = new DataTable();
            string connectionstr = ConfigurationManager.ConnectionStrings["CentralDB"].ConnectionString;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionstr))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;

                command.CommandText = "select id,connectionstring from corporation where applicationid=@applicationid";
                command.Parameters.Add("applicationid", NpgsqlTypes.NpgsqlDbType.Text).Value = applicationid;
                
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                da.Fill(dt);
                
                connection.Close();
            }

            this.connectionstring = dt.Rows[0]["connectionstring"].ToString();
            this.corporationid = Convert.ToInt32(dt.Rows[0]["id"]);

            this.postgres_connectionstring = this.connectionstring;

            //每个登录用户拥有自己的数据库角色
            if (PluginContext.Current != null && PluginContext.Current.Account.Id > 1)
            {
                string role = this.GetCurrentUserRole();
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

        public string GetCurrentUserRole()
        {
            string role = "myuid" + this.corporationid + "_" + PluginContext.Current.Account.Id;

            return role;
        }

        public string ConnectionString { get { return this.connectionstring; } }
        public string PostgresConnectionString { get { return this.postgres_connectionstring; } }
        public int CorporationId { get { return this.corporationid; } }
        public string ApplicationId { get { return this.applicationid; } }
    }
}
