using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Kinect
{
    class ClosingLabel : Label
    {
        public ClosingLabel()
        {
            SetIsPressTarget = true;
            SetIsGripTarget = true;
            MousePointerHandler();
        }


        private bool _isPressTarget = false;

        private bool _isGripTarget = false;

        bool isGripinInteraction = false;

        private Point currentHandPointerPosition;

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
            this.MouseDown += MyMouseDown;
        }


        public void MyMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        public void MyMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        public void MyMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
           
        }

        public void MyMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }


   
    }
}
