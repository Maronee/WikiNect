using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using DataStore;


namespace Kinect
{
    class SegmentationButton_old : KinectTileButton
    {

        public bool _isGripTarget = false;
        public bool _isPressTarget = false;
        private Point currentHandPointerPosition;
        private bool isGripinInteraction = false;


        /// <summary>
        /// IsHandPointerOver dependency property for use in the control template triggers
        /// </summary>
        public static readonly DependencyProperty IsHandPointerOverProperty = DependencyProperty.Register(
            "IsHandPointerOver", typeof(bool), typeof(SegmentationButton), new PropertyMetadata(false));

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

        public SegmentationButton_old()
        {
            this.SetIsGripTarget = true;
            this.SetIsPressTarget = true;
        }


        public bool SetIsGripTarget
        {
            get
            {
                return _isGripTarget;
            }
            set
            {
                _isGripTarget = value;
            }
        }

        public bool SetIsPressTarget
        {
            get
            {
                return _isPressTarget;
            }
            set
            {
                _isPressTarget = value;
            }
        }

       
    }
}
