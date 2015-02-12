using Kinect;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using WikiNectLayout.Implementions.KinectElements;
using WikiNectLayout.Implementions.Model;
using WikiNectLayout.Interfaces.Kinect;



namespace WikiNectLayout.Implementions.Xamls
{
    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class ImageDisplay : UserControl, WikiNectKeyboardCaller
    {
        private ObservableCollection<ModelImage> mWorkspace = new ObservableCollection<ModelImage>();

        private ModelHeader myHeader = new ModelHeader();
        private ModelHeader MainHead;
        private ModelImage myImage;
        private KeyboardHandler keybh;

        public ImageDisplay(ModelImage image)
        {
            Initialize(image);
        }

        public ImageDisplay(ModelImage image, ObservableCollection<ModelImage> mWorkspace, ModelHeader MainHead)
        {
            Initialize(image);

            this.MainHead = MainHead;
            this.mWorkspace = mWorkspace;
            lbImageGallery.ItemsSource = this.mWorkspace;

           foreach (ModelImage item in this.mWorkspace)
            {
                item.selected = "0";
                item.visible = "Hidden";
            }

            delete.Visibility = System.Windows.Visibility.Visible;
            graph.Visibility = System.Windows.Visibility.Visible;
            imageWorkspace.Visibility = System.Windows.Visibility.Visible;
        }

        private void Initialize(ModelImage image) 
        {
            InitializeComponent();

            myHeader.title = "Detailansicht";
            myHeader.subTitle = image.title;
            myImage = image;

            details.DataContext = myImage;
            Header.DataContext = myHeader;

            keybh = new KeyboardHandler();
        }

        private void btnSegmentation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var segmentationDisplay = new SegmentationDisplay(myImage.kimage);
            this.ImageDisplayContent.Visibility = System.Windows.Visibility.Collapsed;
            this.ImageDisplayWindow.Children.Add(segmentationDisplay);
        }

        private void button_show(object sender, System.Windows.RoutedEventArgs e)
        {
            CategoryButton workspacePick = (CategoryButton)sender;
            myImage.selected = "5";
            myImage.visible = "Visible";

            myImage = (ModelImage)workspacePick.DataContext;
            myHeader.subTitle = myImage.title;
            details.DataContext = myImage;           
        }

        private void OnLoadedStoryboardCompleted(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (ModelImage item in this.mWorkspace)
            {
                item.selected = "5";
                item.visible = "Visible";
            }

            var parent = (Grid)this.Parent;
            parent.Children[0].Visibility = System.Windows.Visibility.Visible;
            parent.Children.Remove(this);
        }

        private void btnFullScreen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var cid = new CroppedImageDisplay(myImage);

            this.ImageDisplayWindow.Children.Add(cid);
        }

        private void btnDeleteFromWorkspace_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (mWorkspace.Count > 1)
            {
                ModelImage nextImage;
                if (mWorkspace.Last() == myImage)
                {
                    nextImage = mWorkspace[mWorkspace.IndexOf(myImage) - 1];
                }
                else 
                {
                    nextImage = mWorkspace[mWorkspace.IndexOf(myImage) + 1];
                }
                myImage.selected = "0";
                myImage.visible = "Hidden";

                this.mWorkspace.Remove(myImage);
                myImage = nextImage;

                myHeader.subTitle = myImage.title;
                details.DataContext = myImage;
            }
            else
            {
                myImage.selected = "0";
                myImage.visible = "Hidden";
                this.mWorkspace.Remove(myImage);

                var parent = (Grid)this.Parent;
                parent.Children[0].Visibility = System.Windows.Visibility.Visible;
                parent.Children.Remove(this);
            }

            MainHead.workspaceTrigger = mWorkspace.Count;
        }

        private void CropPanelRefreshBtnOnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (ModelImage item in this.mWorkspace)
            {
                item.selected = "0";
                item.visible = "Hidden";
            }
            mWorkspace.Clear();
            MainHead.workspaceTrigger = mWorkspace.Count;

            var parent = (Grid)this.Parent;
            parent.Children[0].Visibility = System.Windows.Visibility.Visible;
            parent.Children.Remove(this);
        }

        private void btnGraph_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var graphDisplay = new GraphDisplay(this.mWorkspace);
            this.ImageDisplayContent.Visibility = System.Windows.Visibility.Collapsed;
            this.ImageDisplayWindow.Children.Add(graphDisplay);
        }

        private void artistButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.keybh.showKeyboard("", this);
        }

        public void closeWithNewText(String newText)
        {

        }

        public void closeWithoutNewText()
        {

        }
    }
}
