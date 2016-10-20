using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Rings.Models
{
    public static class LimitHelper
    {
        public static bool IsAllowed(this Account account, string limitname)
        {
            if (account.UserName.ToLower() == "admin" || account.Id == 1)
            {
                return true;
            }

            if (string.IsNullOrEmpty(account.Limit)) return false;

            DataContext db = new DataContext(account.ApplicationId);
            DataTable dt = new DataTable();
            using (NpgsqlConnection connection = new NpgsqlConnection(db.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;

                command.CommandText = "select * from \"limit\" where content->>'name'=@limitname";
                command.Parameters.Add("limitname", NpgsqlTypes.NpgsqlDbType.Text).Value = limitname;
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                da.Fill(dt);

                connection.Close();
            }

            if (dt.Rows.Count == 0) return false;

            return IsAllowed(account,Convert.ToInt32(dt.Rows[0]["id"])) ;
        }

        public static bool IsAllowed(this Account account, int limitid)
        {
            if (account.UserName.ToLower() == "admin" || account.Id == 1)
            {
                return true;
            }

            if (string.IsNullOrEmpty(account.Limit)) return false;
            if (account.Limit.Length <= limitid) return false;

            return account.Limit.Substring(limitid,1)=="1";
        }
    }
}
