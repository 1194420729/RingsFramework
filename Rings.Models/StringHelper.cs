using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Rings.Models
{
    public class StringHelper
    {
        public static string GetString(string s)
        {
            PluginContext context = PluginContext.Current;

            string lan=context.Account.Language;

            return GetString(s,lan);
        }

        public static string GetString(string s,string lan)
        {  
            Assembly assem = Assembly.GetCallingAssembly();

            FileInfo fileinfo = new FileInfo(assem.CodeBase.Substring(8));
            string path = Path.Combine(fileinfo.Directory.FullName, "strings.config.xml");

            if (File.Exists(path) == false)
            {
                return s;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.DocumentElement.SelectSingleNode("/languages/" + lan);
            if (node == null) return s;

            XmlNode stringnode = node.SelectSingleNode("./string[@name='" + s + "']");
            if (stringnode == null) return s;

            return stringnode.Attributes["value"].Value;
        }
    }
}
