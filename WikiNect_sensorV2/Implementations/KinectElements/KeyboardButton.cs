using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using DataStore;
using System.Windows.Media;


namespace Kinect
{
    class KeyboardButton : Button
    {

        public bool _isGripTarget = false;
        public bool _isPressTarget = false;
        private bool isGripinInteraction = false;
        private Point currentHandPointerPosition;
        private bool alternatable;
        private String actionOnClick;
        private String myContent;
        private String myAlternateContent;


        /// <summary>
        /// IsHandPointerOver dependency property for use in the control template triggers
        /// </summary>
        public static readonly DependencyProperty IsHandPointerOverProperty = DependencyProperty.Register(
            "IsHandPointerOver", typeof(bool), typeof(KeyboardButton), new PropertyMetadata(false));

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

        public KeyboardButton()
        {
            this.SetIsGripTarget = true;
            this.SetIsPressTarget = true;
        }

        public KeyboardButton(int width, int height, String content, String alternateContent, int margin, String actionClick)
        {
            myContent = content;
            myAlternateContent = alternateContent;
            alternatable = true;

            LinearGradientBrush buttonBrush = new LinearGradientBrush();
            buttonBrush.StartPoint = new Point(0.5, 0);
            buttonBrush.EndPoint = new Point(0.5, 1);
            GradientStop firstGS = new GradientStop();
            firstGS.Color = (Color)ColorConverter.ConvertFromString("#FF8F8F8F");
            firstGS.Offset = 0.0;
            buttonBrush.GradientStops.Add(firstGS);
            GradientStop sndGS = new GradientStop();
            sndGS.Color = (Color)ColorConverter.ConvertFromString("#FFBFBFBF");
            sndGS.Offset = 1;
            buttonBrush.GradientStops.Add(sndGS);

            this.Width = width;
            this.Height = height;
            this.Content = myContent;
            this.Margin = new Thickness(margin, margin, margin, margin);
            this.BorderBrush = Brushes.DarkGray;
            this.Background = buttonBrush;
            this.actionOnClick = actionClick;
            this.SetIsGripTarget = true;
            this.SetIsPressTarget = true;
        }
        
        public KeyboardButton(int width, int height, String content, int margin, String actionClick)
        {
            myContent = content;
            alternatable = false;

            LinearGradientBrush buttonBrush = new LinearGradientBrush();
            buttonBrush.StartPoint = new Point(0.5, 0);
            buttonBrush.EndPoint = new Point(0.5, 1);
            GradientStop firstGS = new GradientStop();
            firstGS.Color = (Color)ColorConverter.ConvertFromString("#FF8F8F8F");
            firstGS.Offset = 0.0;
            buttonBrush.GradientStops.Add(firstGS);
            GradientStop sndGS = new GradientStop();
            sndGS.Color = (Color)ColorConverter.ConvertFromString("#FFBFBFBF");
            sndGS.Offset = 1;
            buttonBrush.GradientStops.Add(sndGS);

            this.Width = width;
            this.Height = height;
            this.Content = myContent;
            this.Margin = new Thickness(margin, margin, margin, margin);
            this.BorderBrush = Brushes.DarkGray;
            this.Background = buttonBrush;
            this.actionOnClick = actionClick;
            this.SetIsGripTarget = true;
            this.SetIsPressTarget = true;
        }

        public void changeToAlternateContent()
        {
            if (this.alternatable) { this.Content = myAlternateContent; }
        }

        public void restoreContent()
        {
            this.Content = myContent;
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
