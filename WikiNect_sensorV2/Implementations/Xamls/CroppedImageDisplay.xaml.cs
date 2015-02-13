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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kinect;
using WikiNectLayout.Implementions.Model;

namespace WikiNectLayout.Implementions.Xamls
{
    /// <summary>
    /// Interaction logic for CroppedImageDisplay.xaml
    /// </summary>
    public partial class CroppedImageDisplay : UserControl
    {
        public CroppedImageDisplay(KinImage imageDisplay)
        {
            InitializeComponent();
            crpImageDis.Source = imageDisplay.Source;
        }
        public CroppedImageDisplay(ModelImage imageDisplay)
        {
            InitializeComponent();
            crpImageDis.Source = imageDisplay.imagesource;
        }

        private void crpImageDis_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);    

        }

        private void crpImageDis_kinect(object sender)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);   
        }

        private void PressableWithoutKinoogle_HandPointerTapped(object sender, Microsoft.Kinect.Input.KinectTappedEventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }
    }
}
