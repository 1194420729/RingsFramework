using CsQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Rings.Models
{
    public class MenuLoader
    {
        private string applicationid;
        private string language;
        private Account account;
        public MenuLoader(Account account)
        {
            this.applicationid = account.ApplicationId;
            this.language = account.Language;
            this.account = account;
        }

        public string RenderMenu()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //当前用户自定义目录下的指定语言目录
            string path = HttpContext.Current.Server.MapPath("~/Views/" + this.applicationid + "/" + this.language);
            string[] files = new string[] { };
            if (Directory.Exists(path))
            {
                files = Directory.GetFiles(path, "menu.cshtml", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    string modulename = fi.DirectoryName.ToLower().Replace(path.ToLower(), "");

                    dic.Add(modulename, file);
                }
            }

            //当前用户自定义目录下的默认语言目录
            path = HttpContext.Current.Server.MapPath("~/Views/" + this.applicationid);
            if (Directory.Exists(path))
            {
                string[] files2 = Directory.GetFiles(path, "menu.cshtml", SearchOption.AllDirectories);
                foreach (string file in files2)
                {
                    if (files.Contains(file)) continue;

                    FileInfo fi = new FileInfo(file);
                    string modulename = fi.DirectoryName.ToLower().Replace(path.ToLower(), "");

                    if (dic.ContainsKey(modulename) == false)
                        dic.Add(modulename, file);
                }
            }

            //默认目录下的指定语言目录
            path = HttpContext.Current.Server.MapPath("~/Views/Default/" + this.language);
            string[] files3 = new string[] { };
            if (Directory.Exists(path))
            {
                files3 = Directory.GetFiles(path, "menu.cshtml", SearchOption.AllDirectories);
                foreach (string file in files3)
                {
                    FileInfo fi = new FileInfo(file);
                    string modulename = fi.DirectoryName.ToLower().Replace(path.ToLower(), "");

                    if (dic.ContainsKey(modulename) == false)
                        dic.Add(modulename, file);
                }
            }

            //默认目录下的默认语言目录
            path = HttpContext.Current.Server.MapPath("~/Views/Default");
            if (Directory.Exists(path))
            {
                string[] files4 = Directory.GetFiles(path, "menu.cshtml", SearchOption.AllDirectories);
                foreach (string file in files4)
                {
                    if (files3.Contains(file)) continue;

                    FileInfo fi = new FileInfo(file);
                    string modulename = fi.DirectoryName.ToLower().Replace(path.ToLower(), "");

                    if (dic.ContainsKey(modulename) == false)
                        dic.Add(modulename, file);
                }
            }

            //读取menu文件内容
            List<Menu> list = new List<Menu>();
            foreach (string file in dic.Values)
            {
                string html = File.ReadAllText(file, Encoding.UTF8);
                CQ frag = CQ.CreateFragment(html);

                #region 去除没有权限的
                //去除没有权限的菜单项
                CQ lis = frag.Select("li[ng-limit]");
                for (int i = 0; i < lis.Length; i++)
                {
                    string limitname = lis[i].Attributes["ng-limit"];
                    bool isallowed = this.account.IsAllowed(limitname);
                    lis[i].SetAttribute("menu-limit", isallowed.ToString().ToLower());
                    lis[i].RemoveAttribute("ng-limit");
                }

                frag = frag.Select("li[menu-limit='false']").Remove();

                //去除没有子项的父菜单
                bool hasempty = true;
                while (hasempty)
                {
                    CQ uls = frag.Select("li > ul");
                    hasempty = false;
                    for (int i = 0; i < uls.Length; i++)
                    {
                        if (uls[i].Cq().Find("li").Length == 0)
                        {
                            hasempty = true;
                            uls[i].Cq().Parent().Remove();
                        }
                    }
                }

                //如果整个菜单为空，忽略
                if (frag.Select("li").Length == 0) continue;

                #endregion

                var ul = frag.Select("ul[data-sort]");
                string sort = ul.DataRaw("sort");
                string group = ul.DataRaw("group");
                string groupsort = ul.DataRaw("groupsort");
                string groupicon = ul.DataRaw("groupicon");
                 
                Menu menu = new Menu()
                {
                    Sort = sort == null ? 0 : Convert.ToInt32(sort),
                    Group=group,
                    GroupSort = groupsort == null ? 0 : Convert.ToInt32(groupsort),
                    GroupIcon = groupicon,
                    Path = file,
                    Html = ul.Html()
                };

                list.Add(menu);
            }

            //排序后拼装Menu
            StringBuilder sb = new StringBuilder();
            
            var groups = (from c in list
                         group c by new { c.Group, c.GroupIcon, c.GroupSort } into g
                         select g.Key).OrderByDescending(c=>c.GroupSort).ToList();

            if (groups.Count == 1)
            {
                //仅有一个模块组，忽略组名称
                list = list.OrderByDescending(c => c.Sort).ToList();
                foreach (var item in list)
                {
                    sb.AppendFormat(item.Html);
                }
            }
            else
            {
                foreach (var g in groups)
                {
                    sb.Append("<li>");
                    sb.AppendFormat("<span><i class=\"{0}\"></i><span class=\"menutitle\">{1}</span></span>", g.GroupIcon, g.Group);
                    sb.Append("<ul>");
                    var sublist = list.Where(c => c.Group == g.Group).OrderByDescending(c => c.Sort).ToList();
                    foreach (var item in sublist)
                    {
                        sb.AppendFormat(item.Html);
                    }
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
            }
            return sb.ToString();

        }
    }


}
