using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;

namespace Graphspace
{
    public class WikiGraph : BidirectionalGraph<WikiVertex, WikiEdge>
    {
        public WikiGraph() { }

        public WikiGraph(bool allowParallelEdges)
            : base(allowParallelEdges) { }

        public WikiGraph(bool allowParallelEdges, int vertexCapacity)
            : base(allowParallelEdges, vertexCapacity) { }
    }
}

