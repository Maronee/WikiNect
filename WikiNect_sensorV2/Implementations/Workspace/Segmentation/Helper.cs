using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Windows.Media.Imaging;

using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;

using OpenCvSharp;


using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Net;

using System.Diagnostics;


/*
* Helper Class
*
* Autor: Hesamedin Ghavami Kazzazi
*
* This class includes the help methods like image resizer, Bitmap to IplImage coverter and other converter
* 
*/

namespace Segmentation
{

    /// <summary>
    /// Includes help functions like transformations of different types.
    /// </summary>
    class Helper
    {
        #region Helper.drawTransparentBitmap
        /// <summary>
        /// Creates a transparent bitmap layer by a given size.
        /// </summary>
        /// <param name="width">width of the layer</param>
        /// <param name="height">height of the layer</param>
        /// <returns>fitting layer</returns>
        public static Bitmap drawTransparentBitmap(int width, int height)
        {
            try
            {
                Bitmap transparentImg = new Bitmap(width, height);
                transparentImg.MakeTransparent(System.Drawing.Color.FromArgb(255, 0, 0, 0));
                return transparentImg;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("in drawTransparentBitmap()-method ---> Exception occurred --->' Message: " + ex.Message + "'");
                return null;
            }
        }
        #endregion

        #region Helper.ConvertWpfImageToImage
        /// <summary>
        /// Converts a System.Windows.Controls.Image to System.Drawing.Image
        /// Quelle: http://www.mycsharp.de/wbb2/thread.php?threadid=91456
        /// </summary>
        /// <param name="image">converting System.Windows.Controls.Image </param>
        /// <returns>converted System.Drawing.Image</returns>
        public static System.Drawing.Image ConvertWpfImageToImage(System.Windows.Controls.Image image)
        {
            if (image == null)
                throw new ArgumentNullException("image", "Image darf nicht null sein.");

            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            MemoryStream ms = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
            encoder.Save(ms);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }
        #endregion

        #region Helper.ConvertImageToWpfImage
        /// <summary>
        /// Converts a System.Drawing.Image to System.Windows.Controls.Image
        /// Quelle: http://www.mycsharp.de/wbb2/thread.php?threadid=91456
        /// </summary>
        /// <param name="image">converting System.Drawing.Image</param>
        /// <returns>converted System.Windows.Controls.Image</returns>
        public static System.Windows.Controls.Image ConvertImageToWpfImage(System.Drawing.Image image)
        {
            try
            {
                if (image != null)
                {
                    using (System.Drawing.Bitmap dImg = new System.Drawing.Bitmap(image))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            dImg.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                            System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();

                            bImg.BeginInit();
                            bImg.StreamSource = new MemoryStream(ms.ToArray());
                            bImg.EndInit();

                            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                            img.Source = bImg;

                            return img;
                        }
                    }
                }
                else { return null; }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception '" + ex.Message + "' occurred inside 'ConvertImageToWpfImage()'...");
                return null;
            }
        }
        #endregion

        #region Helper.BitmapToIplImage
        /// <summary>
        /// Convert System.Drawing.Bitmap to OpenCVSharp.IplImage to use in OpenCvSharp methods
        /// Quelle: http://opencv-users.1802565.n2.nabble.com/Convert-IplImage-to-Bitmap-td3784378.html
        /// </summary>
        /// <param name="bitmap">converting System.Drawing.Bitmap</param>
        /// <returns>converted OpenCVSharp.IplImage</returns>
        public static IplImage BitmapToIplImage(Bitmap bitmap)
        {
            IplImage tmp = null;

            System.Drawing.Rectangle bRect = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new Size((int)bitmap.Width, (int)bitmap.Height));
            BitmapData bmData = bitmap.LockBits(bRect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                tmp = Cv.CreateImage(Cv.Size(bitmap.Width, bitmap.Height), BitDepth.U8, 1);
                tmp.ImageData = bmData.Scan0; ;
            }

            else if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                tmp = Cv.CreateImage(Cv.Size(bitmap.Width, bitmap.Height), BitDepth.U8, 3);
                tmp.ImageData = bmData.Scan0; ;
            }

            bitmap.UnlockBits(bmData);

            return tmp;
        }
        #endregion

        #region Helper.IplImageToBitmap
        /// <summary>
        /// Convert OpenCVSharp.IplImage to System.Drawing.Bitmap
        /// Quelle: http://opencv-users.1802565.n2.nabble.com/Convert-IplImage-to-Bitmap-td3784378.html
        /// </summary>
        /// <param name="Ipl">converting OpenCVSharp.IplImage</param>
        /// <returns>converted System.Drawing.Bitmap</returns>
        static Bitmap IplImageToBitmap(IplImage ipl)
        {
            Bitmap bitmap;
            if (ipl.NChannels == 3)
            {
                bitmap = new System.Drawing.Bitmap(ipl.Width, ipl.Height, ipl.WidthStep, System.Drawing.Imaging.PixelFormat.Format24bppRgb, (System.IntPtr)ipl.ImageData);
            }

            else
            {
                bitmap = new System.Drawing.Bitmap(ipl.Width, ipl.Height, ipl.WidthStep, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, (System.IntPtr)ipl.ImageData);
            }
            return bitmap;
        }
        #endregion

        #region Helper.ImageResize
        /// <summary>
        /// Resizes a System.Drawing.Bitmap according to the Height and Width of an System.Windows.Controls.Image
        /// </summary>
        /// <param name="bitmapToResize">System.Darwing.Bitmap to be resized</param>
        /// <param name="imageFrame">System.Windows.Controls.Image to which the bitmap is tailored</param>
        /// <returns>Resized System.Drawing.Bitmap</returns>
        public static System.Drawing.Bitmap ImageResize(System.Drawing.Bitmap bitmapToResize, System.Windows.Controls.Image imageFrame)
        {
          
            // Get the image's original width and height
            int originalWidth = bitmapToResize.Width;
            int originalHeight = bitmapToResize.Height;

            // To preserve the aspect ratio
            float ratioX = (float)imageFrame.Width / (float)originalWidth;
            float ratioY = (float)imageFrame.Height / (float)originalHeight;

            float ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            int resizedWidth = (int)(originalWidth * ratio);
            int resizedHeight = (int)(originalHeight * ratio);

            // Convert other formats (including CMYK) to RGB.
            Bitmap resizedBitmap = new Bitmap(resizedWidth, resizedHeight, PixelFormat.Format24bppRgb);

            // Draws the image in the specified size with quality mode set to HighQuality
            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(bitmapToResize, 0, 0, resizedWidth, resizedHeight);
                return resizedBitmap;
            }
        }
        #endregion

        #region convertIntPointsToPoints
        /// <summary>
        /// Converts IntPoint list to System.Drawing.Point[]
        /// </summary>
        /// <param name="inputList">converting List</param>
        /// <returns>converted System.Drawing.Point[]</returns>
        public System.Drawing.Point[] convertIntPointsToPoints(List<IntPoint> inputList)
        {
            List<IntPoint> intPoints = new List<IntPoint>(inputList);
            int length = intPoints.Count;
            System.Drawing.Point[] points = new System.Drawing.Point[length];
            int count = 0;
            foreach (IntPoint p in intPoints)
            {
                System.Drawing.Point point = new System.Drawing.Point();
                point.X = p.X;
                point.Y = p.Y;

                points[count] = point;
                count++;
            }
            return points;
        }
        #endregion

        #region Helper.BitmapImage2Bitmap
        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        #endregion

    }

 }



