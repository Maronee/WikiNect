using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;

namespace WikiNectLayout
{
    /// <summary>
    /// Interaction logic for WikiNectApp.xaml
    /// </summary>
    public partial class WikiNectApp : Application
    {
        internal KinectSensor kinectSensor { get; set; }
        internal KinectRegion kinectRegion { get; set; }
        public WikiNectApp()
        {
            //WikiNect_new wikinect = new WikiNect_new();   
        }

    }
}
