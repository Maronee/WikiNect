using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using DataStore;
using Kinect;


namespace Kinect
{
    class CategoryButton : KinectTileButton
    {
        public Category category;

        /// <summary>
        /// IsHandPointerOver dependency property for use in the control template triggers
        /// </summary>
        public static readonly DependencyProperty IsHandPointerOverProperty = DependencyProperty.Register(
            "IsHandPointerOver", typeof(bool), typeof(CategoryButton), new PropertyMetadata(false));

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

        public CategoryButton()
        {
            
        }
    }
}
