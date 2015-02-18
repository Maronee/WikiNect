using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Shapes;

using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using Microsoft.Kinect.VisualGestureBuilder;

using Microsoft.Kinect.Input;
//using Microsoft.Kinect.Toolkit.Input;

using Kinect;
using DataConnection;
using DataStore;
using Segmentation;
using WikiNectLayout.Implementions.Xamls;

namespace WikiNectLayout.Implementions.Xamls
{
    /// <summary>
    /// Interaktionslogik für SegmentationDisplay.xaml
    /// </summary>
    public partial class SegmentationDisplay : KinoogleInterface
    {

        #region Variables
        // this receives the argument of SegmentationMainWindow
        KinImage imageInit;

        //Path string of image file
        string url_Seg;

        //Bitmaps, which will be painted or marked with points and Lines by the use of System.Drawing.Graphics
        private System.Drawing.Bitmap startImage, startImage_unchanged;

        //Generic Lists for holding both System.Windows.Points and System.Drawing.Points in CropMode.PolygonMode   
        private List<System.Windows.Point> mouseClicked = new List<System.Windows.Point>(); //mouse click points to crop image
        private List<System.Drawing.Point> drawingTempPoints = new List<System.Drawing.Point>(); //mouse click points to crop image

        // List of saved file urls
        private List<string> savedFileUrl = new List<string>();
        private List<KinImage> chosenImage = new List<KinImage>();

        //register and capture mouse clicks as points. These points builds the generic lists 
        private System.Windows.Point win_tempPoint;
        private System.Drawing.Point draw_tempPoint;

        //counter for rigistered points by clicking the image control, undo counter and  save counters 
        private int clickCount, undocount = 0, croppingCount = 0, saveExtended = 0, singleSave = 0, cropPanelRefresh = 0;
        private int saveNameCount = 0;
        //Cropping Modes
        private enum CropMode { RectMode, PolygonMode, None };
        private CropMode cropMode;

        //Graphics made from Bitmap startImage. g is the main Graphic and temp_g for undo function 
        private Graphics g;

        //pen and brush for drawing points and lines
        private System.Drawing.Pen a = new System.Drawing.Pen(System.Drawing.Color.Red, 2);
        private SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);

        //Image Control. After finishing the manipulation of bitmaps they will be converted to this image control
        private System.Windows.Controls.Image finalImage;

        //Cropping status
        private bool croppingDone = false;
        private bool croppingInit = false;
        private bool isPolygonCropping = false;
        private bool isRectCropping = false;

        //if an image is loaded
        private bool isImgLoaded = false;
        #endregion



        #region MainWindow
        public SegmentationDisplay(KinImage image)
        {
            DataContext = this;
            InitializeComponent();
            Segmentation_AddHandler();
            imageInit = image;
            url_Seg = image.url;
            cropMode = CropMode.None;
            FirstRefreshImg();
            this.startKinoogleDetection();
            this.Loaded += SegmentationDisplay_Loaded;
        }

        void SegmentationDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            //var window = KinectCoreWindow.GetForCurrentThread();
            //window.PointerMoved += window_PointerMoved;
            this.startKinoogleDetection();

            double w = orgImage.ActualWidth;
            double h = orgImage.ActualHeight;
            Console.WriteLine(orgImage.ActualWidth);
            Console.WriteLine(orgImage.ActualHeight);
            this.orgImage.Width = w;
            this.orgImage.Height = h;
            //this.imageContainer.Width = w;
            //this.imageContainer.Height = h;

        }


        //KinectPointerPoint handPointer;
        //void window_PointerMoved(object sender, KinectPointerEventArgs e)
        //{
        //    if (IsApplicableKinectPoint(e.CurrentPoint)) 
        //    {
        //        this.handPointer = e.CurrentPoint;
        //    }
        //}

        //bool IsApplicableKinectPoint(KinectPointerPoint kinectPoint)
        //{
        //    return (kinectPoint.Properties.IsEngaged &&
        //      kinectPoint.Properties.IsInRange);
        //}  
        #endregion

        #region SegmentationDisplay Properties
        private bool IsCroppingDone
        {
            get
            {
                return croppingDone;
            }

            set
            {
                croppingDone = value;
            }
        }

        private bool IsCroppingInitialized
        {
            get
            {
                return croppingInit;
            }

            set
            {
                croppingInit = value;
            }
        }

        private bool IsPlygonCropping
        {
            get
            {
                return isPolygonCropping;
            }

            set
            {
                isPolygonCropping = value;
            }
        }

        private bool IsRectangularCropping
        {
            get
            {
                return isRectCropping;
            }

            set
            {
                isRectCropping = value;
            }
        }

        private bool IsImageLoaded
        {
            get
            {
                return isImgLoaded;
            }

            set
            {
                isImgLoaded = value;
            }
        }
        #endregion

        #region Segmentation AddHandler
        //Adding the EventHandler for both System.Windows.Controls.Image orgImage and crpImage  
        private void Segmentation_AddHandler()
        {
            //orgImage.ImageGripped += new ImageGrippedEventHandler(KinectDrawing_Grip);
            //orgImage.ImagePressed += new ImagePressedEventHandler(OriginalImagePressed);
            orgImage.RightClicked += new MouseRightButtonEventHandler(OrgImage_MouseRightButtonDown);
            //crpImage.ImagePressed += new ImagePressedEventHandler(CroppedImagePressed);
            //crpImage.ImageGripped += new ImageGrippedEventHandler(CroppedImageGripped);
            crpImage.RightClicked += new MouseRightButtonEventHandler(CrpImage_MouseRightButtonDown);
            crpImage.Gripped += new GrippedEventHandler(CroppedImagePick);
        }
        #endregion

        #region Kinect Private Methods
        //Grip on System.Windows.Controls.Image orgImage marked points on grip position
        private void KinectDrawing_Grip(object sender, Microsoft.Kinect.Input.KinectManipulationStartedEventArgs e)
        {
            //var parent = (Microsoft.Kinect.Toolkit.Input.ManipulatableModel)sender;
            //GripAndPressable gp = (GripAndPressable)parent.Content;
            //orgImage = (KinImage)gp.Content; // System.Windows.Controls.Image orgImage is the sender of Event

            //System.Windows.Point targetLoc = orgImage.PointToScreen(new System.Windows.Point(0, 0));
            //Rect rect = new Rect(targetLoc.X, targetLoc.Y, orgImage.Width, orgImage.Height);
            //Point p = Microsoft.Kinect.Toolkit.Input.InputPointerManager.TransformInputPointerCoordinatesToWindowCoordinates(e.Position, rect);


            if (cropMode != CropMode.None && !this.IsCroppingDone)
            {
                //register the position of HandPointer in grip state
                win_tempPoint = new System.Windows.Point() { X = e.Position.X * this.orgImage.ActualWidth, Y = e.Position.Y * this.orgImage.ActualHeight };

                double w = orgImage.ActualWidth;
                double h = orgImage.ActualHeight;
                Console.WriteLine(orgImage.ActualWidth);
                Console.WriteLine(orgImage.ActualHeight);
                this.orgImage.Width = w + 1;
                this.orgImage.Height = h + 1;
                //this.imageContainer.Width = w + 1;
                //this.imageContainer.Height = h + 1;

                Console.WriteLine(this.orgImage.Source.Width);
                Console.WriteLine(this.orgImage.Source.Height);
                Console.WriteLine(this.orgImage.Width);
                Console.WriteLine(this.orgImage.Height);
                Console.WriteLine("Handpointer X: " + e.Position.X + " Y: " + e.Position.Y);
                Console.WriteLine("Handpointer X: " + e.Position.X * this.orgImage.ActualWidth + " Y: " + e.Position.Y * this.orgImage.ActualHeight);
                //Console.WriteLine("container width: " + imageContainer.ActualWidth);


                //Converting System.Windows.Point to System.Drawing.Point
                int x = Convert.ToInt32(win_tempPoint.X);
                int y = Convert.ToInt32(win_tempPoint.Y);

                //System.Drawing.Point for PolygonCrop Class in order to calculate Croping Region 
                draw_tempPoint = new System.Drawing.Point(x, y);
                clickCount = mouseClicked.Count;

                //System.Windows.Point Container
                mouseClicked.Add(win_tempPoint);

                //System.Drawing.Point which will be passed to PolygonCrop Class
                drawingTempPoints.Add(draw_tempPoint);

                // calls the drawing methode
                DrawingLP(mouseClicked, drawingTempPoints);
            }


        }

        //Pressing on System.Windows.Control.Image orgImage does the cropping like pressing the crop button "cropBtn"  
        private void OriginalImagePressed(object sender, Microsoft.Kinect.Input.KinectTappedEventArgs e)
        {
            //var gp = (GripAndPressable)Parent;
            //orgImage = (KinImage)gp.Content;

            if (!this.IsCroppingDone)
            {
                Cropping();
            }

            else if (this.IsCroppingDone)
            {
                Refresh();
            }
        }

        private void CroppedImageGripped(object sender, Microsoft.Kinect.Input.KinectManipulationStartedEventArgs e)
        {
            //var parent = (GripAndPressable)sender;
            //crpImage = (KinImage)parent.Content;
            Refresh();
        }

        private void CroppedImagePressed(object sender, Microsoft.Kinect.Input.KinectTappedEventArgs e)
        {
            //var parent = (GripAndPressable)sender;
            //crpImage = (KinImage)parent.Content;

            CroppedImagePickMethod();
        }


        #endregion

        #region Private Methodes
        //Refresh botth orgImage and crpImag Image Controls (Restore the Progaramm's Start Position ) 
        private void FirstRefreshImg()
        {
            BitmapImage imagesource = new BitmapImage(new Uri(imageInit.url, UriKind.Absolute));
            crpImage.Source = null;
            status_TextBox.Clear();


            if (imagesource.Height > orgImage.Height || imagesource.Width > orgImage.Width ||   // checks if height and width of the original System.Drawing.Bitmap startImage 
                imagesource.Height < orgImage.Height || imagesource.Width < orgImage.Width)     // doesn't match the System.Windows.Controls.Image orgImage then calls the Helper.ImageResize
            {
                using (Bitmap temp_startImage = Helper.BitmapImage2Bitmap(imagesource))
                {
                    startImage = Helper.ImageResize(temp_startImage, orgImage);
                    //startImage = temp_startImage;
                    finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage);
                    this.orgImage.Source = finalImage.Source;
                    startImage_unchanged = startImage.Clone(new System.Drawing.Rectangle(0, 0, startImage.Width, startImage.Height), startImage.PixelFormat);
                    //creating Graphics from Bitmap bitmap_temp (Working with a copy of startImage) in order to Draw Points and Lines on it
                    g = Graphics.FromImage(startImage);
                    crpImage.Source = null;
                    status_TextBox.Clear();
                }

            }
            else      // if height and width of original Bitmap matches the orgImage there is no need to resize the Bitmap
            {
                startImage = Helper.BitmapImage2Bitmap(imagesource);
                this.orgImage.Source = imagesource;
                startImage_unchanged = startImage.Clone(new System.Drawing.Rectangle(0, 0, startImage.Width, startImage.Height), startImage.PixelFormat);
                //creating Graphics from Bitmap bitmap_temp (Working with a copy of startImage) in order to Draw Points and Lines on it
                g = Graphics.FromImage(startImage);
                crpImage.Source = null;
                status_TextBox.Clear();
            }
            isImgLoaded = true;

        }

        // Draw lines and points
        // the caller of this methode should check first "if (cropMode != CropMode.None && !this.IsCroppingDone)"
        private void DrawingLP(List<System.Windows.Point> windowsPoints, List<System.Drawing.Point> drawingPoints)
        {

            if (cropMode == CropMode.PolygonMode)
            {
                isPolygonCropping = true;
                status_TextBox.Text = "Polygon Mode";
            }

            else if (cropMode == CropMode.RectMode)
            {
                isRectCropping = true;
                status_TextBox.Text = "Rectangular Mode";
            }


            //looping to draw the points and the lines between the points
            for (int i = 0; i < windowsPoints.Count; i++)
            {

                //Draw Points
                g.FillEllipse(brush, (drawingPoints[i].X) - 5, (drawingPoints[i].Y) - 5, 10, 10);


                //Draw Lines between Points if the number of Points are more than one
                if (windowsPoints.Count > 1)
                {
                    try
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.DrawLine(a, drawingPoints[windowsPoints.Count - 2], drawingPoints[windowsPoints.Count - 1]);
                    }
                    catch (Exception ex)
                    {
                        this.Refresh();
                        MessageBox.Show("Program has encountered an unexpected error and has to be reset." + ex);
                    }

                }

            }
            finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage);
            this.orgImage.Source = finalImage.Source;
        }

        // Cropping Function
        // After determinig the Nodes, which defines the polygon or Rectangle, calls either the PolygonCrop class or RectCrop class 
        // to calculate the cropping region
        private void Cropping()
        {
            if (cropMode == CropMode.PolygonMode)
            {
                if (this.IsPlygonCropping && mouseClicked.Count > 2)
                {
                    g.Dispose();
                    finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage_unchanged);
                    orgImage.Source = finalImage.Source;
                    PolygonCrop polygonCrop = new PolygonCrop(startImage_unchanged, crpImage, drawingTempPoints);
                    croppingDone = true;
                    isPolygonCropping = false;
                    croppingInit = false;
                    this.startImage.Dispose();
                    this.startImage_unchanged.Dispose();
                    mouseClicked.Clear();
                    drawingTempPoints.Clear();
                }

                else if (this.IsPlygonCropping && mouseClicked.Count < 3)
                {
                    status_TextBox.Text = "At Least Three Points";
                }

                else if (this.IsCroppingDone)
                {
                    status_TextBox.Text = "Cropping Completed";
                }

                else
                {
                    status_TextBox.Text = "No Points Marked";
                }

            }

            else if (cropMode == CropMode.RectMode)
            {
                if (this.IsRectangularCropping && mouseClicked.Count > 3)
                {
                    g.Dispose();
                    finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage_unchanged);
                    orgImage.Source = finalImage.Source;
                    RectCrop rectCrop = new RectCrop(startImage_unchanged, orgImage, crpImage, mouseClicked);
                    croppingDone = true;
                    isRectCropping = false;
                    croppingInit = false;
                    this.startImage.Dispose();
                    this.startImage_unchanged.Dispose();
                    mouseClicked.Clear();
                    drawingTempPoints.Clear();
                }

                else if (this.IsRectangularCropping && mouseClicked.Count < 4)
                {
                    status_TextBox.Text = "At Least four Points";
                }

                else if (this.IsCroppingDone)
                {
                    status_TextBox.Text = "Cropping Completed";
                }

                else
                {
                    status_TextBox.Text = "No Points Marked";
                }
            }

            else if (!this.IsImageLoaded)
            {
                status_TextBox.Text = "Pick A Photo";
            }

            else if (cropMode == CropMode.None)
            {
                status_TextBox.Text = "Select Cropping Mode";
            }
        }

        // Save. if an image is loade and a region is selected this calls the VisualToImage Class 
        private void Save()
        {
            if (!this.IsImageLoaded)
            {
                status_TextBox.Text = "Pick A Photo";
            }
            else if (this.IsImageLoaded && crpImage.Source == null)
            {
                status_TextBox.Text = "Nothing to Save";
            }

            else if (crpImage.Source != null && chosenImage.Count == 0) // saving single image in crpImage when no image is in cropped panel
            {
                singleSave += 100;
                string singleSaveName = VisualToImage.SavedUrl(url_Seg, singleSave, cropPanelRefresh, saveExtended);
                RenderTargetBitmap singleSave_rtb = VisualToImage.RenderVisual(crpImage);
                BitmapEncoder singleSaveEncoder = new JpegBitmapEncoder();
                BitmapFrame frame = BitmapFrame.Create(singleSave_rtb);
                singleSaveEncoder.Frames.Add(frame);
                var dir = System.AppDomain.CurrentDomain.BaseDirectory;
                Console.WriteLine(dir);
                //saving the cropped image under targetFileName
                using (var stream = File.Create(singleSaveName))
                {
                    singleSaveEncoder.Save(stream);
                }
                status_TextBox.Text = "One Saved";
            }

            else
            {
                VisualToImage vti = new VisualToImage(chosenImage, savedFileUrl, url_Seg, croppingCount, saveExtended);
                saveExtended += (croppingCount - saveExtended);
                status_TextBox.Text = "All Saved";
            }
        }

        //Polygon Undo: By Clicking undo the last Line will be deleted 
        private void UndoDrawingLP(List<System.Windows.Point> windowsPoints, List<System.Drawing.Point> drawingPoints)
        {
            startImage.Dispose();
            g.Dispose(); // we have to dipose g object not to blow up ram

            using (Bitmap bitmapTmp_undo = (Bitmap)startImage_unchanged.Clone())
            {
                using (Graphics temp_g = Graphics.FromImage(bitmapTmp_undo))
                {
                    for (int i = 0; i < windowsPoints.Count; i++)
                    {
                        temp_g.FillEllipse(brush, (drawingPoints[i].X) - 5, (drawingPoints[i].Y) - 5, 10, 10);

                        for (int j = 0; j < windowsPoints.Count - 1; j++)
                        {

                            temp_g.SmoothingMode = SmoothingMode.AntiAlias;
                            temp_g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            temp_g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            temp_g.DrawLine(a, drawingPoints[j].X, drawingPoints[j].Y, drawingPoints[j + 1].X, drawingPoints[j + 1].Y);

                        }
                    }

                }
                startImage = (Bitmap)bitmapTmp_undo.Clone();
            }

            finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage);
            orgImage.Source = finalImage.Source;
            g = Graphics.FromImage(startImage);
        }

        //Undo Function, which will be called by Undo Button
        private void Undo()
        {
            if (!this.IsImageLoaded)
            {
                status_TextBox.Text = "Pick A Photo";
            }

            else if (cropMode == CropMode.PolygonMode || cropMode == CropMode.RectMode)
            {
                if (this.IsPlygonCropping && !this.IsCroppingDone || this.IsRectangularCropping && !this.IsCroppingDone)
                {
                    if (mouseClicked.Count > 0)
                    {
                        undocount++;

                        try
                        {
                            //releasing the last Point in both Lists of Points
                            mouseClicked.RemoveRange(clickCount, 1);
                            drawingTempPoints.RemoveRange(clickCount, 1);
                            clickCount--;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());

                        }

                        UndoDrawingLP(mouseClicked, drawingTempPoints);
                        status_TextBox.Text = "Undoing";
                    }

                    else
                    {
                        status_TextBox.Text = "Undo-Nothing-To-Do";
                    }
                }

                else if (this.IsCroppingDone)
                {
                    Refresh();
                }

                else if (!this.IsPlygonCropping && !this.IsCroppingDone || !this.IsRectangularCropping && !this.IsCroppingDone)
                {
                    status_TextBox.Text = "Undo-Nothing-To-Do";
                }
            }

            else
            {
                status_TextBox.Text = "Select Cropping Mode";
            }
        }

        //Refresh Function, which will be called by Refresh Button
        private void Refresh()
        {
            if (!this.IsImageLoaded)
            {
                status_TextBox.Text = "Nothing To Refresh";
            }

            else if (this.IsCroppingDone)
            {
                croppingDone = false;
                isPolygonCropping = false;
                isRectCropping = false;
                undocount = 0;
                FirstRefreshImg();
                status_TextBox.Text = "REFRESHED";
            }

            else if ((cropMode == CropMode.PolygonMode || cropMode == CropMode.RectMode))
            {
                this.g.Dispose();
                this.startImage.Dispose();
                this.startImage_unchanged.Dispose();
                croppingDone = false;
                isPolygonCropping = false;
                isRectCropping = false;
                mouseClicked.Clear();
                drawingTempPoints.Clear();
                undocount = 0;
                FirstRefreshImg();
                status_TextBox.Text = "REFRESHED";

            }

            else if (cropMode == CropMode.None && !this.IsCroppingInitialized)
            {
                status_TextBox.Text = "REFRESHED";
            }
        }

        private void CroppedPanelRefresh()
        {
            croppingCount = 0;
            saveExtended = 0;
            saveNameCount = 0;
            chosenImage.Clear();
            savedFileUrl.Clear();
            croppedImgPanel.Children.Clear();
            cropPanelRefresh++;
            status_TextBox.Text = "Panel Refreshed";
        }
        #endregion

        #region Event Handlers, Routed Events and Butttons

        //On Image Control(orgImage) Mouse Left Button Event Handler
        public void OrgImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (cropMode != CropMode.None && !this.IsCroppingDone)
            {
                //register the position of Mouse Clicks
                win_tempPoint = e.GetPosition(orgImage);

                Console.WriteLine("Mouse X: " + e.GetPosition(orgImage).X);
                Console.WriteLine("Mouse Y: " + e.GetPosition(orgImage).Y);

                //Converting System.Windows.Point to System.Drawing.Point
                int x = Convert.ToInt32(win_tempPoint.X);
                int y = Convert.ToInt32(win_tempPoint.Y);

                //System.Drawing.Point for PolygonCrop Class in order to calculate Croping Region 
                draw_tempPoint = new System.Drawing.Point(x, y);
                clickCount = mouseClicked.Count;

                //System.Windows.Point Container
                mouseClicked.Add(win_tempPoint);

                //System.Drawing.Point which will be passed to PolygonCrop Class
                drawingTempPoints.Add(draw_tempPoint);

                // calls the drawing methode
                DrawingLP(mouseClicked, drawingTempPoints);
            }
        }

        // Right click on orgImage will execute cropping
        private void OrgImage_MouseRightButtonDown(object sender, EventArgs e)
        {
            if (!this.IsCroppingDone)
            {
                Cropping();
            }

            else if (this.IsCroppingDone)
            {
                Refresh();
            }
        }

        // right click on crpImage(Cropped Image) will execute Refresh()
        private void CrpImage_MouseRightButtonDown(object sender, EventArgs e)
        {
            Refresh();
        }

        //Initiate Rectangular Cropping
        private void RectBtnOnClick(object sender, RoutedEventArgs e)
        {
            if (!isImgLoaded)
            {
                status_TextBox.Text = "Pick A Photo";
            }

            else if (cropMode == CropMode.None)
            {
                cropMode = CropMode.RectMode;
                croppingInit = true;
                status_TextBox.Text = "Rectangular Mode";
            }

            else if (this.IsCroppingDone)
            {
                Refresh();
                cropMode = CropMode.RectMode;
            }

            else if (cropMode == CropMode.PolygonMode)
            {
                cropMode = CropMode.RectMode;
                croppingInit = true;
                isRectCropping = true;
                status_TextBox.Text = "Rectangular Mode";

            }
        }


        //Initiate Polygon Cropping
        private void polygonBtnOnClick(object sender, RoutedEventArgs e)
        {
            if (!isImgLoaded)
            {
                status_TextBox.Text = "Pick A Photo";
            }

            else if (cropMode == CropMode.None)
            {
                cropMode = CropMode.PolygonMode;
                croppingInit = true;
                status_TextBox.Text = "Polygon Mode";

            }

            else if (this.IsCroppingDone)
            {
                Refresh();
                cropMode = CropMode.PolygonMode;
            }

            else if (cropMode == CropMode.RectMode)
            {
                cropMode = CropMode.PolygonMode;
                croppingInit = true;
                isPolygonCropping = true;
                status_TextBox.Text = "Polygon Mode";
            }
        }

        //After determinig the Nodes, which defines the polygon or Rectangle, executes Cropping()
        private void CropBtnOnClick(object sender, RoutedEventArgs e)
        {
            Cropping();
        }

        //Save Button 
        private void SaveBtnOnClick(object sender, RoutedEventArgs e)
        {
            Save();
        }

        //Undo Button
        private void UndoBtnOnClick(object sender, RoutedEventArgs e)
        {

            Undo();

        }
        //Refresh Button
        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        // Event handler for removing the cropped picture from cropped image panel
        private void CroppedImageRemove(object sender, EventArgs e)
        {
            if (!(chosenImage.IndexOf((KinImage)sender, 0, saveExtended) >= 0)) // if position of the sender in  List<KinImage> chosenImage is not before saveExtended(last saving)
            {
                croppingCount--;
                chosenImage.Remove((KinImage)sender);
                savedFileUrl.Remove(((KinImage)sender).url);
                croppedImgPanel.Children.Remove((KinImage)sender);
            }

            else
            {
                croppedImgPanel.Children.Remove((KinImage)sender);
            }
        }

        private void CroppedImagePick(object sender, EventArgs e)
        {
            CroppedImagePickMethod();
        }

        private void CroppedImagePickMethod()
        {
            croppingCount++;
            saveNameCount++;
            KinImage displayCroppedImg = new KinImage();
            displayCroppedImg.Width = 100;
            displayCroppedImg.Height = 130;
            displayCroppedImg.Stretch = Stretch.Uniform;
            displayCroppedImg.StretchDirection = StretchDirection.Both;
            displayCroppedImg.Margin = new Thickness(5);
            //AJ   Style imagestyle = this.FindResource("HoverImageStyle") as Style;
            //AJ       displayCroppedImg.Style = imagestyle;
            displayCroppedImg.Source = VisualToImage.RenderVisual(crpImage);
            string fileNameToSave = VisualToImage.SavedUrl(url_Seg, saveNameCount, cropPanelRefresh, saveExtended);
            displayCroppedImg.url = fileNameToSave;
            savedFileUrl.Add(fileNameToSave);
            chosenImage.Add(displayCroppedImg);
            displayCroppedImg.Gripped += new GrippedEventHandler(CroppedImageEnlarged);
            displayCroppedImg.RightClicked += new MouseRightButtonEventHandler(CroppedImageRemove);
            croppedImgPanel.Children.Add(displayCroppedImg);
        }

        // event handler for viewing the cropped images. 
        // clicking on cropped images on cropped panel will maximize that image
        private void CroppedImageEnlarged(object sender, EventArgs e)
        {
            var cid = new CroppedImageDisplay((KinImage)sender);
            Grid.SetColumnSpan(cid, 2);
            Grid.SetRowSpan(cid, 1);
            this.imageDisplay.Children.Add(cid);
        }

        private void OnLoadedStoryboardCompleted(object sender, RoutedEventArgs e)
        {
            CroppedPanelRefresh();

            if (!this.IsCroppingInitialized)
            {
                g.Dispose();
                startImage.Dispose();
                startImage_unchanged.Dispose();
                var parent = (Panel)this.Parent;
                parent.Children[0].Visibility = System.Windows.Visibility.Visible;
                parent.Children.Remove(this);
            }

            else if (this.IsCroppingDone)
            {
                var parent = (Panel)this.Parent;
                parent.Children[0].Visibility = System.Windows.Visibility.Visible;
                parent.Children.Remove(this);
            }

            else if ((this.IsRectangularCropping || this.IsPlygonCropping))
            {
                g.Dispose();
                startImage.Dispose();
                startImage_unchanged.Dispose();
                mouseClicked.Clear();
                drawingTempPoints.Clear();
                var parent = (Panel)this.Parent;
                parent.Children[0].Visibility = System.Windows.Visibility.Visible;
                parent.Children.Remove(this);
            }

        }

        private void CropPanelRefreshBtnOnClick(object sender, RoutedEventArgs e)
        {
            CroppedPanelRefresh();
        }


        //Help
        //private void HelpBtnOnClick(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Picture Selection : Picture Buttons must be pressed to be selected." + "\n" + "\n" +
        //                    "Control Buttons : Other Buttons can be either pressed or griped to be selected." + "\n" +
        //                    "Mark Gripping Points : Griping the image will leave cropping points on the position of grip." + "\n" +
        //                    "Cropping : After marking the cropping points, image cropping can be completed either by simply press the image or press/gripping the crop button." + "\n" +
        //                    "Saving : Pressing the crvopped image will save it", "Help");
        //}
        #endregion

        #region Kinoogle implementation
        KinectRegion _kinectRegion;
        KinectSensor _kinectSensor;
        Body[] _bodies;
        ulong _currentTrackedId;
        CameraSpacePoint _leftHandOrigin;
        CameraSpacePoint _rightHandOrigin;
        CameraSpacePoint _leftHandCycle;
        CameraSpacePoint _rightHandCycle;
        BodyFrameReader _bodyReader;
        int _counter;
        int _currentCount;
        bool _constantHandState;
        KinoogleExtensions.HandGesture _gestureState;
        VisualGestureBuilderFrameSource _vgbFrameSource;
        VisualGestureBuilderFrameReader _vgbFrameReader;
        double _usedDistance;
        string _currentGesture;
        KinectRegion KinoogleInterface.kinectRegion
        {
            get
            {
                return _kinectRegion;
            }
            set
            {
                _kinectRegion = value;
            }
        }

        public KinectSensor kinectSensor
        {
            get
            {
                return _kinectSensor;
            }
            set
            {
                _kinectSensor = value;
            }
        }

        Body[] KinoogleInterface.bodies
        {
            get
            {
                return _bodies;
            }
            set
            {
                _bodies = value;
            }
        }

        public ulong currentTrackedId
        {
            get
            {
                return _currentTrackedId;
            }
            set
            {
                _currentTrackedId = value;
            }
        }

        public CameraSpacePoint leftHandOrigin
        {
            get
            {
                return _leftHandOrigin;
            }
            set
            {
                _leftHandOrigin = value;
            }
        }

        public CameraSpacePoint rightHandOrigin
        {
            get
            {
                return _rightHandOrigin;
            }
            set
            {
                _rightHandOrigin = value;
            }
        }

        public CameraSpacePoint leftHandCycle
        {
            get
            {
                return _leftHandCycle;
            }
            set
            {
                _leftHandCycle = value;
            }
        }

        public CameraSpacePoint rightHandCycle
        {
            get
            {
                return _rightHandCycle;
            }
            set
            {
                _rightHandCycle = value;
            }
        }

        public BodyFrameReader bodyReader
        {
            get
            {
                return _bodyReader;
            }
            set
            {
                _bodyReader = value;
            }
        }

        public int counter
        {
            get
            {
                return _counter;
            }
            set
            {
                _counter = value;
            }
        }

        public int currentCount
        {
            get
            {
                return _currentCount;
            }
            set
            {
                _currentCount = value;
            }
        }

        public bool constantHandState
        {
            get
            {
                return _constantHandState;
            }
            set
            {
                _constantHandState = value;
            }
        }

        public KinoogleExtensions.HandGesture gestureState
        {
            get
            {
                return _gestureState;
            }
            set
            {
                _gestureState = value;
                handStateLabel.GetBindingExpression(Label.ContentProperty).UpdateTarget();
            }
        }

        public VisualGestureBuilderFrameSource vgbFrameSource
        {
            get
            {
                return _vgbFrameSource;
            }
            set
            {
                _vgbFrameSource = value;
            }
        }

        public VisualGestureBuilderFrameReader vgbFrameReader
        {
            get
            {
                return _vgbFrameReader;
            }
            set
            {
                _vgbFrameReader = value;
            }
        }

        public double usedDistance
        {
            get
            {
                return _usedDistance;
            }
            set
            {
                _usedDistance = value;
            }
        }

        public string currentGesture
        {
            get
            {
                return _currentGesture;
            }
            set
            {
                _currentGesture = value;
                kinoogleGestureLabel.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                //kinoogleGestureLabel.GetBindingExpression(Label.ContentProperty).UpdateTarget();
            }
        }


        public void startKinoogleDetection()
        {
            this.initKinoogle();
        }


        public void bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            this.kinoogleBodyFrameHandler(e);
        }

        public void vgbFrameReader_FrameArrived(object sender, Microsoft.Kinect.VisualGestureBuilder.VisualGestureBuilderFrameArrivedEventArgs e)
        {
            this.kinoogleVgbFrameHandler(e);
        }

        public void onPan(float xDiff, float yDiff)
        {
            // m -> cm
            double x = xDiff * 100;
            double y = yDiff * 100;
            //extension Funktion Cm zu Pixel entsprechend der .Net Normwerte
            x = x.CmToPx();
            y = y.CmToPx();

            orgImage.RenderTransformOrigin = new System.Windows.Point(.5, .5);
            TranslateTransform tt = new TranslateTransform();
            tt.X = x;
            tt.Y = y;
            orgImage.RenderTransform = tt;
        }

        public void onRotate(double mDiff, bool right)
        {
            orgImage.RenderTransformOrigin = new System.Windows.Point(.5, .5);
            if (right)
            {
                RotateTransform trans = new RotateTransform(360 * mDiff);
                orgImage.RenderTransform = trans;
            }
            else
            {
                RotateTransform trans = new RotateTransform(-360 * mDiff);
                orgImage.RenderTransform = trans;
            }
        }

        public void onTilt()
        {
        }

        public void onZoom(double distDelta)
        {
            //double zoom = 0;
            //if (Math.Abs(1 - distDelta) > 1)
            //{
            //    zoom = distDelta > 0 ? 1 : -1;
            //}
            //else if (Math.Abs(1 - distDelta) < 1 && Math.Abs(1 - distDelta) > 0.2)
            //{
            //    zoom = distDelta > 0 ? 1 : -1;
            //}
            orgImage.RenderTransformOrigin = new System.Windows.Point(.5, .5);
            ScaleTransform st = new ScaleTransform();
            st.ScaleX = 1 + distDelta;
            st.ScaleY = 1 + distDelta;
            orgImage.RenderTransform = st;
            //double slidingScalex = imageScaleTransform.ScaleX / 2 * distDelta;
            //double slidingScaley = imageScaleTransform.ScaleY / 2 * distDelta;
            //imageScaleTransform.ScaleX = imageScaleTransform.ScaleY += slidingScalex;

        }

        public void onUpUp(bool isDetected, float confidence)
        {
            if (isDetected) { Cropping(); }
        }

        public void onUpRight(bool isDetected, float confidence)
        {
            if (isDetected)
            {
                if (!isImgLoaded)
                {
                    status_TextBox.Text = "Pick A Photo";
                }

                else if (cropMode == CropMode.None)
                {
                    cropMode = CropMode.RectMode;
                    croppingInit = true;
                    status_TextBox.Text = "Rectangular Mode";
                }

                else if (this.IsCroppingDone)
                {
                    Refresh();
                    cropMode = CropMode.RectMode;
                }

                else if (cropMode == CropMode.PolygonMode)
                {
                    cropMode = CropMode.RectMode;
                    croppingInit = true;
                    isRectCropping = true;
                    status_TextBox.Text = "Rectangular Mode";

                }
            }
        }

        public void onLeftRight(bool isDetected, float confidence)
        {
            if (isDetected) { Cropping(); }
        }

        public void onLeftUp(bool isDetected, float confidence)
        {
            if (isDetected)
            {
                if (!isImgLoaded)
                {
                    status_TextBox.Text = "Pick A Photo";
                }

                else if (cropMode == CropMode.None)
                {
                    cropMode = CropMode.PolygonMode;
                    croppingInit = true;
                    status_TextBox.Text = "Polygon Mode";

                }

                else if (this.IsCroppingDone)
                {
                    Refresh();
                    cropMode = CropMode.PolygonMode;
                }

                else if (cropMode == CropMode.RectMode)
                {
                    cropMode = CropMode.PolygonMode;
                    croppingInit = true;
                    isPolygonCropping = true;
                    status_TextBox.Text = "Polygon Mode";
                }
            }
        }

        public void onTouchdown(bool isDetected, float confidence)
        {
            if (isDetected) { Undo(); }
        }

        public void onStretched(bool isDetected, float confidence)
        {
            if (isDetected) { Refresh(); }
        }

        public void onTurnRight(bool isDetected, float confidence)
        {

        }

        public void onTurnLeft(bool isDetected, float confidence)
        {

        }

        public void onWalkingRight(bool isDetected, float confidence)
        {

        }

        public void onWalkingLeft(bool isDetected, float confidence)
        {

        }
        #endregion    }
    }
}







