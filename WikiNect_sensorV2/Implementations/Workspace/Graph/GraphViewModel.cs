using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GraphSharp.Controls;
using WikiNectLayout.Implementions.Model;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Graphspace
{

    public class WikiGraphLayout : GraphLayout<WikiVertex, WikiEdge, WikiGraph>
    {
    }

    class GraphViewModel : INotifyPropertyChanged
    {
        
        private string layoutAlgorithmType;
        private WikiGraph graph;
        private WikiVertex vertexAdd, center;
        private WikiEdge newEdge;

        private List<String> layoutAlgorithmTypes = new List<string>();

        //private List<String> existingVertices;
        private List<WikiVertex> graphVertices = new List<WikiVertex>();

        public GraphViewModel(ObservableCollection<ModelImage> Workspace, string[] att, string[] colors)
        {
            //colors = new string[] { "#ffffff", "#990082", "#829900", "#009964", "#003699", "#996400", "#990036", "#991700" };
            //colors = new string[] { "#ffffff", "#008299", "#00A376", "#0055A1", "#F89200", "#F85A00", "#01424E", "#032D52", "#00533C", "#7F4A00", "#7F2E00" }; // #008299 akzentfarbe und analoge

            graph = new WikiGraph(true);
            //existingVertices = new List<String>();

            foreach (ModelImage item in Workspace)
            {
                bool first = true;
                int i = 0;
                foreach (String attribut in att)
                {
                    string pic = (string)item.GetType().GetProperty(attribut).GetValue(item, null);
                    string text = pic;
                    string pic_show = "Collapsed";
                    string text_show = "Visible";

                    if (pic.EndsWith(".jpg") || pic.EndsWith(".jpeg") || pic.EndsWith(".png"))
                    {
                        text = null;
                        pic_show = "Visible";
                        text_show = "Collapsed";
                    }
                    
                    if(graphVertices == null || first)
                    {
                        vertexAdd = new WikiVertex(pic, pic_show, text, text_show);
                        vertexAdd.color = colors[0];
                        center = vertexAdd;
                        Graph.AddVertex(vertexAdd);
                        graphVertices.Add(vertexAdd);
                        first = false;
                    }
                    else
                    {
                        bool noDublicat = true;

                        foreach(WikiVertex wVertex in graphVertices)
                        {
                            if(wVertex.text == pic)
                            {
                                noDublicat = false;
                                AddNewGraphEdge(center, wVertex);
                            }
                        }

                        if (noDublicat)
                        {
                            vertexAdd = new WikiVertex(pic, pic_show, text, text_show);
                            vertexAdd.color = colors[i];
                            Graph.AddVertex(vertexAdd);
                            graphVertices.Add(vertexAdd);
                            AddNewGraphEdge(center, vertexAdd);
                        }
                    }
                    i++;
                }
            }


            
            /*
            graph = new WikiGraph(true);
            existingVertices = new List<String>();
            
            foreach (ModelImage item in Workspace)
            {                
               
                existingVertices.Add(item._url); //0
                existingVertices.Add(item._artist); //1
                existingVertices.Add( item._museum); //2
                existingVertices.Add( item._year); //3
                existingVertices.Add(item.title); //4
               
            }

           existingVertices = existingVertices.Distinct().ToList();
            
            foreach (String vertex in existingVertices)
           {
               vertexAdd = new WikiVertex(vertex, vertex);
               Graph.AddVertex(vertexAdd);
               graphVertices.Add(vertexAdd);
           }





            foreach (ModelImage item in Workspace)
            {
                 
                foreach (WikiVertex vertex in graphVertices)
                {
                    vertex1 = vertex;
                    if (vertex1.bild == item._url) { 
                    foreach (WikiVertex vertex3 in graphVertices) { 
                        if (vertex3.bild == item._artist || vertex3.bild == item._museum ||
                            vertex3.bild == item._year || vertex3.bild == item.title) {

                                vertex2 = vertex3;
                                AddNewGraphEdge(vertex1, vertex2);
                            }
                        }
                    }
                }     
            }  */

            //Add Layout Algorithm Types
            layoutAlgorithmTypes.Add("BoundedFR");
            layoutAlgorithmTypes.Add("Circular");
            layoutAlgorithmTypes.Add("CompoundFDP");
            layoutAlgorithmTypes.Add("EfficientSugiyama");
            layoutAlgorithmTypes.Add("FR");
            layoutAlgorithmTypes.Add("ISOM");
            layoutAlgorithmTypes.Add("KK");
            layoutAlgorithmTypes.Add("LinLog");
            layoutAlgorithmTypes.Add("Tree");

            //Pick a default Layout Algorithm Type
            LayoutAlgorithmType = "LinLog";
        }

        private WikiEdge AddNewGraphEdge(WikiVertex from, WikiVertex to)
        {
           // newEdge = new WikiEdge("test", from, to);
            newEdge = new WikiEdge(from, to);
            Graph.AddEdge(newEdge);
            return newEdge;
        }    

        public List<String> LayoutAlgorithmTypes
        {
            get { return layoutAlgorithmTypes; }
        }

        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                NotifyPropertyChanged("LayoutAlgorithmType");
            }
        }

        public WikiGraph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                NotifyPropertyChanged("Graph");
            }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
