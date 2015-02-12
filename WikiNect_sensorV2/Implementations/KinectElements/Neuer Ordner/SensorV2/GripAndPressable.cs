using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Kinect.Input;
using Microsoft.Kinect.Wpf.Controls;
using Microsoft.Kinect.Toolkit.Input;

namespace Kinect
{
    class GripAndPressable : UserControl
    {
        
        public GripAndPressable()
        {
            this.Loaded += GripAndPressable_Loaded;                       
        }

        void GripAndPressable_Loaded(object sender, RoutedEventArgs e)
        {
            GripableWithoutKinoogle g = new GripableWithoutKinoogle();
            PressableWithoutKinoogle p = new PressableWithoutKinoogle();
            p.Content = this.Content;
            g.Content = p;
            this.Content = g;

            g.GripComplete += g_GripComplete;
            g.GripStart += g_GripStart;
            g.GripUpdate += g_GripUpdate;

            p.HandPointerLeave += p_HandPointerLeave;
            p.HandPointerHolding += p_HandPointerHolding;
            p.HandPointerHover += p_HandPointerHover;
            p.HandPointerTapped += p_HandPointerTapped;
            p.HandPointerEnter += p_HandPointerEnter;
        }

        #region Global events from delegates
        void GripAndPressable_HandPointerTapped(object sender, KinectTappedEventArgs e)
        {

        }
        void GripAndPressable_HandPointerHover(object sender, KinectPressingUpdatedEventArgs e)
        {

        }
        void GripAndPressable_HandPointerHolding(object sender, KinectHoldingEventArgs e)
        {

        }
        void GripAndPressable_HandPointerLeave(object sender, KinectPressingCompletedEventArgs e)
        {

        }
        void GripAndPressable_GripUpdate(object sender, KinectManipulationUpdatedEventArgs e)
        {

        }
        void GripAndPressable_GripStart(object sender, KinectManipulationStartedEventArgs e)
        {

        }
        void GripAndPressable_GripComplete(object sender, KinectManipulationCompletedEventArgs e)
        {

        }
        #endregion

        #region Global events from controller
        void p_HandPointerEnter(object sender, KinectPressingStartedEventArgs e)
        {
            this.HandPointerEnter += GripAndPressable_HandPointerEnter;
            OnEnter(sender, e);
        }

        void p_HandPointerTapped(object sender, KinectTappedEventArgs e)
        {
            this.HandPointerTapped += GripAndPressable_HandPointerTapped;
            OnTapped(sender, e);
        }

        void p_HandPointerHover(object sender, KinectPressingUpdatedEventArgs e)
        {
            this.HandPointerHover += GripAndPressable_HandPointerHover;
            OnHover(sender, e);
        }
        
        void p_HandPointerHolding(object sender, KinectHoldingEventArgs e)
        {
            this.HandPointerHolding += GripAndPressable_HandPointerHolding;
            OnHolding(sender, e);
        }

        void p_HandPointerLeave(object sender, KinectPressingCompletedEventArgs e)
        {
            this.HandPointerLeave += GripAndPressable_HandPointerLeave;
            OnLeaving(sender, e);
        }

        void g_GripStart(object sender, KinectManipulationStartedEventArgs e)
        {
            this.GripStart += GripAndPressable_GripStart;
            onGripStart(sender, e);
        }

        void g_GripUpdate(object sender, KinectManipulationUpdatedEventArgs e)
        {
            this.GripUpdate += GripAndPressable_GripUpdate;
            onGripUpdate(sender, e);
        }

        void g_GripComplete(object sender, Microsoft.Kinect.Input.KinectManipulationCompletedEventArgs e)
        {
            this.GripComplete += GripAndPressable_GripComplete;
            onGripComplete(sender, e);
        }

        #endregion

        #region Delegates
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
            this.HandPointerEnter += GripAndPressable_HandPointerEnter;
            HandPointerEnter(sender, e);

        }

        void GripAndPressable_HandPointerEnter(object sender, KinectPressingStartedEventArgs e)
        {
            
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

    }

    class GripableWithoutKinoogle : UserControl, IKinectControl
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

    class PressableWithoutKinoogle : UserControl, IKinectControl
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
