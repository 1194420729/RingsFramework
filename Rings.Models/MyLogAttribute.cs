using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rings.Models
{
    public class MyLogAttribute:Attribute
    {
        private string title = "";
        
        public MyLogAttribute(string title)
        {
            this.title = title;
        }

        public string Title { get { return this.title; } }
    }
}
