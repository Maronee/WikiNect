using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Kinect;
using System.Collections.ObjectModel;
using WikiNectLayout.Implementions.Model;
using QuickGraph;
using GraphSharp.Controls;
using GraphSharp;
using Graphspace;



namespace WikiNectLayout.Implementions.Xamls
{
    /// <summary>
    /// Interaction logic for GraphDisplay.xaml
    /// </summary>
    public partial class GraphDisplay : UserControl
    {
        private GraphViewModel vm;
        private List<ModelLegende> mLegend = new List<ModelLegende>();
        private ModelHeader myHeader = new ModelHeader();



        public GraphDisplay(ObservableCollection<ModelImage> mWorkspace)
        {
            string[] colors = new string[] { "#ffffff", "#008299", "#00A376", "#0055A1", "#F89200", "#F85A00", "#01424E", "#032D52", "#00533C", "#7F4A00", "#7F2E00" }; // #008299 akzentfarbe und analoge
            string[] attribut = { "url", "artist", "museum", "year", "title" };

           
            

            vm = new GraphViewModel(mWorkspace, attribut, colors);

            this.DataContext = vm;

            InitializeComponent(); 
            myHeader.title = "Graphansicht";
            Header.DataContext = myHeader;

            //Legende erstellen
            for (int i = 0; i < attribut.Length; i++ )
            {
                ModelLegende legend = new ModelLegende();

                legend.color = colors[i];
                legend.title = attribut[i];

                mLegend.Add(legend);
            }
            
            legende.ItemsSource = mLegend;
        }

        private void OnLoadedStoryboardCompleted(object sender, System.Windows.RoutedEventArgs e)
        {   
            var parent = (Grid)this.Parent;
            parent.Children[0].Visibility = System.Windows.Visibility.Visible;
            parent.Children.Remove(this);
        }   
    }
}
