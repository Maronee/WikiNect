using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphspace
{
    public class WikiVertex
    {
        private string _bild;
        public string bild { 
            get { return _bild; }
            set { _bild = value; }
        }
        private string _text;
        public string text{
            get { return _text; }
            set { _text = value;}
        }
        private string _bild_show;
        public string bild_show
        {
            get { return _bild_show; }
            set { _bild_show = value; }
        }
        private string _text_show;
        public string text_show
        {
            get { return _text_show; }
            set { _text_show = value; }
        }
        private string _color;
        public string color
        {
            get { return _color; }
            set { _color = value; }
        }

        public WikiVertex(string bild, string bild_show, string text, string text_show)
        {
            this.bild = bild;
            this.text = text;
            this.bild_show = bild_show;
            this.text_show = text_show;
        }
    }
}
