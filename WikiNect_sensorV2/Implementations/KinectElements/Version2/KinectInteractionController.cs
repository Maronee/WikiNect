using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Microsoft.Kinect.Wpf.Controls;
using Microsoft.Kinect.Toolkit.Input;
using System.Windows;

namespace Kinect
{
    class KinectInteractionController: IKinectController,IKinectManipulatableController,IKinectPressableController
    {
        ManipulatableModel _manipModel;
        PressableModel _pressModel;
        FrameworkElement _element;

        public KinectInteractionController(IInputModel inputModel, KinectRegion kinectRegion)
        {            
            _element = inputModel.Element as FrameworkElement;
            _pressModel = inputModel as PressableModel;
            _manipModel = inputModel as ManipulatableModel;
        }

        public ManipulatableModel ManipulatableInputModel
        {
            get { return _manipModel; }
        }

        public PressableModel PressableInputModel
        {
            get { return _pressModel; }
        }

        public FrameworkElement Element
        {
            get { return _element; }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _manipModel = null;
                _pressModel = null;
                _element = null;                
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        
    }
}
