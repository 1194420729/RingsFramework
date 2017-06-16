using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rings.Models
{
    [Serializable]
    public class Account
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ApplicationId { get; set; }
        public string Language { get; set; }
        public string Limit { get; set; }
        public string ZtName { get; set; }
        public bool IsDefaultZt { get; set; }
    }

    [Serializable]
    public class PrintTemplate
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public decimal Padding { get; set; }
        public bool? RepeatHeader { get; set; }
        public int? FixedLines { get; set; }
        public int? MaxLines { get; set; }
    }

    [Serializable]
    public class PrintData
    {
        public List<string> HeaderField { get; set; }
        public List<string> DetailField { get; set; }

        public Dictionary<string, string> HeaderValue { get; set; }
        public List<Dictionary<string, string>> DetailValue { get; set; }

    }

    [Serializable]
    public class Log
    {
        public string Title { get; set; }
        public string Parameters { get; set; }
        public DateTime CreateTime { get; set; }
        public string AccountName { get; set; }
    }

    [Serializable]
    public class Limit
    {
        public string Name { get; set; }
        public string Title { get; set; } 
        public string GroupName { get; set; }
    }

    [Serializable]
    public class Menu
    {
        public int Sort { get; set; }
        public string Path { get; set; }
        public string Html { get; set; }
        public string Group { get; set; }
        public int GroupSort { get; set; }
        public string GroupIcon { get; set; }
    }

    [Serializable]
    public class AttachmentInfo
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }        
        public int Size { get; set; }
    }
     
}
