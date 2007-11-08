using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class DeinterlaceFilter
    {
        private string script;
        private string title;
        public DeinterlaceFilter(string title, string script)
        {
            this.title = title;
            this.script = script;
        }
        public override string ToString()
        {
            return this.title;
        }

        public string Script
        {
            get { return script; }
            set { script = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }
}
