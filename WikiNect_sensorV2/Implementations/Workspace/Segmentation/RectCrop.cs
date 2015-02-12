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

using AForge;
using AForge.Imaging;


namespace Segmentation
{
    class RectCrop
    {

        Bitmap startImage, processImage;
        System.Windows.Controls.Image finalImage, segmentImage, temp_segmentImage;
        private List<System.Windows.Point> clickedPoints = new List<System.Windows.Point>(); //mouse click points to crop image

        /// <summary>
        /// Calculate and indicate the cropped region in rectangular mode
        /// </summary>
        /// <param name="rectCrop_bmp">Source Bitmap</param>
        /// <param name="sourceCrop_img">Main Image Control for indicating the calculated Rectangle</param>
        /// <param name="rectCrop_img">Image Control for showing the cropped region</param>
        /// <param name="rectCrop_pt">selected points</param>
        public RectCrop(Bitmap rectCrop_bmp, System.Windows.Controls.Image sourceCrop_img, System.Windows.Controls.Image rectCrop_img, List<System.Windows.Point> rectCrop_pt) 
        {
            startImage = rectCrop_bmp;
            finalImage = sourceCrop_img;
            segmentImage = rectCrop_img;
            clickedPoints = rectCrop_pt;
            RectCropping();
        }

        private void RectCropping() 
        {
            processImage = startImage;
            //instance of Filter class
            Filter filter = new Filter(processImage);
            //Edge filter
            Bitmap edgeImage = new Bitmap(filter.sobel());

            //List of edge points
            List<IntPoint> edgePoints = new List<IntPoint>();

            if (clickedPoints.Count >= 4)
            {
                foreach (System.Windows.Point wpoint in clickedPoints)
                {
                    IntPoint point = new IntPoint(Convert.ToInt32(wpoint.X), Convert.ToInt32(wpoint.Y));
                    edgePoints.Add(point);
                }
            }
            //Cuts the Segment and adds it the list 
            using (Bitmap segment = filter.cropRect(edgePoints))
            {
                if (segment != null)
                {
                    temp_segmentImage = Helper.ConvertImageToWpfImage(segment);
                    this.segmentImage.Source = temp_segmentImage.Source;
                }
            }

            startImage.Dispose();
            processImage.Dispose();
            edgeImage.Dispose();
            //empties the list of points
            this.clickedPoints.Clear();
            edgePoints.Clear();
        }
    }
}

