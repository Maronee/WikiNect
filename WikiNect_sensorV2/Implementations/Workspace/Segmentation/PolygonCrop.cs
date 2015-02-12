using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Windows.Controls;


using OpenCvSharp;

/*
* PolygonCrop Class
*
* Autor: Hesamedin Ghavami Kazzazi
*
* this class receives the registered points on the main image control and calculated the cropping region accordingly.
* This class utilizes the OpenCvSharp libarary and its methodes to indicate the selected region by points
* 
*/


namespace Segmentation
{
    // Polygan Class    
    class PolygonCrop
    {
        #region Variables
        private List<Point> polygonPoints = new List<Point>(); //mouse click points to crop image
       
        Bitmap sourceBitmap, cropRegion_bmp, refBitmap; // original image bitmap format for using System.Drawing.Graphics Class (sourceBmp_temp)
        Rectangle rect_cropRegion;
        
        System.Windows.Controls.Image croppedImage;
        System.Windows.Controls.Image temp_croppedImage;
        #endregion

        #region Constructor
        /// <summary>
        /// Calculate and indicate the cropped region in polygon mode
        /// </summary>
        /// <param name="polyCrop_bmp">Source Bitmap</param>
        /// <param name="polyCrop_img">Image Control for showing the cropped region</param>
        /// <param name="polyCrop_pt">selected points</param>
        public PolygonCrop(Bitmap polyCrop_bmp, System.Windows.Controls.Image polyCrop_img, List<Point> polyCrop_pt)
        {
            sourceBitmap = polyCrop_bmp;
            polygonPoints = polyCrop_pt;
            croppedImage =  polyCrop_img;
            //Cloning the Bitmap sourceBitmap for furthur usage in OpenCVSharp Method Cv.Mul
            refBitmap = sourceBitmap.Clone(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), sourceBitmap.PixelFormat);
            //Calling the croping method
            PolygonCropping();  
        }
        #endregion

        #region Private Methods
        // Creats two Masks for cropping. One is a Black image with a transparent Polygon part, which is defined in MainWindow
        //and the other one is the unchanged original image    
        private void PolygonCropping()
        {
            Graphics Ga = Graphics.FromImage(sourceBitmap);

            //the black image
            Ga.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height));

            //draw from the last point to first point  
            Ga.DrawLine(new Pen(Color.Black, 3), polygonPoints[polygonPoints.Count - 1], polygonPoints[0]);

            //all of the rgb values of Brush are set to 1, which let all the colors through
            SolidBrush transparentBrush = new SolidBrush(Color.FromArgb(1, 1, 1));

            //Marking the Transparent Polygon using the Brush with RGB Values 1
            Ga.FillClosedCurve(transparentBrush, polygonPoints.ToArray());

            //this method multiply two maskes and save the result in IplImage iplImgCropped.
            //A Black mask with a tranparent polygon part multiplies the unchanged original image equals 
            //only a region of image marked or determined with transparent polygon 
            using (IplImage iplImgCropped = Cv.CreateImage(new CvSize(sourceBitmap.Width, sourceBitmap.Height), BitDepth.U8, 3))
            {
                Cv.Mul(Helper.BitmapToIplImage(refBitmap), Helper.BitmapToIplImage(sourceBitmap), iplImgCropped, 1);

                Calc_CropRegion();

                //using the calculated region of image which includes the drawn polygon to show cropped region part of image
                cropRegion_bmp = iplImgCropped.ToBitmap().Clone(rect_cropRegion, refBitmap.PixelFormat);
            }

            temp_croppedImage = Helper.ConvertImageToWpfImage((System.Drawing.Image)cropRegion_bmp);
            this.croppedImage.Source = temp_croppedImage.Source;
            Ga.Dispose();
            cropRegion_bmp.Dispose();
            sourceBitmap.Dispose();
            refBitmap.Dispose();
            transparentBrush.Dispose();
            this.polygonPoints.Clear();
                      
        }

        //Calculates the region of Image which includes the drawn polygon
        private void Calc_CropRegion()
        {
            int smallestX = sourceBitmap.Width, biggestX = 0, biggestY = 0, smallestY = sourceBitmap.Height;
            for (int i = 0; i < polygonPoints.Count; i++)
            {
                biggestX = Math.Max(biggestX, polygonPoints[i].X);
                smallestX = Math.Min(smallestX, polygonPoints[i].X);

                biggestY = Math.Max(biggestY, polygonPoints[i].Y);
                smallestY = Math.Min(smallestY, polygonPoints[i].Y);
            }

            rect_cropRegion = new Rectangle(smallestX, smallestY, biggestX - smallestX, biggestY - smallestY);
        }
        #endregion

    }
}
