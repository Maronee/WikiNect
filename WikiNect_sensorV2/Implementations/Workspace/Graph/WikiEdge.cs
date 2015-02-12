using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using System.ComponentModel;
using System.Diagnostics;

namespace Graphspace
{
    public class WikiEdge : Edge<WikiVertex>
    {
        private string _Tip_ID;
        public string Tip_ID
        {
            get { return _Tip_ID; }
            set { _Tip_ID = value; }
        }
        public WikiEdge(WikiVertex source, WikiVertex target)
            : base(source, target)
        {
        }

        public WikiEdge(string tip_id, WikiVertex source, WikiVertex target)
            : base(source, target)
        {
            Tip_ID = tip_id;
        }

    }
}

