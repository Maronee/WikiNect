using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using Kinect;

/*
* VisualToImage Class
*
* Autor: Hesamedin Ghavami Kazzazi
*
* this class handels solely the save function by extracting a BitmapSource from System.Windows.Controls.Image crpImage 
* and determining the appropriat encoder for saving the Bitmapsource according to the extention of the original file.
* the location of original file (string url) helps this class to determine the extention of the original file and the 
* target location for saving the cropped image. 
*/

namespace Segmentation
{
    class VisualToImage
    {
        #region Variables
        private RenderTargetBitmap croppedRtb;
        private System.Windows.Controls.Image croppedSaved;
        string saveUrl;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of VisualToImage Class. This calls SaveAs method after extracting the BitmapSource
        /// out of System.Windows.Controls.Image crpImage
        /// </summary>
        /// <param name="savingCrpImg">Image Control which indicates the cropped region</param>
        /// <param name="imageSrc">source image file path</param>
        
        public VisualToImage(List<KinImage> savingCrpImg, List<string> fileUrlToSave, string imageSrc, int saveNameCount, int saveExtended)
        {
            for (int i = saveExtended; i < saveNameCount; i++)
            {
                croppedSaved = savingCrpImg[i];
                saveUrl = fileUrlToSave[i];
                croppedRtb = RenderVisual(croppedSaved);
                BitmapEncoder encoderForSaving = SavingEncoder(imageSrc);
                SaveUsingEncoder(croppedRtb, saveUrl, encoderForSaving);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// chooses the appropriat Image Encoder according to the extension of source file 
        /// </summary>
        /// <param name="filePath">String of source file url</param>
        /// <returns>appropriat  System.Windows.Media.Imaging.BitmapEncoder</returns>
        private BitmapEncoder SavingEncoder(String filePath)
        {
            string fileExtension = Path.GetExtension(filePath); // source image file extension

            try
            {
                if (fileExtension == ".Jpeg" || fileExtension == ".JPEG" || fileExtension == ".jpeg" ||
                         fileExtension == ".Jpg" || fileExtension == ".JPG" || fileExtension == ".jpg") //checking, if the extention is Jpeg
                    {

                        return new JpegBitmapEncoder();
                    }

                else if (fileExtension == ".Bmp" || fileExtension == ".BMP" || fileExtension == ".bmp") //checking, if the extention is BMP
                    {

                        return new BmpBitmapEncoder();
                    }

                else if (fileExtension == ".Png" || fileExtension == ".PNG" || fileExtension == ".png") //checking, if the extention is PNG
                {

                    return new PngBitmapEncoder();
                }

                else if (fileExtension == ".Gif" || fileExtension == ".GIF" || fileExtension == ".gif") //checking, if the extention is GIF
                    {

                        return new GifBitmapEncoder();
                    }
              
                else if (fileExtension == ".Tiff" || fileExtension == ".TIFF" || fileExtension == ".tiff") //checking, if the extention is TIFF
                    {

                        return new TiffBitmapEncoder();
                    }
                    else //if none of above, then the extention should be WMP
                    {

                        return new WmpBitmapEncoder();
                    }
            }

            catch
            {
                return new JpegBitmapEncoder();
            }
        }

        static public double DPI
        { get; set; }

        static public PixelFormat PixelFormat
        { get; set; }
        
        // According to the selected image encoder from "SaveAs" method saves the image under the path, which is determined by targetFileName  
        private void SaveUsingEncoder(BitmapSource visual, string fileName, BitmapEncoder encoder)
        {
            BitmapFrame frame = BitmapFrame.Create(croppedRtb);
            encoder.Frames.Add(frame);
           

            //saving the cropped image under targetFileName
            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the save file url using the number of saves
        /// </summary>
        /// <param name="fileName">string of source file url</param>
        /// /// <param name="saveNameCount">int which indicates the number of saves</param>
        /// <returns>Generated file name string</returns>
        public static string SavedUrl(string fileName, int saveNameCount, int crpPanelRefreshCount, int saveNameExtend)
        {
            string strg_saveNameCount = saveNameCount.ToString();
            string strg_nameExtendI = crpPanelRefreshCount.ToString();
            string strg_nameExtendII = saveNameExtend.ToString();
            //get only the name of source file 
            string fn = Path.GetFileNameWithoutExtension(fileName);
            //get only the extention of source file
            string ext = Path.GetExtension(fileName);
            //get the current working directory 
            string path = Directory.GetCurrentDirectory();
            //get the directory two level above of Debug directory, which results "wherever\Segmentation\Segmentation"
            var parent = Directory.GetParent(Directory.GetParent(path).ToString()).ToString();
            //creating a relative path with the aid of above parameters(fn, ext, parent) 
            string targetFileName = @"Implementions/Workspace/Segmentation/Cropped/" + fn + "_Cropped" + crpPanelRefreshCount + strg_nameExtendII + strg_saveNameCount + ext;

            return targetFileName;
        }

        /// <summary>
        /// Extract the image from a System.Windows.Controls.Image
        /// </summary>
        /// <param name="imageToRender">System.Windows.Controls.Image includes the image which is to be extracted</param>
        /// <returns>Rendered  System.Windows.Media.Imaging.RenderTargetBitmap</returns>
        public static RenderTargetBitmap RenderVisual(System.Windows.Controls.Image imageToRender)
        {
            DPI = 96;
            PixelFormat = PixelFormats.Default;
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(imageToRender.Source, new Rect(0, 0, imageToRender.Source.Width, imageToRender.Source.Height));
            drawingContext.Close();
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)imageToRender.Source.Width, (int)imageToRender.Source.Height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);

            return rtb;
        }
        #endregion

    }
}
