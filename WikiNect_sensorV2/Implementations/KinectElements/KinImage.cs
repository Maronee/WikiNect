using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Kinect;
using System.Windows;

namespace Kinect
{

    public delegate void ChangedEventHandler(object sender, EventArgs e);
    public delegate void GrippedEventHandler(object sender, EventArgs e);
    public delegate void MouseRightButtonEventHandler(object sender, EventArgs e);

    // Eventhandler with HandPointerEventArgs is needed to be called from SegmentatinMainWindow
    //public delegate void ImageGrippedEventHandler(object sender, HandPointerEventArgs e);
    //public delegate void ImagePressedEventHandler(object sender, HandPointerEventArgs e);

    public class KinImage : Image

    {
        public event ChangedEventHandler Changed;
        public event GrippedEventHandler Gripped, Pick;
        public event MouseRightButtonEventHandler RightClicked;

        //Eventhandler for Segmentation
        //public event ImageGrippedEventHandler ImageGripped;
        //public event ImagePressedEventHandler ImagePressed;

        private bool isGripinInteraction = false;
        public string year { get; set; }
        public string artist { get; set; }
        public string museum { get; set; }
        public string title { get; set; }
        public string url { get; set; }

        public KinImage()
        {
            MousePointerHandler();
        }

        /// <summary>
        /// IsHandPointerOver dependency property for use in the control template triggers
        /// </summary>
        public static readonly DependencyProperty IsHandPointerOverProperty = DependencyProperty.Register(
            "IsHandPointerOver", typeof(bool), typeof(KinImage), new PropertyMetadata(false));

        public bool IsHandPointerOver
        {
            get
            {
                return (bool)this.GetValue(IsHandPointerOverProperty);
            }

            set
            {
                this.SetValue(IsHandPointerOverProperty, value);
            }
        }

        public void MousePointerHandler()
        {
            this.MouseLeftButtonDown += MyMouseDown;
            this.MouseRightButtonDown += MyMouseRightDown;
            //this.MouseUp += MyMouseUp;
            this.MouseEnter += MyMouseEnter;
            this.MouseLeave += MyMouseLeave;
        }

        void MyMouseRightDown(object sender, MouseButtonEventArgs e)
        {
            if (RightClicked != null)
            {
                RightClicked(this, e);
            }
        }

        public void MyMouseLeave(object sender, MouseEventArgs e)
        {
            this.IsHandPointerOver = false;
        }

        public void MyMouseEnter(object sender, MouseEventArgs e)
        {
            if (Changed != null)
            {
                this.IsHandPointerOver = true;
                Changed(this, e);
            }
        }

        public void MyMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Gripped != null)
            {
                Gripped(this, e);
            }
        }

        public void MyMouseUp(object sender, MouseButtonEventArgs e)
        {
        }


        
        
    }
}
