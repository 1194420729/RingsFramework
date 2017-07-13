using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using Npgsql;
using Newtonsoft.Json;
using CsQuery;

namespace Rings.Models
{
    public class PrintManager
    {
        //todo:定期清理printdata

        private string applicationid = null;
        public PrintManager(string applicationid)
        {
            this.applicationid = applicationid;
        }

        public int RegisterPrintModel(PrintData model)
        {
            int id = 0;
            DataContext db = new DataContext(this.applicationid);

            using (NpgsqlConnection connection = new NpgsqlConnection(db.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;

                command.CommandText = "insert into printdata (content,createtime) values (@content,now()) returning id";
                command.Parameters.Add("content", NpgsqlTypes.NpgsqlDbType.Jsonb).Value = JsonConvert.SerializeObject(model);

                id = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }

            return id;
        }


        public int RegisterPrintModel(CreatePrintData creator)
        {
            PrintData model = creator();

            return RegisterPrintModel(model);
        }


        public PrintData GetPrintModel(int id)
        {
            PrintData pd = null;

            DataContext db = new DataContext(this.applicationid);

            using (NpgsqlConnection connection = new NpgsqlConnection(db.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;

                command.CommandText = "select content from printdata where id=" + id;
                string json = command.ExecuteScalar().ToString();
                connection.Close();

                pd = JsonConvert.DeserializeObject<PrintData>(json);
            }

            return pd;
        }

        public string RenderPrintTemplate(int templateid, PrintData model)
        {
            //获取模板
            PrintTemplate template = null;

            DataContext db = new DataContext(this.applicationid);
            using (NpgsqlConnection connection = new NpgsqlConnection(db.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;

                command.CommandText = "select content from printtemplate where id=" + templateid;
                string json = command.ExecuteScalar().ToString();

                template = JsonConvert.DeserializeObject<PrintTemplate>(json);
                connection.Close();
            }

            //补齐空行
            if (template.FixedLines.HasValue)
            {
                int blanklines = model.DetailValue.Count % template.FixedLines.Value;
                if (blanklines > 0) blanklines = template.FixedLines.Value - blanklines;
                for (int i = 0; i < blanklines; i++)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (string f in model.DetailField)
                    {
                        dic.Add(f, string.Empty);
                    }
                    model.DetailValue.Add(dic);
                }

            }

            //计算分页
            int lines = template.FixedLines.HasValue ? template.FixedLines.Value : template.MaxLines.Value;
            int pages = model.DetailValue.Count / lines + ((model.DetailValue.Count % lines) > 0 ? 1 : 0);
            if (pages == 0) pages = 1;

            //每页渲染
            StringBuilder sb = new StringBuilder();
            CQ doc = CQ.CreateFragment(template.Content);
            CQ header = doc.Select("#pageheader");
            CQ detail = doc.Select("#pagedetail");
            CQ footer = doc.Select("#pagefooter");
            CQ pagenumber = doc.Select("#pagenumber");
            for (int i = 0; i < pages; i++)
            {
                if (header != null)
                {
                    if (i == 0 || (template.RepeatHeader.HasValue && template.RepeatHeader.Value == true))
                    {
                        //表头                    
                        string headerhtml = HttpUtility.HtmlDecode(header.RenderSelection());
                        sb.Append(RenderHeaderFooter(headerhtml, model));
                    }
                }

                //明细
                if (detail != null)
                {
                    PrintData modeldetail = new PrintData();
                    modeldetail.DetailField = model.DetailField;
                    modeldetail.DetailValue = model.DetailValue.Skip(i * lines).Take(lines).ToList();
                    string detailhtml = HttpUtility.HtmlDecode(detail.RenderSelection());
                    sb.Append(RenderHeaderFooter(RenderDetail(detailhtml, modeldetail), model));
                }

                if (footer != null)
                {
                    if (i == pages - 1 || (template.RepeatHeader.HasValue && template.RepeatHeader.Value == true))
                    {
                        //表尾
                        string footerhtml = HttpUtility.HtmlDecode(footer.RenderSelection());
                        sb.Append(RenderHeaderFooter(footerhtml, model));
                    }
                }

                //分页符及页码
                if (pagenumber != null)
                {
                    if (pagenumber.Length > 0)
                    {
                        string pg = HttpUtility.HtmlDecode(pagenumber.RenderSelection());
                        //sb.AppendFormat("<center><div class=\"pagenumber\">{0}</div></center>", i + 1);
                        sb.Append(pg.Replace("{no}", (i + 1).ToString()));
                    }
                }

                if (i + 1 < pages)
                {
                    sb.Append("<div style=\"page-break-after:always;\"></div>");
                }
            }
             
            return sb.ToString();
        }

        private string RenderHeaderFooter(string html,PrintData model)
        {
            string s = html.Replace("\n", "").Replace("\t", "");
            Regex r = new Regex(@"\{.+?}");
            s = r.Replace(s, m => ReplaceHeaderField(m, model));

            return s;
        }

        private string RenderDetail(string html, PrintData model)
        {
            string s = html.Replace("\n", "").Replace("\t", "");
            Regex r = new Regex(@"<tr>((?!tr).)*?\[+?.*?</tr>");
            s = r.Replace(s, m => RepeatDetailField(m, model));

            return s;
        }

        private string ReplaceHeaderField(Match m, PrintData model)
        {
            foreach (var field in model.HeaderField)
            {
                if (m.Value == "{" + field + "}")
                {
                    return model.HeaderValue[field];
                }
            }

            return "";
        }

        private string ReplaceDetailField(Match m, Dictionary<string, string> detailvalues)
        {
            foreach (var field in detailvalues.Keys)
            {
                if (m.Value == "[" + field + "]")
                {
                    return detailvalues[field];
                }
            }

            return "";
        }

        private string RepeatDetailField(Match m, PrintData model)
        {
            string tr = m.Value;
            Regex r = new Regex(@"\[.+?]");
            StringBuilder sb = new StringBuilder();
            foreach (var item in model.DetailValue)
            {
                sb.Append(r.Replace(tr, c => ReplaceDetailField(c, item)));
            }

            return sb.ToString();
        }
    }

    public delegate PrintData CreatePrintData();
}