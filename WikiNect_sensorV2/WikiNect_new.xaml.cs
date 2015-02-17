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
using Microsoft.Kinect;
using System.Drawing;

using Kinect;
using DataConnection;
using DataStore;
//using Microsoft.Kinect.Toolkit.Controls;
using WikiNectLayout.Implementions.Xamls;
using WikiNectLayout.Implementions.Model;
using System.Collections.ObjectModel;
using WikiNectLayout.Implementions.KinectElements;


namespace WikiNectLayout
{
    /// <summary>
    /// Interaktionslogik für WikiNect.xaml
    /// </summary>
    public partial class WikiNect_new : Window
    {
        //private KinectSensorChooser sensorChooser;
        private Connection connection = null;
        private List<Category> categories = new List<Category>();
        private List<String> imgUrl = new List<String>();

        private ModelHeader mHead = new ModelHeader();
        private ObservableCollection<ModelKategorie> mKategorie = new ObservableCollection<ModelKategorie>();
        private ObservableCollection<ModelImage> mKategorieItems = new ObservableCollection<ModelImage>();
        private ObservableCollection<ModelImage> mWorkspace = new ObservableCollection<ModelImage>();

        private bool categorie = true;
        private bool select = false;

        public WikiNect_new()
        {
            init();
            
            InitializeComponent();

            page.DataContext = mHead;
            mHead.title = "WikiNect";
            
            lbImageGallery.ItemsSource = mKategorie;
            findKategorie();

        }

        public void findKategorie()
        {
            foreach (Category item in connection.getCategories())
            {
                
                ModelKategorie model = new ModelKategorie();
                model.title = item.getName();
                try
                {
                    model.imagesource = new BitmapImage(new Uri(this.connection.getRandomImage(item).getURL(), UriKind.Absolute));
                }
                catch (Exception)
                {
                    model.imagesource = new BitmapImage(new Uri("icons/WikiNect.png", UriKind.Relative));
                }
                model.categorie = item;

                this.mKategorie.Add(model);

            }
        }


        private void init()
        {
            // Init Connection --> JSONConnection
            this.connection = new JSONConnection("http://mw1.wikinect.hucompute.org/");
            //this.connection = new JSONConnection("http://de.wikipedia.org/w/");

            //this.connection = new DataBase

            //this.categories = this.connection.getCategories();
        }

        public void windowLoaded(object sender, RoutedEventArgs e)
        {
            KinectSensor sensor = KinectSensor.GetDefault();
            if (sensor != null)
            {
                sensor.Open();
                WikiNectApp app = (WikiNectApp)Application.Current;
                app.kinectRegion = kinectRegion;
                app.kinectSensor = sensor;
            }
        }

        public void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WikiNectApp app = (WikiNectApp)Application.Current;
            app.kinectSensor.Close();
            app.kinectSensor = null;
            Application.Current.Shutdown();
        }
        
        //public void sensorChooserOnKinectChanged(object sender, Microsoft.Kinect.Toolkit.KinectChangedEventArgs args)
        //{
        //    bool error = false;
        //    if (args.OldSensor != null)
        //    {
        //        try
        //        {
        //            args.OldSensor.DepthStream.Range = DepthRange.Default;
        //            args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
        //            args.OldSensor.DepthStream.Disable();
        //            args.OldSensor.SkeletonStream.Disable();
        //        }
        //        catch (InvalidOperationException) { error = true; }
        //    }

        //    if (args.NewSensor != null)
        //    {

        //        try
        //        {
        //            args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
        //            args.NewSensor.SkeletonStream.Enable();
        //            try
        //            {
        //                args.NewSensor.DepthStream.Range = DepthRange.Near;
        //                args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
        //                args.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
        //            }
        //            catch (InvalidOperationException)
        //            {
        //                args.NewSensor.DepthStream.Range = DepthRange.Default;
        //                args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
        //                error = true;
        //            }
        //        }
        //        catch (InvalidOperationException) { error = true; }
        //    }
        //    else
        //    {
        //        error = true;
        //    }
        //    if (!error)
        //    {
        //        // Bindet den Sensor an die Region
        //        //Console.WriteLine("Bin da");
        //        kinectRegion.KinectSensor = args.NewSensor;
        //    }
        //}

        private void button_show(object sender, RoutedEventArgs e)
        {
            CategoryButton cbtn = (CategoryButton)sender;

            if (select && !categorie)
            {
                ModelImage mk = (ModelImage)cbtn.DataContext;

                if (mk.selected == "0")
                {
                    mWorkspace.Add(mk);
                    mk.selected = "5";
                    mk.visible = "Visible";

                    mHead.workspaceTrigger = mWorkspace.Count;
                }
                else
                {
                    mWorkspace.Remove(mk);
                    mk.selected = "0";
                    mk.visible = "Hidden";

                    mHead.workspaceTrigger = mWorkspace.Count;
                }
            }
            else
            {
                if (categorie)
                {
                    categorie = false;

                    ModelKategorie mk = (ModelKategorie)cbtn.DataContext;

                    mKategorieItems.Clear();
                    lbImageGallery.ItemsSource = mKategorieItems;

                    Workspace_btn.Visibility = System.Windows.Visibility.Visible;
                    btnBack.Visibility = System.Windows.Visibility.Visible;
                    selectStartWorkspace.Visibility = System.Windows.Visibility.Visible;
                    //GridViewScroll.ScrollToHome();

                    mHead.title = "Kategorie";
                    mHead.subTitle = mk.title;

                    foreach (DataStore.Image picture in this.connection.getImages(mk.categorie))
                    {
                        ModelImage model = new ModelImage();
                        BitmapImage imagesource = new BitmapImage(new Uri(picture.getURL(), UriKind.Absolute));
                        model.kimage = new KinImage
                        {
                            title = picture.getInfo("ÜBERSCHRIFT"),
                            artist = picture.getInfo("KÜNSTLER"),
                            year = picture.getInfo("JAHR"),
                            museum = picture.getInfo("MUSEUM"),
                            url = picture.getURL()
                        };

                        model.title = picture.getInfo("ÜBERSCHRIFT");
                        model.artist = picture.getInfo("KÜNSTLER");
                        model.year = picture.getInfo("JAHR");
                        model.museum = picture.getInfo("MUSEUM");
                        model.url = picture.getURL();

                        model.imagesource = imagesource;

                        foreach (ModelImage item in mWorkspace) { 
                            if (item.title == model.title)
                            {
                                model.selected = "5";
                                model.visible = "Visible";
                                model=item;
                            }
                        }
                        mKategorieItems.Add(model);
                    }
                }
                else
                {
                    ModelImage mk = (ModelImage)cbtn.DataContext;
                    var imageDisplay = new ImageDisplay(mk);

                    this.page.Visibility = System.Windows.Visibility.Collapsed;
                    this.page_grid.Children.Add(imageDisplay);
                }
            }
        }

        private void backbtnClick(object sender, RoutedEventArgs e)
        {
            if (categorie == false)
            {
                categorie = true;
                
                if (mWorkspace.Count()==0)
                    Workspace_btn.Visibility = System.Windows.Visibility.Hidden;

                btnBack.Visibility = System.Windows.Visibility.Hidden;
                selectStartWorkspace.Visibility = System.Windows.Visibility.Hidden;

                mHead.title = "WikiNect";
                mHead.subTitle = null;

                lbImageGallery.ItemsSource = mKategorie;
            }
        }

        private void changeViewSelect(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            if (select)
            {
                select = false;
                btn_active.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFFFFF");
                btn_active_select.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("#000000");
            }
            else 
            {
                select = true;
                btn_active.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#008299");
                btn_active_select.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("#ffffff");
            }
        }

        private void workspaceopen(object sender, RoutedEventArgs e)
        {
            if (mWorkspace.Count != 0)
            {
                var imageDisplay = new ImageDisplay(mWorkspace[0], mWorkspace, mHead);

                this.page.Visibility = System.Windows.Visibility.Collapsed;
                this.page_grid.Children.Add(imageDisplay);
            }
        }

        private void PressableWithoutKinoogle_HandPointerTapped(object sender, Microsoft.Kinect.Input.KinectTappedEventArgs e)
        {
            PressableWithoutKinoogle p = (PressableWithoutKinoogle)sender;
            CategoryButton cbt = p.Content as CategoryButton;
            cbt.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
