using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Kinect;
using System.Windows;
using Microsoft.Kinect.Input;
using Microsoft.Kinect.Wpf.Controls;
using Microsoft.Kinect.Toolkit.Input;

namespace Kinect
{

    public delegate void ChangedEventHandler(object sender, EventArgs e);
    public delegate void GrippedEventHandler(object sender, EventArgs e);
    public delegate void MouseRightButtonEventHandler(object sender, EventArgs e);

    // Eventhandler with HandPointerEventArgs is needed to be called from SegmentatinMainWindow
    //public delegate void ImageGrippedEventHandler(object sender, HandPointerEventArgs e);
    //public delegate void ImagePressedEventHandler(object sender, HandPointerEventArgs e);

    public class KinImage : Image, IKinectControl

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


        public bool IsManipulatable
        {
            get { return true; }
        }

        public bool IsPressable
        {
            get { return false; }
        }
        public IKinectController CreateController(IInputModel inputModel, KinectRegion kinectRegion)
        {
            KinectInteractionController kc = new KinectInteractionController(inputModel, kinectRegion);
            kc.ManipulatableInputModel.ManipulationStarted += ManipulatableInputModel_ManipulationStarted;
            kc.ManipulatableInputModel.ManipulationUpdated += ManipulatableInputModel_ManipulationUpdated;
            kc.ManipulatableInputModel.ManipulationCompleted += ManipulatableInputModel_ManipulationCompleted;
            return kc;
        }

        void ManipulatableInputModel_ManipulationUpdated(object sender, Microsoft.Kinect.Input.KinectManipulationUpdatedEventArgs e)
        {
            this.GripUpdate += Gripable_GripUpdate;
            onGripUpdate(sender, e);
        }

        void Gripable_GripUpdate(object sender, KinectManipulationUpdatedEventArgs e)
        {

        }

        void ManipulatableInputModel_ManipulationStarted(object sender, Microsoft.Kinect.Input.KinectManipulationStartedEventArgs e)
        {
            this.GripStart += Gripable_GripStart;
            onGripStart(sender, e);
        }

        void Gripable_GripStart(object sender, KinectManipulationStartedEventArgs e)
        {

        }

        void ManipulatableInputModel_ManipulationCompleted(object sender, Microsoft.Kinect.Input.KinectManipulationCompletedEventArgs e)
        {
            this.GripComplete += Gripable_GripComplete;
            onGripComplete(sender, e);
        }

        void Gripable_GripComplete(object sender, KinectManipulationCompletedEventArgs e)
        {

        }

        public delegate void GripStartHandler(object sender, KinectManipulationStartedEventArgs e);

        public event GripStartHandler GripStart;
        public void onGripStart(object sender, KinectManipulationStartedEventArgs e)
        {
            GripStart(sender, e);
        }

        public delegate void GripUpdateHandler(object sender, KinectManipulationUpdatedEventArgs e);

        public event GripUpdateHandler GripUpdate;

        public void onGripUpdate(object sender, KinectManipulationUpdatedEventArgs e)
        {
            GripUpdate(sender, e);
        }

        public delegate void GripCompleteHandler(object sender, KinectManipulationCompletedEventArgs e);

        public event GripCompleteHandler GripComplete;
        public void onGripComplete(object sender, KinectManipulationCompletedEventArgs e)
        {
            GripComplete(sender, e);
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
