using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect.Toolkit.Input;
using Microsoft.Kinect.Wpf.Controls;
using System.Windows.Controls;
using System.Windows;

namespace Kinect
{
    class Pressable : UserControl, IKinectControl
    {
        //private KinectController pc;
        public IKinectController CreateController(IInputModel inputModel, KinectRegion kinectRegion)
        {
            KinectInteractionController pc = new KinectInteractionController(inputModel, kinectRegion);
            pc.PressableInputModel.PressingStarted += PressableInputModel_PressingStarted;
            pc.PressableInputModel.PressingUpdated += PressableInputModel_PressingUpdated;
            pc.PressableInputModel.PressingCompleted += PressableInputModel_PressingCompleted;
            pc.PressableInputModel.Tapped += PressableInputModel_Tapped;            
            pc.PressableInputModel.Holding += PressableInputModel_Holding;
            //pc.ManipulatableInputModel.ManipulationCompleted += ManipulatableInputModel_ManipulationCompleted;
            //pc.ManipulatableInputModel.ManipulationStarted += ManipulatableInputModel_ManipulationStarted;
            return pc;
        }

        void PressableInputModel_Holding(object sender, Microsoft.Kinect.Input.KinectHoldingEventArgs e)
        {
            this.HandPointerHolding += Pressable_HandPointerHolding;
            HandPointerHolding(sender, e);
        }

        void PressableInputModel_PressingUpdated(object sender, Microsoft.Kinect.Input.KinectPressingUpdatedEventArgs e)
        {
            this.HandPointerHover += Pressable_HandPointerHover;
            HandPointerHover(sender, e);
        }

        void PressableInputModel_PressingStarted(object sender, Microsoft.Kinect.Input.KinectPressingStartedEventArgs e)
        {

            this.HandPointerEnter += Pressable_HandPointerEnter;
            HandPointerEnter(sender, e);
              
        }

        void PressableInputModel_PressingCompleted(object sender, Microsoft.Kinect.Input.KinectPressingCompletedEventArgs e)
        {
            this.HandPointerLeave += Pressable_HandPointerLeave;
            HandPointerLeave(sender, e);
        }

        void PressableInputModel_Tapped(object sender, Microsoft.Kinect.Input.KinectTappedEventArgs e)
        {
            this.HandPointerTapped += Pressable_HandPointerTapped;
            HandPointerTapped(sender, e);
        }


        #region Globale Eventhandler
        /// <summary>
        /// Hier kann Verhalten definiert werden, dass global fuer alle Pressable Elemente gilt.
        /// Nicht definiert um variables Verhalten zu haben, aber noetig um keine NullReferenceException zu erhalten.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        void Pressable_HandPointerHolding(object sender, Microsoft.Kinect.Input.KinectHoldingEventArgs e)
        {

        }

        void Pressable_HandPointerHover(object sender, Microsoft.Kinect.Input.KinectPressingUpdatedEventArgs e)
        {

        }

        void Pressable_HandPointerLeave(object sender, Microsoft.Kinect.Input.KinectPressingCompletedEventArgs e)
        {

        }

        void Pressable_HandPointerEnter(object sender, Microsoft.Kinect.Input.KinectPressingStartedEventArgs e)
        {

        }

        void Pressable_HandPointerTapped(object sender, Microsoft.Kinect.Input.KinectTappedEventArgs e)
        {

        }

        #endregion


        #region Delegates and Eventhandler for costum behaviour
        public delegate void DragStartHandler(object sender, Microsoft.Kinect.Input.KinectManipulationStartedEventArgs e);

        public event DragStartHandler DragStart;

        public void OnDragStart(object sender, Microsoft.Kinect.Input.KinectManipulationStartedEventArgs e)
        {
            DragStart(sender, e);
        }

        /// <summary>
        /// Event that fires after the pressing gesture has finished. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void TappedHandler(object sender, Microsoft.Kinect.Input.KinectTappedEventArgs e);

        public event TappedHandler HandPointerTapped;

        public void OnTapped(object sender, Microsoft.Kinect.Input.KinectTappedEventArgs e)
        {
            HandPointerTapped(sender, e);
        }

        /// <summary>
        /// Kinect Pressing started equals to the old HandPointer Enter event, meaning it fires when the control is entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EnterHandler(object sender, Microsoft.Kinect.Input.KinectPressingStartedEventArgs e);

        public event EnterHandler HandPointerEnter;

        public void OnEnter(object sender, Microsoft.Kinect.Input.KinectPressingStartedEventArgs e)
        {
            HandPointerEnter(sender, e);
            
        }

        /// <summary>
        /// Handpointer hover event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void HoverHandler(object sender, Microsoft.Kinect.Input.KinectPressingUpdatedEventArgs e);

        public event HoverHandler HandPointerHover;

        public void OnHover(object sender, Microsoft.Kinect.Input.KinectPressingUpdatedEventArgs e)
        {
            HandPointerHover(sender, e);

        }

        /// <summary>
        /// HandPointer Leave Event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void LeavingHandler(object sender, Microsoft.Kinect.Input.KinectPressingCompletedEventArgs e);

        public event LeavingHandler HandPointerLeave;

        public void OnLeaving(object sender, Microsoft.Kinect.Input.KinectPressingCompletedEventArgs e)
        {
            HandPointerLeave(sender, e);

        }

        /// <summary>
        /// The holding event fires when the HandPointer is pressing down and for certain amount of time;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void HoldingHandler(object sender, Microsoft.Kinect.Input.KinectHoldingEventArgs e);

        public event HoldingHandler HandPointerHolding;

        public void OnHolding(object sender, Microsoft.Kinect.Input.KinectHoldingEventArgs e)
        {
            HandPointerHolding(sender, e);

        }


        #endregion
        //Sagt dem KinectController welches InputModel erlaubt ist
        // Auf true setzen, sonst kann das entsprechende Inputmodel nicht generiert werden.
        public bool IsManipulatable
        {
            get { return false; }
        }

        public bool IsPressable
        {
            get { return true; }
        }

        
    }
}
