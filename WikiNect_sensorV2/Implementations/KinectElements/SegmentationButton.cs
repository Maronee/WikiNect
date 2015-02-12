using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using DataStore;


namespace Kinect
{
    class SegmentationButton : Button
    {

        public bool _isGripTarget = false;
        public bool _isPressTarget = false;


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

        public SegmentationButton()
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

       

        public void MousePointerHandler()
        {
            
        }

        public void PressTargetEnable()
        {
            
        }

        
        public void MyMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        public void MyMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        public void MyMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.IsHandPointerOver = false;
        }

        public void MyMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.IsHandPointerOver = true;
        }
    }
}
