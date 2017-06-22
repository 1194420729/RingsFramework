using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using Npgsql;
using Newtonsoft.Json;
using System.Text;
using CsQuery;

namespace Rings.Models
{
    public class LimitLoader
    {
        public void Load()
        {

            string applicationid = PluginContext.Current.Account.ApplicationId;

            List<Limit> list = new List<Limit>();

            //查找定制目录下的权限配置 
            string applicationpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views\\" + applicationid);
            string[] subdirs = new string[] { };
            if (Directory.Exists(applicationpath))
            {
                subdirs = Directory.GetDirectories(applicationpath, "*", SearchOption.TopDirectoryOnly);
            }

            foreach (string dir in subdirs)
            {
                string path = Path.Combine(dir, "limits.config.xml");
                if (File.Exists(path))
                {
                    list.AddRange(ReadConfigFile(path, applicationid));
                }
            }

            //查找默认目录下的权限配置
            string defaultpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views\\Default");
            string[] defaultsubdirs = Directory.GetDirectories(defaultpath, "*", SearchOption.TopDirectoryOnly);
            foreach (string subdir in defaultsubdirs)
            {
                if (subdirs.Contains(subdir)) continue;
                string path = Path.Combine(subdir, "limits.config.xml");
                if (File.Exists(path))
                {
                    list.AddRange(ReadConfigFile(path, applicationid));
                }

            }

            //删除数据库中多余的权限项
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            foreach (var item in list)
            {
                sb.Append("'");
                sb.Append(item.Name);
                sb.Append("',");
            }
            sb.Append("'')");

            DataContext db = new DataContext(applicationid);
            using (NpgsqlConnection connection = new NpgsqlConnection(db.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;

                command.CommandText = "delete from \"limit\" where content->>'name' not in " + sb.ToString();
                command.ExecuteNonQuery();

                connection.Close();
            }

        }

        private List<Limit> ReadConfigFile(string path, string applicationid)
        {
            //读取同目录下的Menu.cshtml
            string menupath = Path.Combine(new FileInfo(path).DirectoryName, "menu.cshtml");
            string html = File.ReadAllText(menupath, Encoding.UTF8);
            CQ frag = CQ.CreateFragment(html);
            var ul = frag.Select("ul[data-sort]");
            string sort = ul.DataRaw("sort");
            string group = ul.DataRaw("group");
            string groupsort = ul.DataRaw("groupsort");

            List<Limit> list = new List<Limit>();

            DataContext db = new DataContext(applicationid);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/limits/limit");

            foreach (XmlNode node in nodes)
            {
                Limit limit = new Limit()
                {
                    Name = node.Attributes["name"].Value,
                    Title = node.Attributes["title"].Value,
                    GroupName = node.Attributes["groupname"].Value,
                    ModuleName = node.Attributes["modulename"] == null ? group : node.Attributes["modulename"].Value,
                    GroupSort=Convert.ToInt32(sort),
                    ModuleSort=Convert.ToInt32(groupsort)
                };
                list.Add(limit);

                using (NpgsqlConnection connection = new NpgsqlConnection(db.ConnectionString))
                {
                    connection.Open();

                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;

                    command.CommandText = "select id from \"limit\" where content->>'name' =@name limit 1";
                    command.Parameters.Add("name", NpgsqlTypes.NpgsqlDbType.Text).Value = limit.Name;

                    object obj = command.ExecuteScalar();
                    if (obj == null)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "insert into \"limit\" (content) values (@content)";
                        command.Parameters.Add("content", NpgsqlTypes.NpgsqlDbType.Jsonb).Value = JsonConvert.SerializeObject(limit).ToLower();
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

            }

            return list;
        }
    }
}