using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Npgsql;

namespace Rings.Models
{
    public class PluginLoader : MarshalByRefObject
    { 
        public string Run(string assemblypath, string classname, string methodname, string parameters, Account account)
        {
            PluginContext.Current = new PluginContext()
            {
                Account = account
            };

            string accountname = account.Name;
            string applicationid = account.ApplicationId;

            object result = new { };

            Assembly assem = Assembly.LoadFrom(assemblypath);

            Type[] types = assem.GetTypes();
            foreach (var item in types)
            {
                if (item.IsClass && item.Name.ToUpper() == classname.ToUpper())
                {
                    var method = item.GetMethod(methodname, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (method == null)
                    {
                        return JsonConvert.SerializeObject(new { message = "服务不存在！" });
                    }
                    var instance = assem.CreateInstance(item.FullName);

                    //记录日志
                    object[] attrs = method.GetCustomAttributes(typeof(MyLogAttribute), false);
                    if (attrs.Length > 0)
                    {
                        MyLogAttribute mylog = attrs[0] as MyLogAttribute;
                        Log log = new Log()
                        {
                            CreateTime = DateTime.Now,
                            Parameters = parameters,
                            Title = mylog.Title,
                            AccountName = accountname
                        };

                        DataContext db = new DataContext(applicationid);

                        using (NpgsqlConnection connection = new NpgsqlConnection(db.ConnectionString))
                        {
                            connection.Open();

                            NpgsqlCommand command = new NpgsqlCommand();
                            command.Connection = connection;

                            command.CommandText = "insert into log (content) values (@content)";
                            command.Parameters.Add("content", NpgsqlTypes.NpgsqlDbType.Jsonb).Value = JsonConvert.SerializeObject(log).ToLower();
                            command.ExecuteNonQuery();

                            connection.Close();
                        }

                        //todo:删除3个月前的日志
                    }

                    //执行方法
                    object returnval = method.Invoke(instance, new object[] { parameters });

                    if (returnval != null)
                    {
                        result = returnval;
                    }

                    break;
                }
            }

            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            return JsonConvert.SerializeObject(result, Formatting.None, jSetting);
        }
    }

    public class PluginContext
    {
        public Account Account { get; set; }
         
        public static PluginContext Current { get; set; }
    }
}