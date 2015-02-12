using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Kinect.Wpf.Controls;
using Microsoft.Kinect.Toolkit.Input;
using Microsoft.Kinect.Input;

namespace Kinect
{
    class Gripable : UserControl, IKinectControl
    {
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
    }
}
