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

using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

using Kinect;
using DataConnection;
using DataStore;
using WikiNectLayout.Implementions.Xamls;


/*
* MainWindow
*
* Autor: Hesamedin Ghavami Kazzazi
*
* this class defines the GUI and its ways of interaction. This class also calls other class whenever they are needed.
* 
*/

namespace Segmentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SegmentationMainWindow : Window, KinectStartUp // should inherit from KinectStartUp
    {

        #region Variables
        private KinectSensorChooser sensorChooser;
        string url, temp_url; //Path string of image file
        //Bitmaps, which will be painted or marked with points and Lines by the use of System.Drawing.Graphics
        private System.Drawing.Bitmap startImage, startImage_unchanged, bitmapTmp_undo;
        //Generic Lists for holding both System.Windows.Points and System.Drawing.Points in CropMode.PolygonMode   
        private List<System.Windows.Point> mouseClicked = new List<System.Windows.Point>(); //mouse click points to crop image
        private List<System.Drawing.Point> temp_polygonPoint = new List<System.Drawing.Point>(); //mouse click points to crop image
        //register and capture mouse clicks as points. These points builds the generic lists 
        private System.Windows.Point wpftemp_point;
        private System.Drawing.Point bmptemp_point;
        //counter for rigistered points by clicking the image control, undo counter and  save counter 
        private int wpfPt_Count, undocount = 0, saveCount = 0;
        //Cropping Modes
        private enum CropMode { RectMode, PolygonMode, None };
        private CropMode cropMode;
        //Graphics made from Bitmap startImage. g is the main Graphic and temp_g for undo function 
        private Graphics g, temp_g;
        //pen and brush for drawing points and lines
        private System.Drawing.Pen a = new System.Drawing.Pen(System.Drawing.Color.Red, 4);
        private SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
        //Image Control. After finishing the manipulation of bitmaps they will be converted to this image control
        private System.Windows.Controls.Image finalImage;
        //Cropping status
        private bool croppingDone, croppingInit, isPolygonCropping, isRectCropping;
        //if an image is selected
        private bool imgIsLoaded;
        #endregion

        #region MainWindow
        public SegmentationMainWindow()
        {
            InitializeComponent();
            cropMode = CropMode.None;
            imgIsLoaded = false;
            croppingInit = false;
            croppingDone = false;
            isPolygonCropping = false;
            orgImage.PointMarked += new ImageClickedEventHandler(KinectDrawing);
            orgImage.ImagePressed += new ImagePressedEventHandler(ImagePressToCrop);
            Loaded += windowLoaded;
        }
        #endregion

        #region Kinect
        /// <summary>
        /// On Window_Loaded initiate the Kinect sensor.
        /// </summary>
        private void windowLoaded(object sender, RoutedEventArgs e)
        {
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();
        }

        public void SensorChooserOnKinectChanged(object sender, Microsoft.Kinect.Toolkit.KinectChangedEventArgs args)
        {
            bool error = false;

            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException) { error = true; }
            }

            if (args.NewSensor != null)
            {

                try
                {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();
                    try
                    {
                        args.NewSensor.DepthStream.Range = DepthRange.Near;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                        args.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
                    }
                    catch (InvalidOperationException)
                    {
                        args.NewSensor.DepthStream.Range = DepthRange.Default;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                        error = true;
                    }
                }
                catch (InvalidOperationException) 
                {
                    error = true;
                }
            }
            else
            {
                error = true;
            }
            if (!error)
            {
                // Bindet den Sensor an die Region
                kinectRegion.KinectSensor = args.NewSensor;
            }
        }

        #endregion

        #region Kinect Private Methods
        //Grip on System.Windows.Controls.Image orgImage marked points on grip position
        private void KinectDrawing(object sender, HandPointerEventArgs e)
        {
            orgImage = (KinImage)sender;
            if (cropMode == CropMode.PolygonMode && !croppingDone)
            {
                isPolygonCropping = true;
                //register the position of Mouse Clicks
                wpftemp_point = e.HandPointer.GetPosition(this.orgImage);
                //Converting System.Windows.Point to System.Drawing.Point
                int x = Convert.ToInt32(wpftemp_point.X);
                int y = Convert.ToInt32(wpftemp_point.Y);

                //System.Drawing.Point for PolygonCrop Class in order to calculate Croping Region 
                bmptemp_point = new System.Drawing.Point(x, y);
                wpfPt_Count = mouseClicked.Count;
                //System.Windows.Point Container
                mouseClicked.Add(wpftemp_point);
                //System.Drawing.Point which will be passed to PolygonCrop Class
                temp_polygonPoint.Add(bmptemp_point);


                //looping to draw the points and the lines between the points
                for (int i = 0; i < mouseClicked.Count; i++)
                {

                    //Draw Points
                    g.FillEllipse(brush, (temp_polygonPoint[i].X) - 5, (temp_polygonPoint[i].Y) - 5, 15, 15);


                    //Draw Lines between Points if the number of Points are more than one
                    if (mouseClicked.Count > 1)
                    {
                        try
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.DrawLine(a, temp_polygonPoint[mouseClicked.Count - 2], temp_polygonPoint[mouseClicked.Count - 1]);
                        }
                        catch (Exception ex)
                        {
                            this.Refresh();
                            MessageBox.Show("Program has encountered an unexpected error and has to be reset." + ex);
                        }

                    }

                }

                status_TextBox.Text = "Polygon Mode";
                finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage);
                this.orgImage.Source = finalImage.Source;

            }

            else if (cropMode == CropMode.RectMode && !croppingDone)
            {
                isRectCropping = true;
                //register the position of Mouse Clicks
                wpftemp_point = e.HandPointer.GetPosition(this.orgImage);
                //Converting System.Windows.Point to System.Drawing.Point
                int x = Convert.ToInt32(wpftemp_point.X);
                int y = Convert.ToInt32(wpftemp_point.Y);

                //System.Drawing.Point for PolygonCrop Class in order to calculate Croping Region 
                bmptemp_point = new System.Drawing.Point(x, y);
                wpfPt_Count = mouseClicked.Count;
                //System.Windows.Point Container
                mouseClicked.Add(wpftemp_point);
                //System.Drawing.Point which will be passed to PolygonCrop Class
                temp_polygonPoint.Add(bmptemp_point);

                //Graphics
                for (int i = 0; i < mouseClicked.Count; i++)
                {
                    //Draw Points
                    g.FillEllipse(brush, (temp_polygonPoint[i].X) - 5, (temp_polygonPoint[i].Y) - 5, 15, 15);
                }

                status_TextBox.Text = "Rectangular Mode";
                finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage);
                this.orgImage.Source = finalImage.Source;

            }

            else if (cropMode == CropMode.None)
            {
                //MessageBox.Show("Please choose a Segmentation Mode!", "Segmentation Mode");
                status_TextBox.Text = "Select Cropping Mode";

            }
        }
        //Pressing on System.Windows.Control.Image orgImage does the cropping like pressing the crop button "cropBtn"  
        private void ImagePressToCrop(object sender, HandPointerEventArgs e) 
        {
            orgImage = (KinImage)sender;
            if (cropMode == CropMode.PolygonMode)
            {
                if (isPolygonCropping && mouseClicked.Count > 2)
                {
                    g.Dispose();
                    finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage_unchanged);
                    orgImage.Source = finalImage.Source;
                    PolygonCrop polygonCrop = new PolygonCrop(startImage_unchanged, crpImage, temp_polygonPoint);
                    croppingDone = true;
                    isPolygonCropping = false;
                    this.startImage.Dispose();
                    this.startImage_unchanged.Dispose();

                }

                else if (isPolygonCropping && mouseClicked.Count < 3)
                {
                    //MessageBox.Show("At least Three points are required for Polygon", "Insufficient Polygon Points");
                    status_TextBox.Text = "At Least Three Points";
                }

                else
                {
                    //MessageBox.Show("At least Three points are required for Polygon", "No Polygon Points");
                    status_TextBox.Text = "No Points Marked";
                }

            }

            else if (cropMode == CropMode.RectMode)
            {
                if (isRectCropping && mouseClicked.Count > 3)
                {
                    g.Dispose();
                    finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage_unchanged);
                    orgImage.Source = finalImage.Source;
                    RectCrop rectCrop = new RectCrop(startImage_unchanged, orgImage, crpImage, mouseClicked);
                    croppingDone = true;
                    isRectCropping = false;
                    this.startImage.Dispose();
                    this.startImage_unchanged.Dispose();
                }

                else if (isRectCropping && mouseClicked.Count < 4)
                {
                    //MessageBox.Show("At least Three points are required for Polygon", "Insufficient Polygon Points");
                    status_TextBox.Text = "At Least four Points";
                }
                else
                {
                    status_TextBox.Text = "No Points Marked";
                }
            }

            else if (!imgIsLoaded)
            {
                //MessageBox.Show("Please choose a picture and your desired region to crop in rectangular mode first","No Picture");
                status_TextBox.Text = "Pick A Photo";
            }

            else if (cropMode == CropMode.None)
            {
                //MessageBox.Show("Please choose the rectangular mode and choos your desired region then hit this button again. This button is not needed in polygon mode ","Cutting");
                status_TextBox.Text = "Select Cropping Mode";
            }
        }
        
        #endregion

        #region Private Methodes
        //Refresh botth orgImage and crpImag Image Controls (Restore the Progaramm's Start Position ) 
        private void FirstRefreshImg()
        {
            using (Bitmap temp_startImage = new Bitmap(url))
            {
                if (temp_startImage.Height > orgImage.Height || temp_startImage.Width > orgImage.Width ||   // checks if height and width of the original System.Drawing.Bitmap startImage 
                    temp_startImage.Height < orgImage.Height || temp_startImage.Width < orgImage.Width)     // doesn't match the System.Windows.Controls.Image orgImage then calls the Helper.ImageResize
                {

                    startImage = Helper.ImageResize(temp_startImage, orgImage);
                    finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage);
                    this.orgImage.Source = finalImage.Source;
                    startImage_unchanged = startImage.Clone(new Rectangle(0, 0, startImage.Width, startImage.Height), startImage.PixelFormat);
                    //creating Graphics from Bitmap bitmap_temp (Working with a copy of startImage) in order to Draw Points and Lines on it
                    g = Graphics.FromImage(startImage);
                    crpImage.Source = null;
                    status_TextBox.Clear();
                    temp_startImage.Dispose();
                }
                else      // if height and width of original Bitmap matches the orgImage there is no need to resize the Bitmap
                {
                    startImage = new Bitmap(url);
                    finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage);
                    this.orgImage.Source = finalImage.Source;
                    startImage_unchanged = startImage.Clone(new Rectangle(0, 0, startImage.Width, startImage.Height), startImage.PixelFormat);
                    //creating Graphics from Bitmap bitmap_temp (Working with a copy of startImage) in order to Draw Points and Lines on it
                    g = Graphics.FromImage(startImage);
                    crpImage.Source = null;
                    status_TextBox.Clear();
                }
            }
        }


        //On Image Control(orgImage) Mouse Event Handler
        public void OrgImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (cropMode == CropMode.PolygonMode && !croppingDone)
            {
                isPolygonCropping = true;
                //register the position of Mouse Clicks
                wpftemp_point = e.GetPosition(orgImage);
                //Converting System.Windows.Point to System.Drawing.Point
                int x = Convert.ToInt32(wpftemp_point.X);
                int y = Convert.ToInt32(wpftemp_point.Y);

                //System.Drawing.Point for PolygonCrop Class in order to calculate Croping Region 
                bmptemp_point = new System.Drawing.Point(x, y);
                wpfPt_Count = mouseClicked.Count;
                //System.Windows.Point Container
                mouseClicked.Add(wpftemp_point);
                //System.Drawing.Point which will be passed to PolygonCrop Class
                temp_polygonPoint.Add(bmptemp_point);


                //looping to draw the points and the lines between the points
                for (int i = 0; i < mouseClicked.Count; i++)
                {

                    //Draw Points
                    g.FillEllipse(brush, (temp_polygonPoint[i].X) - 5, (temp_polygonPoint[i].Y) - 5, 15, 15);


                    //Draw Lines between Points if the number of Points are more than one
                    if (mouseClicked.Count > 1)
                    {
                        try
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.DrawLine(a, temp_polygonPoint[mouseClicked.Count - 2], temp_polygonPoint[mouseClicked.Count - 1]);
                        }
                        catch (Exception ex)
                        {
                            this.Refresh();
                            MessageBox.Show("Program has encountered an unexpected error and has to be reset." + ex);
                        }

                    }

                }

                status_TextBox.Text = "Polygon Mode";
                finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage);
                this.orgImage.Source = finalImage.Source;

            }

            else if (cropMode == CropMode.RectMode && !croppingDone)
            {
                isRectCropping = true;
                //register the position of Mouse Clicks
                wpftemp_point = e.GetPosition(orgImage);
                //Converting System.Windows.Point to System.Drawing.Point
                int x = Convert.ToInt32(wpftemp_point.X);
                int y = Convert.ToInt32(wpftemp_point.Y);

                //System.Drawing.Point for PolygonCrop Class in order to calculate Croping Region 
                bmptemp_point = new System.Drawing.Point(x, y);
                wpfPt_Count = mouseClicked.Count;
                //System.Windows.Point Container
                mouseClicked.Add(wpftemp_point);
                //System.Drawing.Point which will be passed to PolygonCrop Class
                temp_polygonPoint.Add(bmptemp_point);

                //Graphics
                for (int i = 0; i < mouseClicked.Count; i++)
                {

                    //Draw Points
                    g.FillEllipse(brush, (temp_polygonPoint[i].X) - 5, (temp_polygonPoint[i].Y) - 5, 15, 15);


                    //Draw Lines between Points if the number of Points are more than one
                    if (mouseClicked.Count > 1)
                    {
                        try
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.DrawLine(a, temp_polygonPoint[mouseClicked.Count - 2], temp_polygonPoint[mouseClicked.Count - 1]);
                        }
                        catch (Exception ex)
                        {
                            this.Refresh();
                            MessageBox.Show("Program has encountered an unexpected error and has to be reset." + ex);
                        }

                    }

                }

                status_TextBox.Text = "Rectangular Mode";
                finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage);
                this.orgImage.Source = finalImage.Source;

            }

            else if (cropMode == CropMode.None)
            {
                //MessageBox.Show("Please choose a Segmentation Mode!", "Segmentation Mode");
                status_TextBox.Text = "Select Cropping Mode";

            }
        }

        // Save. if an image is loade and a region is selected this calls the VisualToImage Class 
        private void Save()
        {
            if (!imgIsLoaded)
            {
                //MessageBox.Show("You did not pick a source picture to select a region/segment from", "No Picture");
                status_TextBox.Text = "Pick A Photo";
            }
            else if (imgIsLoaded == true && crpImage.Source == null)
            {
                //MessageBox.Show("No region/segment is determined to save", "Empty Region");
                status_TextBox.Text = "Nothing to Save";
            }
            else
            {
                saveCount++;
                VisualToImage vti = new VisualToImage(crpImage, url, saveCount);
                status_TextBox.Text = "SAVED";

            }
        }

        //Polygon Undo: By Clicking undo the last Line will be deleted 
        private void Undo_PolygonDraw()
        {
            undocount++;


            try
            {
                //releasing the last Point in both Lists of Points
                mouseClicked.RemoveRange(wpfPt_Count, 1);
                temp_polygonPoint.RemoveRange(wpfPt_Count, 1);
                wpfPt_Count--;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }

            FirstRefreshImg();
            g.Dispose(); // we have to dipose g object not to blow up ram

            bitmapTmp_undo = startImage; //making a copy of source image
            try
            {
                temp_g = Graphics.FromImage(bitmapTmp_undo);
            }
            catch
            {
                temp_g.Dispose();
            }

            for (int i = 0; i < mouseClicked.Count; i++)
            {
                temp_g.FillEllipse(brush, (temp_polygonPoint[i].X) - 2, (temp_polygonPoint[i].Y) - 2, 7, 7);

                for (int j = 0; j < mouseClicked.Count - 1; j++)
                {
                    try
                    {

                        temp_g.SmoothingMode = SmoothingMode.AntiAlias;
                        temp_g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        temp_g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        temp_g.DrawLine(a, temp_polygonPoint[j].X, temp_polygonPoint[j].Y, temp_polygonPoint[j + 1].X, temp_polygonPoint[j + 1].Y);
                    }
                    catch
                    {
                        temp_g.Dispose();
                        MessageBox.Show("Program has encountered an unexpected error and has to be reset.");
                    }
                }
            }
            startImage = bitmapTmp_undo;
            finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage);
            orgImage.Source = finalImage.Source;
            g = Graphics.FromImage(startImage);
            temp_g.Dispose();

        }

        //Undo Function, which will be called by Undo Button
        private void Undo()
        {
            if (!imgIsLoaded)
            {
                status_TextBox.Text = "Pick A Photo";
            }

            else if (cropMode == CropMode.PolygonMode)
            {
                if (isPolygonCropping && !croppingDone)
                {
                    if (mouseClicked.Count > 0)
                    {
                        Undo_PolygonDraw();
                        status_TextBox.Text = "Undoing";
                    }
                    else
                    {
                        //MessageBox.Show("In order to be able to undo something, something must be first done", "Undo-Nothing-To-Do");
                        status_TextBox.Text = "Undo-Nothing-To-Do";
                    }
                }

                else if (croppingDone)
                {
                    if (MessageBox.Show("Do you want to start a new segmentation?", "Segmentation Completed", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Refresh();
                    }
                }

                else if (!isPolygonCropping && !croppingDone)
                {
                    status_TextBox.Text = "Undo-Nothing-To-Do";
                }
            }

            else if (cropMode == CropMode.RectMode)
            {
                MessageBox.Show("The implementation of rectangular segmenting gives the user the Undo control", "Regtangular Segmentation");
                status_TextBox.Text = "Rectangular Mode";
            }


            else
            {
                //MessageBox.Show("Please choose a Segmentation Mode and start segmenting", "Segmentaion Mode");
                status_TextBox.Text = "Select Cropping Mode";
            }
        }

        //Refresh Function, which will be called by Refresh Button
        private void Refresh()
        {

            if (!imgIsLoaded)
            {
                //MessageBox.Show("Please choose a picture first", "No Picture");
                status_TextBox.Text = "Nothing To Refresh";
            }

            else if (cropMode == CropMode.PolygonMode || cropMode == CropMode.RectMode)
            {
                this.g.Dispose();
                this.startImage.Dispose();
                this.startImage_unchanged.Dispose();
                croppingDone = false;
                isPolygonCropping = false;
                isRectCropping = false;
                croppingInit = false;
                mouseClicked.Clear();
                temp_polygonPoint.Clear();
                FirstRefreshImg();
                status_TextBox.Text = "REFRESHED";

            }
            else if (cropMode == CropMode.PolygonMode && !isPolygonCropping || cropMode == CropMode.RectMode && !isRectCropping)
            {
                croppingInit = false;
                status_TextBox.Text = "REFRESHED";
            }

            else if (cropMode == CropMode.None && !croppingInit)
            {
                status_TextBox.Text = "REFRESHED";
            }

        }
        #endregion

        #region Window Size changed
        //maintains the cropping region marked with rectangle when the window size is changed
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (cropMode == CropMode.RectMode && croppingInit)
            {
                //rc.RefreshCropImage();
            }
        }

        #endregion

        #region Routed Events and Butttons
        //Load Koala Picture
        private void KoalaBtnOnClick(object sender, RoutedEventArgs e)
        {
            temp_url = @"Pictures/Koala.jpg";
            if (!imgIsLoaded) // No image is yet loaded onto System.Windows.Controls.Image orgImage 
            {
                url = temp_url;
                imgIsLoaded = true;
                FirstRefreshImg();
            }
            else if (temp_url == url) // if the newly loaded image is the same image the save counter will not be reseted
            {
                Refresh();
            }
            else if (temp_url != url) // if the newly loaded image is the same image the save counter will be reseted
            {
                url = temp_url;
                saveCount = 0;
                Refresh();
            }
        }
        //Load Jellyfish Picture
        private void JellyBtnOnClick(object sender, RoutedEventArgs e)
        {
            temp_url = @"Pictures/Jellyfish.jpg";
            if (!imgIsLoaded) // No image is yet loaded onto System.Windows.Controls.Image orgImage
            {
                url = temp_url;
                imgIsLoaded = true;
                FirstRefreshImg();
            }
            else if (temp_url == url) // if the newly loaded image is the same image the save counter will not be reseted
            {
                Refresh();
            }
            else if (temp_url != url) // if the newly loaded image is the same image the save counter will be reseted
            {
                url = temp_url;
                saveCount = 0;
                Refresh();
            }
        }
        //Load Desert Picture
        private void DesertBtnOnClick(object sender, RoutedEventArgs e)
        {
            temp_url = @"Pictures/Desert.jpg";
            if (!imgIsLoaded) // No image is yet loaded onto System.Windows.Controls.Image orgImage
            {
                url = temp_url;
                imgIsLoaded = true;
                FirstRefreshImg();
            }
            else if (temp_url == url)  // if the newly loaded image is the same image the save counter will not be reseted
            {
                Refresh();
            }
            else if (temp_url != url) // if the newly loaded image is not the same image the save counter will be reseted
            {
                url = temp_url;
                saveCount = 0;
                Refresh();
            }
        }
        //Initiate Rectangular Cropping
        private void RectBtnOnClick(object sender, RoutedEventArgs e)
        {
            if (!imgIsLoaded)
            {
                status_TextBox.Text = "Pick A Photo";
            }

            else if (cropMode == CropMode.None)
            {
                cropMode = CropMode.RectMode;
                croppingInit = true;
                status_TextBox.Text = "Rectangular Mode";
            }

            else if (cropMode == CropMode.PolygonMode)
            {

                cropMode = CropMode.RectMode;
                Refresh();
                croppingInit = true; 
                status_TextBox.Text = "Rectangular Mode";
                
            }

            else if (croppingDone || isRectCropping)
            {
                Refresh();
            }
        }
        //Initiate Polygon Cropping
        private void polygonBtnOnClick(object sender, RoutedEventArgs e)
        {
            if (!imgIsLoaded)
            {
                status_TextBox.Text = "Pick A Photo";
            }

            else if (cropMode == CropMode.None)
            {
                cropMode = CropMode.PolygonMode;
                croppingInit = true;
                status_TextBox.Text = "Polygon Mode";
                startImage_unchanged = startImage.Clone(new Rectangle(0, 0, startImage.Width, startImage.Height), startImage.PixelFormat);
            }

            else if (cropMode == CropMode.RectMode)
            {
                cropMode = CropMode.PolygonMode;
                Refresh();
                croppingInit = true;
                status_TextBox.Text = "Polygon Mode";
            }

            else if (croppingDone || isPolygonCropping)
            {
                Refresh();
            }
        }

        //After determinig the Nodes, which defines the polygon, calls the PolygonCrop class to calculate the cropping region
        private void CropBtnOnClick(object sender, RoutedEventArgs e)
        {
            if (cropMode == CropMode.PolygonMode)
            {
                if (isPolygonCropping && mouseClicked.Count > 2)
                {
                    g.Dispose();
                    finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage_unchanged);
                    orgImage.Source = finalImage.Source;
                    PolygonCrop polygonCrop = new PolygonCrop(startImage_unchanged, crpImage, temp_polygonPoint); 
                    croppingDone = true;
                    isPolygonCropping = false;
                    this.startImage.Dispose();
                    this.startImage_unchanged.Dispose();

                }

                else if (isPolygonCropping && mouseClicked.Count < 3)
                {
                    //MessageBox.Show("At least Three points are required for Polygon", "Insufficient Polygon Points");
                    status_TextBox.Text = "At Least Three Points";
                }

                else
                {
                    //MessageBox.Show("At least Three points are required for Polygon", "No Polygon Points");
                    status_TextBox.Text = "No Points Marked";
                }

            }

            else if (cropMode == CropMode.RectMode)
            {
                if (isRectCropping && mouseClicked.Count > 3)
                {
                    g.Dispose();
                    finalImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)startImage_unchanged);
                    orgImage.Source = finalImage.Source;
                    RectCrop rectCrop = new RectCrop(startImage_unchanged, orgImage, crpImage, mouseClicked);
                    croppingDone = true;
                    isRectCropping = false;
                    this.startImage.Dispose();
                    this.startImage_unchanged.Dispose();
                }

                else if (isRectCropping && mouseClicked.Count < 4)
                {
                    //MessageBox.Show("At least Three points are required for Polygon", "Insufficient Polygon Points");
                    status_TextBox.Text = "At Least four Points";
                }
                else
                {
                    status_TextBox.Text = "No Points Marked";
                }
            }

            else if (!imgIsLoaded)
            {
                //MessageBox.Show("Please choose a picture and your desired region to crop in rectangular mode first","No Picture");
                status_TextBox.Text = "Pick A Photo";
            }

            else if (cropMode == CropMode.None)
            {
                //MessageBox.Show("Please choose the rectangular mode and choos your desired region then hit this button again. This button is not needed in polygon mode ","Cutting");
                status_TextBox.Text = "Select Cropping Mode";
            }


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

        //Help
        private void HelpBtnOnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Picture Selection : Picture Buttons must be pressed to be selected." + "\n" +"\n"+
                            "Control Buttons : Other Buttons can be either pressed or griped to be selected." + "\n" +
                            "Mark Gripping Points : Griping the image will leave cropping points on the position of grip." + "\n" +
                            "Cropping : After marking the cropping points, image cropping can be completed either by simply press the image or press/gripping the crop button", "Help");
        }

        //Close Button 
        private void CloseBtnOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.sensorChooser.Stop();
            }
            catch (NullReferenceException)
            {

                Console.WriteLine("Errrorrrr");
            }

            mouseClicked.Clear();
            temp_polygonPoint.Clear();
            a.Dispose();
            brush.Dispose();
            Application.Current.Shutdown();
        }
        //Back Button
        private void BackBtnOnClick(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            try
            {
                if (ns.CanGoBack)
                {
                    ns.GoBack();
                }
                else
                {
                    MessageBox.Show("No Window/Page to Navigate Backward", "Navigation");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Navigating Encountered Error: " + exp);
            }
        }

        public void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.sensorChooser.Stop();
            }
            catch (NullReferenceException)
            {

                Console.WriteLine("Errrorrrr");
            }

            mouseClicked.Clear();
            temp_polygonPoint.Clear();
            a.Dispose();
            brush.Dispose();
            Application.Current.Shutdown();
        }
        #endregion
    }
}

      





