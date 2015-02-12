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
using System.Windows.Shapes;
using System.Drawing;

using Kinect;
using DataConnection;
using DataStore;
using WikiNectLayout.Implementions.Xamls;


namespace WikiNectLayout
{
    /// <summary>
    /// Interaktionslogik für WikiNect.xaml
    /// </summary>
    public partial class WikiNect : Window
    {
        private Connection connection = null;
        private List<Category> categories = new List<Category>();
        private List<String> imgUrl = new List<String>();

        public WikiNect()
        {
            init();
            
            InitializeComponent();
            //buildDummyInterface();
            categoriegroup.Children.Clear();
            categoryPanel.Children.Clear();
            workspacegroup.Children.Clear();
            setCategoryElements();
        }

        /// <summary>
        /// Builds a dummy Interface with pictures. Good for testing without an internet connection
        /// </summary>
        private void buildDummyInterface()
        {
            categoriegroup.Children.Clear();
            workspacegroup.Children.Clear();
            categoryPanel.Children.Clear();
            Style imagestyle = this.FindResource("HoverImageStyle") as Style;
            var results = getImages("Hurz");
            foreach (KinImage image in results)
            {
                BitmapImage imagesource = new BitmapImage(new Uri(image.url, UriKind.Relative));
                image.Source = imagesource;
                image.Changed += new ChangedEventHandler(item_Changed);
                image.Gripped += new GrippedEventHandler(gripped_Changed);
                image.Style = imagestyle;
                categoriegroup.Children.Add(image);
            }
        }

        public Connection getConnection()
        {
            return this.connection;
        }

        /// <summary>
        /// Creates a List of Dummy KinImage Objects. No need anymore, just for testing purpose.
        /// </summary>
        /// <param name="category">Dummy string, no use for it</param>
        /// <returns>Enumarable of KinImage Objects</returns>
        private IEnumerable<KinImage> getImages(string category)
        {
            return new[]
            {
                new KinImage
                {
                    title = "Koala",
                    url= "icons/Koala.jpg",
                    artist = "Peter",
                    year = "1234",
                    museum = "Städel"
                },
                new KinImage
                {
                    title = "Penguins",
                    url= "icons/Penguins.jpg",
                    artist = "Klaus",
                    year = "4444",
                    museum = "Schirn"
                },
                new KinImage
                {
                    title = "WikiNect",
                    url= "icons/WikiNect.png",
                    artist = "Kim",
                    year = "5555",
                    museum = "Liebig"
                }
            };
        }

        private void init()
        {
            // Init Connection --> JSONConnection
            this.connection = new JSONConnection("http://mw1.wikinect.hucompute.org/");
            //this.connection = new JSONConnection("http://de.wikipedia.org/w/");

            //this.connection = new DataBase

            this.categories = this.connection.getCategories();
        }

        //public override String ToString()
        //{
        //    String s = "";

        //    s += "Categories\n";
        //    //foreach (Category c in this.categories)
        //    for (int i = 1; i <= 1; i++)
        //    {
        //        Category c = this.categories[i];
        //        s += c.ToString() + "\n";
        //        s += "Elements:\n";
        //        foreach (DataElement e in this.connection.getElements(c))
        //        {
        //            s += e.ToString() + "\n";
        //        }
        //        s += "----------------------\n";
        //        s += "Categories:\n";
        //        foreach (Category c2 in this.connection.getSubCategories(c))
        //        {
        //            s += c2.ToString() + "\n";
        //        }
        //        s += "----------------------\n";

        //    }
        //    s += "========================\n";

        //    //foreach (Attribut a in this.connection.getAttributs(new CategoryImpl(this.connection, new Uri("http://nouri/"), "Das Abendmahl", "")))
        //    //{
        //    //    s += a.ToString() + "\n";
        //    //}
        //    //s += "----------------------\n";
        //    //s += string.Join("\n", this.connection.getSuperCategories("Psychologe"));
        //    //foreach (Category c3 in this.connection.getCategories("Psychologe"))
        //    //{
        //    //s += c3.ToString() + "\n";
        //    //}
        //    //s += "\n++++++++++++++++\n";

        //    //s += (this.connection.getCategory((long)3797903)).ToString() + "\n";
        //    //DataElement newElement = (this.connection.getElements((this.connection.getCategories())[0]))[0];
        //    //newElement.setName("TestNr1");

        //    //this.connection.addNewElement(newElement);

        //    //s += "Neues Element hinzugefuegt: \n";
        //    //s += newElement.ToString();

        //    return s;

        //}

        public void windowLoaded(object sender, RoutedEventArgs e)
        {
           

           // this.connection.getRatings();
        }

        public void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
              
        public void setCategoryElements()
        {
            Style style = this.FindResource("HoverLabelStyle") as Style; ;
            foreach (Category item in categories)
            {
                //if (item.getName() == "Kirchengemaelde")
                //{
                //    continue;
                //}
                String s = "";
                s += item.getName();
                /*s += " (";

                int count = 0;
                /*foreach (DataElement element in connection.getElements(item))
                {
                    count++;
                }s
                s += count;
                s += ") ";
                Console.WriteLine("{0}\n ==========", count);*/
                CategoryButton btn = new CategoryButton();
                BitmapImage imagesource = new BitmapImage(new Uri("icons/WikiNect.png", UriKind.Relative));

                try
                {
                    //Console.WriteLine(this.connection.getRandomImage(item).getURL());
                    //JSONConnection.getImageFromURL(this.connection.getImages(item)[0].getURL());
                    imagesource = new BitmapImage(new Uri(this.connection.getRandomImage(item).getURL(), UriKind.Absolute));
                }
                catch (Exception)
                {
                    Console.WriteLine("Kein Bild gefunden!");
                }


                KinectTileButton button = new KinectTileButton
                {
                    Background = new ImageBrush(imagesource),
                    Height = 220,
                    Label = s
                };
                //button.Click += btn_Click;
                //btn.Content = s;
                btn.category = item;
                btn.Style = style;
                btn.Background = new ImageBrush(imagesource);
                btn.Click += btn_Click;
                btn.MouseEnter += (sender, e) => { btn.Content = s;};
                btn.MouseLeave += (sender, e) => { btn.Content = ""; };
                //categoryPanel.Children.Add(button);



                ///     New Contaienr for Button and Label(do diffrent impl later)

                StackPanel btnPanel = new StackPanel();
                btnPanel.Height = 200;
                //btnPanel.Width = 200;
                btnPanel.MinWidth = 200;
                Label titel = new Label();
                titel.Content = s;
                titel.HorizontalAlignment = HorizontalAlignment.Stretch;
                titel.VerticalAlignment = VerticalAlignment.Top;
                titel.FontSize = 22;
                titel.FontWeight = FontWeights.Bold;

                btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                btn.VerticalAlignment = VerticalAlignment.Top;
                

                btnPanel.Children.Add(btn);
                btnPanel.Children.Add(titel);
                categoryPanel.Children.Add(btnPanel);
            }
        }

        

        /// <summary>
        /// This method is activated on a CategoryButton click. It gets the Picture of the selected Categorie and put them into 
        /// the categoriegroup StackPanel.
        /// </summary>
        /// <param name="sender">CategoryButton</param>
        /// <param name="e"></param>
        void btn_Click(object sender, RoutedEventArgs e)
        {
            Style imagestyle = this.FindResource("HoverImageStyle") as Style;       //Finds the style for the KinImage elements
            categoriegroup.Children.Clear();                                        //Delete the images from the last category
            CategoryButton cbtn = (CategoryButton)sender;                           //Cast the sender Object to the CategoryButton
            Category c = cbtn.category;                                             //Get the Category out of the CategoryButton
            
            //BitmapImage imagesource = new BitmapImage(new Uri("icons/WikiNect.png", UriKind.Relative));
            //foreach (DataElement element in this.connection.getElements(c))
            //{
            //    Console.WriteLine(element.getName());
            //    KinImage image = new KinImage
            //    {
            //        title = element.getName(),
            //        artist = "Peter",
            //        year = 1234,
            //        museum = "Städel",
            //    };
            //    image.Source = imagesource;
            //    image.Style = imagestyle;
            //    image.Changed += new ChangedEventHandler(item_Changed);
            //    categoriegroup.Children.Add(image);
            //}
            foreach (DataStore.Image picture in this.connection.getImages(c))
            {
                BitmapImage imagesource = new BitmapImage(new Uri(picture.getURL(), UriKind.Absolute));
                KinImage image = new KinImage
                {
                    title = picture.getInfo("ÜBERSCHRIFT"),
                    artist = picture.getInfo("KÜNSTLER"),
                    year = picture.getInfo("JAHR"),
                    museum = picture.getInfo("MUSEUM"),
                    url = picture.getURL()
                };
                image.Source = imagesource;
                image.Style = imagestyle;
                image.Changed += new ChangedEventHandler(item_Changed);
                image.Gripped += new GrippedEventHandler(gripped_Changed);
                categoriegroup.Children.Add(image);
            }
        }


        /// <summary>
        /// Grip on an Image in the categoriegroup and this method will put it into the workspace StackPanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        


        void gripped_Changed(object sender, EventArgs e)
        {
            KinImage klaus = (KinImage)sender;
            // Workaroud to avoid the InvalidOperationException error
            // TODO fix it 
            BitmapImage imagesource = new BitmapImage(new Uri(klaus.url, UriKind.RelativeOrAbsolute));
            KinImage image = new KinImage();
            Style imagestyle = this.FindResource("HoverImageStyle") as Style;
            image.Source = imagesource;
            image.Style = imagestyle;
            image.url = klaus.url;
            image.Changed += new ChangedEventHandler(item_Changed);
            //image.Pick += new GrippedEventHandler(WorkSpacePick);
            image.Gripped += new GrippedEventHandler(WorkSpacePick);
            image.RightClicked += new MouseRightButtonEventHandler(item_Remove);
            if (!imgUrl.Contains(image.url))
            {
                imgUrl.Add(image.url);
                workspacegroup.Children.Add(image);
            }
            
        }
        void item_Remove(object sender, EventArgs e)
        {
            workspacegroup.Children.Remove((KinImage)sender);
            KinImage img = (KinImage)sender;
            imgUrl.Remove(img.url);
        }

        void item_Changed(object sender, EventArgs e)
        {
            KinImage image = (KinImage)sender;
            List<object> list = new List<object>();
            list.Add(image);
            imageInfo.ItemsSource = list;
        }

        void WorkSpacePick(object sender, EventArgs e) 
        {
            var segmentation = new SegmentationMainWindow((KinImage)sender);
            Grid.SetColumnSpan(segmentation, 2);
            Grid.SetRowSpan(segmentation, 6);
            this.partypooper.Children.Add(segmentation);
        }

            //foreach (DataElement element in connection.getElements(cbtn.category))
            //{
            //    KinectTileButton kinButton = new KinectTileButton();
            //    kinButton.Label = element.getName();
            //    //Console.WriteLine(element.getDescription());
            //    List<Link> linkliste = element.getLinks();
            //    List<Annotation> annotationsliste = element.getAnnotations();
            //    Console.WriteLine(annotationsliste.Count);
            //    Console.WriteLine(linkliste.Count);
            //    categoriegroup.Children.Add(kinButton);
            //}

            //        //foreach (DataElement link in linkliste)
            //        //{
            //        //    Console.WriteLine("Link: {0} \n",link.getName());
            //        //}
            //        //foreach (DataElement annotation in element.getAnnotations())
            //        //{
            //        //    Console.WriteLine("Annotation: {0} \n", annotation.getName());
            //        //}


            //    //kinButton.Content = element.getDescription();
            //    //foreach (Annotation link in this.connection.getLinks(element))
            //    //{
            //    //    Console.WriteLine(link.ToString());
            //    //}
            
        //}

    }
}
