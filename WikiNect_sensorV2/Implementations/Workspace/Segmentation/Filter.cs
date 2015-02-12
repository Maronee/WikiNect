using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

using System.Drawing;

using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge;

namespace Segmentation
{
    /// <summary>
    /// Includes segmentation filters and methods.
    /// </summary>
    public class Filter
    {
        //Bitmap Bilddatei
        Bitmap bitmapImage;

        //Helper 
        Helper helper = new Helper();

        //Homogenity - Edge
        HomogenityEdgeDetector HomoFilter = new HomogenityEdgeDetector();

        //Euclicean Filter - Color
        EuclideanColorFiltering EuclideanFilter = new EuclideanColorFiltering();

        //Sobel - Edge
        SobelEdgeDetector SobelFilter = new SobelEdgeDetector();

        //Corner Detection
        MoravecCornersDetector MCDFilter = new MoravecCornersDetector();
        SusanCornersDetector SCDFilter = new SusanCornersDetector();

        //Grayscale
        Grayscale GrayFilter = new Grayscale(0.3125, 0.6154, 0.0721);

        //RGB
        GrayscaleToRGB RGBFilter = new GrayscaleToRGB();

        /// <summary>
        /// Initialize Filter class.
        /// </summary>
        /// <param name="image">image for segmentation</param>
        public Filter(Bitmap image)
        {
            this.bitmapImage = image;
        }

        /// <summary>
        /// Creates a image of all detected edges with the help of the sobel filter.
        /// </summary>
        /// <returns>edge detected Bitmap image</returns>
        public Bitmap sobel()
        {
            Bitmap grayImage = GrayFilter.Apply(this.bitmapImage);
            return SobelFilter.Apply(grayImage);
        }

        /// <summary>
        /// Creates a image filtered by a given color
        /// </summary>
        /// <param name="r">r-value</param>
        /// <param name="g">g-value</param>
        /// <param name="b">b-value</param>
        /// <returns>Bitmap image</returns>
        public Bitmap color(byte r, byte g, byte b)
        {
            EuclideanFilter.CenterColor = new RGB(r, g, b);
            EuclideanFilter.Radius = 50;
            return EuclideanFilter.Apply(this.bitmapImage);
        }

        /// <summary>
        /// Creates a rectangle of a given list of points.
        /// </summary>
        /// <param name="edgeCorner">given list of points</param>
        /// <returns>Bitmap of the rectangle</returns>
        public Bitmap cropRect(List<IntPoint> edgeCorner)
        {
            //adds all found edge points to a list
            List<IntPoint> corners;

            if (edgeCorner == null)
            {
                //edge detection
                Bitmap grayImage = GrayFilter.Apply(this.bitmapImage);
                Bitmap edgeImage = SobelFilter.Apply(grayImage);
                corners = MCDFilter.ProcessImage(edgeImage);
            }
            else
            {
                corners = edgeCorner;
            }

            //check list items
            if (corners.Count != 0)
            {
                //rectangle
                int MinX = corners[0].X;
                int MinY = corners[0].Y;
                int MaxX = 0;
                int MaxY = 0;
                int width;
                int height;

                //visit each edge
                foreach (IntPoint corner in corners)
                {
                    if (corner.X < MinX)
                    {
                        MinX = corner.X;
                    }
                    if (corner.X > MaxX)
                    {
                        MaxX = corner.X;
                    }
                    if (corner.Y < MinY)
                    {
                        MinY = corner.Y;
                    }
                    if (corner.Y > MaxY)
                    {
                        MaxY = corner.Y;
                    }
                }
                width = MaxX - MinX;
                height = MaxY - MinY;

                //check values
                if ((width > 0) && (height > 0))
                {
                    //crop to segment
                    Crop CutFilter = new Crop(new Rectangle(MinX, MinY, width, height));
                    return CutFilter.Apply(this.bitmapImage);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a rectangle from the given list of points.
        /// </summary>
        /// <param name="corners">list of given points</param>
        /// <returns>rectangle</returns>
        public Rectangle getRect(List<IntPoint> corners)
        {
            //standard rectangle
            Rectangle rect = new Rectangle(1, 1, 0, 0);

            //rectangle
            int MinX = corners[0].X;
            int MinY = corners[0].Y;
            int MaxX = 0;
            int MaxY = 0;
            int width;
            int height;

            //visit each edge
            foreach (IntPoint corner in corners)
            {
                if (corner.X < MinX)
                {
                    MinX = corner.X;
                }
                if (corner.X > MaxX)
                {
                    MaxX = corner.X;
                }
                if (corner.Y < MinY)
                {
                    MinY = corner.Y;
                }
                if (corner.Y > MaxY)
                {
                    MaxY = corner.Y;
                }
            }
            width = MaxX - MinX;
            height = MaxY - MinY;

            //Crop to segment
            return new Rectangle(MinX, MinY, Math.Abs(width), Math.Abs(height));

        }

        /// <summary>
        /// Creates a black/white copy of the detected polygon and crops the 
        /// image to this section in shape of a rectangle.
        /// </summary>
        /// <param name="points">list of points of the detected polygon</param>
        /// <returns>black/white image</returns>
        public Bitmap copyPoly(List<IntPoint> points)
        {
            //Helper instance
            Helper helper = new Helper();

            //Copy segment size
            Bitmap copyRect = new Bitmap(getRect(points).Width, getRect(points).Height);

            //transform each point to the segment scaling
            int MinX = getRect(points).X; //minimal X value = left,top point
            int MinY = getRect(points).Y;
            //add all points to localPoints
            List<IntPoint> localPoints = new List<IntPoint>();
            foreach (IntPoint p in points)
            {
                int xValue = p.X - MinX;
                int yValue = p.Y - MinY;
                localPoints.Add(new IntPoint(xValue, yValue));
            }

            //fill copyRect image black
            Graphics c = Graphics.FromImage(copyRect);
            SolidBrush brushRect = new SolidBrush(Color.Black);
            c.FillRectangle(brushRect, 0, 0, copyRect.Width, copyRect.Height);
            //segment will be added as a white polygonal shape
            System.Drawing.Point[] polyPoints = helper.convertIntPointsToPoints(localPoints);
            Graphics poly = Graphics.FromImage(copyRect);
            SolidBrush brushPoly = new SolidBrush(Color.White);
            poly.FillPolygon(brushPoly, polyPoints);

            //return shape of the segment
            return copyRect;

        }

        /// <summary>
        /// Makes a smooth edge for each cropped polygon.
        /// </summary>
        /// <param name="segment">created polygonal segment</param>
        /// <param name="points">edge points of the polygon</param>
        /// <returns>polygon with smooth edge</returns>
        public Bitmap cleanPolyEdge(Bitmap segment, List<IntPoint> points)
        {
            //transform each point to the local section
            int MinX = getRect(points).X; //minimal X value = left,top point
            int MinY = getRect(points).Y;
            //segment will be added as a white polygonal shape
            List<IntPoint> localPoints = new List<IntPoint>();
            foreach (IntPoint p in points)
            {
                int xValue = p.X - MinX;
                int yValue = p.Y - MinY;
                localPoints.Add(new IntPoint(xValue, yValue));
            }

            //smooth segments edges
            Graphics s = Graphics.FromImage(segment);
            System.Drawing.Point[] pPoints = helper.convertIntPointsToPoints(localPoints);
            s.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            System.Drawing.Pen penPoly = new System.Drawing.Pen(System.Drawing.Color.White, 3);
            s.DrawPolygon(penPoly, pPoints);
            s.Dispose();
            penPoly.Dispose();

            return segment;
        }

        /// <summary>
        /// For each point of clickedPoints: look up where a edge will be hit in edgeImage.
        /// Searching in eight different directions for the next edge.
        /// </summary>
        /// <param name="clickedPoints">List of current points</param>
        /// <param name="edgeImage">image of all detected edges</param>
        /// <returns>list of all points detected</returns>
        public List<IntPoint> getEdgePoints(List<System.Windows.Point> clickedPoints, Bitmap edgeImage)
        {
            //threshold
            double value = 0.5;

            //list of points
            List<IntPoint> edgePoints = new List<IntPoint>();

            if (clickedPoints.Count >= 1)
            {
                foreach (System.Windows.Point p in clickedPoints)
                {
                    int x = Convert.ToInt32(p.X);
                    int y = Convert.ToInt32(p.Y);

                    if (x <= edgeImage.Width && y <= edgeImage.Height)
                    {
                        //Call RBG values from each pixel
                        System.Drawing.Color color = edgeImage.GetPixel(x, y);

                        int counter = 0;
                        /////////////////////////////////////////////////////////////////////
                        //top, left edge point
                        System.Drawing.Point topLeftPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter 
                        counter = 0;
                        while (color.GetBrightness() <= value)
                        {
                            //prevent from running over the edge of an image
                            if ((counter < y) && (counter < x))
                            {
                                color = edgeImage.GetPixel(topLeftPoint.X, topLeftPoint.Y);
                                topLeftPoint.Y--;
                                topLeftPoint.X--;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint topLeft = new IntPoint(topLeftPoint.X, topLeftPoint.Y);
                        edgePoints.Add(topLeft);

                        //top edge point
                        System.Drawing.Point topPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while (color.GetBrightness() <= value)
                        {
                            //prevent from running over the edge of an image
                            if (counter < y)
                            {
                                color = edgeImage.GetPixel(topPoint.X, topPoint.Y);
                                topPoint.Y--;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint top = new IntPoint(topPoint.X, topPoint.Y);
                        edgePoints.Add(top);

                        //top, right edge point
                        System.Drawing.Point topRightPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while (color.GetBrightness() <= value)
                        {
                            //prevent from running over the edge of an image
                            if ((counter < y) && (counter < (edgeImage.Width - x)))
                            {
                                color = edgeImage.GetPixel(topRightPoint.X, topRightPoint.Y);
                                topRightPoint.Y--;
                                topRightPoint.X++;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint topRight = new IntPoint(topRightPoint.X, topRightPoint.Y);
                        edgePoints.Add(topRight);

                        //right edge point
                        System.Drawing.Point rightPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while (color.GetBrightness() <= value)
                        {
                            //prevent from running over the edge of an image
                            if (counter < (edgeImage.Width - x))
                            {
                                color = edgeImage.GetPixel(rightPoint.X, rightPoint.Y);
                                rightPoint.X++;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint right = new IntPoint(rightPoint.X, rightPoint.Y);
                        edgePoints.Add(right);

                        //bottom, right edge point
                        System.Drawing.Point botRightPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while (color.GetBrightness() <= value)
                        {
                            //prevent from running over the edge of an image
                            if (counter < (edgeImage.Height - y) && counter < (edgeImage.Width - x))
                            {
                                color = edgeImage.GetPixel(botRightPoint.X, botRightPoint.Y);
                                botRightPoint.Y++;
                                botRightPoint.X++;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint botRight = new IntPoint(botRightPoint.X, botRightPoint.Y);
                        edgePoints.Add(botRight);

                        //bottom edge point
                        System.Drawing.Point botPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while (color.GetBrightness() <= value)
                        {
                            //prevent from running over the edge of an image
                            if (counter < (edgeImage.Height - y))
                            {
                                color = edgeImage.GetPixel(botPoint.X, botPoint.Y);
                                botPoint.Y++;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint bot = new IntPoint(botPoint.X, botPoint.Y);
                        edgePoints.Add(bot);

                        //bottom left edge point
                        System.Drawing.Point botLeftPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while (color.GetBrightness() <= value)
                        {
                            //prevent from running over the edge of an image
                            if (counter < (edgeImage.Height - y) && counter < x)
                            {
                                color = edgeImage.GetPixel(botLeftPoint.X, botLeftPoint.Y);
                                botLeftPoint.Y++;
                                botLeftPoint.X--;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint botLeft = new IntPoint(botLeftPoint.X, botLeftPoint.Y);
                        edgePoints.Add(botLeft);

                        //left edge point
                        System.Drawing.Point leftPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while (color.GetBrightness() <= value)
                        {
                            //prevent from running over the edge of an image
                            if (counter < x)
                            {
                                color = edgeImage.GetPixel(leftPoint.X, leftPoint.Y);
                                leftPoint.X--;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint left = new IntPoint(leftPoint.X, leftPoint.Y);
                        edgePoints.Add(left);
                    }
                }
            }
            else { }

            return edgePoints;
        }

        /// <summary>
        /// Search for edgePoints by checking neighboring colors of pixels.
        /// </summary>
        /// <param name="clickedPoints">list of clicked points on the image</param>
        /// <param name="edgeImage">is the actual considered image</param>
        /// <returns>list of detected Points</returns>
        public List<IntPoint> getEdgePointsColor(List<System.Windows.Point> clickedPoints, Bitmap edgeImage)
        {
            //Lege Schwellwert fest
            double value = 50;

            //list of edge points
            List<IntPoint> edgePoints = new List<IntPoint>();

            if (clickedPoints.Count >= 1)
            {
                foreach (System.Windows.Point p in clickedPoints)
                {
                    //starting point
                    int x = Convert.ToInt32(p.X);
                    int y = Convert.ToInt32(p.Y);

                    //starting color
                    System.Drawing.Color startColor = edgeImage.GetPixel(x, y);

                    if (x <= edgeImage.Width && y <= edgeImage.Height)
                    {
                        //Rufe Farbwert des Pixels ab
                        System.Drawing.Color color = edgeImage.GetPixel(x, y);

                        int counter = 0;
                        /////////////////////////////////////////////////////////////////////
                        //top, left edge point
                        System.Drawing.Point topLeftPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while ((color.R <= (startColor.R + value)) && (color.R >= (startColor.R - value)) && (color.G <= (startColor.G + value)) &&
                            (color.G >= (startColor.G - value)) && (color.B <= (startColor.B + value)) && (color.B >= (startColor.B - value)))
                        {
                            //prevent from running over the edge of an image
                            if ((counter < y) && (counter < x))
                            {
                                color = edgeImage.GetPixel(topLeftPoint.X, topLeftPoint.Y);
                                topLeftPoint.Y--;
                                topLeftPoint.X--;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint topLeft = new IntPoint(topLeftPoint.X, topLeftPoint.Y);
                        edgePoints.Add(topLeft);

                        //top edge point
                        System.Drawing.Point topPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while ((color.R <= (startColor.R + value)) && (color.R >= (startColor.R - value)) && (color.G <= (startColor.G + value)) &&
                            (color.G >= (startColor.G - value)) && (color.B <= (startColor.B + value)) && (color.B >= (startColor.B - value)))
                        {
                            //prevent from running over the edge of an image
                            if (counter < y)
                            {
                                color = edgeImage.GetPixel(topPoint.X, topPoint.Y);
                                topPoint.Y--;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint top = new IntPoint(topPoint.X, topPoint.Y);
                        edgePoints.Add(top);

                        //top, right edge point
                        System.Drawing.Point topRightPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while ((color.R <= (startColor.R + value)) && (color.R >= (startColor.R - value)) && (color.G <= (startColor.G + value)) &&
                            (color.G >= (startColor.G - value)) && (color.B <= (startColor.B + value)) && (color.B >= (startColor.B - value)))
                        {
                            //prevent from running over the edge of an image
                            if ((counter < y) && (counter < (edgeImage.Width - x)))
                            {
                                color = edgeImage.GetPixel(topRightPoint.X, topRightPoint.Y);
                                topRightPoint.Y--;
                                topRightPoint.X++;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint topRight = new IntPoint(topRightPoint.X, topRightPoint.Y);
                        edgePoints.Add(topRight);

                        //right edge point
                        System.Drawing.Point rightPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while ((color.R <= (startColor.R + value)) && (color.R >= (startColor.R - value)) && (color.G <= (startColor.G + value)) &&
                            (color.G >= (startColor.G - value)) && (color.B <= (startColor.B + value)) && (color.B >= (startColor.B - value)))
                        {
                            //prevent from running over the edge of an image
                            if (counter < (edgeImage.Width - x))
                            {
                                color = edgeImage.GetPixel(rightPoint.X, rightPoint.Y);
                                rightPoint.X++;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint right = new IntPoint(rightPoint.X, rightPoint.Y);
                        edgePoints.Add(right);

                        //bottom, right edge point
                        System.Drawing.Point botRightPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while ((color.R <= (startColor.R + value)) && (color.R >= (startColor.R - value)) && (color.G <= (startColor.G + value)) &&
                            (color.G >= (startColor.G - value)) && (color.B <= (startColor.B + value)) && (color.B >= (startColor.B - value)))
                        {
                            //prevent from running over the edge of an image
                            if (counter < (edgeImage.Height - y) && counter < (edgeImage.Width - x))
                            {
                                color = edgeImage.GetPixel(botRightPoint.X, botRightPoint.Y);
                                botRightPoint.Y++;
                                botRightPoint.X++;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint botRight = new IntPoint(botRightPoint.X, botRightPoint.Y);
                        edgePoints.Add(botRight);

                        //bottom edge point
                        System.Drawing.Point botPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while ((color.R <= (startColor.R + value)) && (color.R >= (startColor.R - value)) && (color.G <= (startColor.G + value)) &&
                            (color.G >= (startColor.G - value)) && (color.B <= (startColor.B + value)) && (color.B >= (startColor.B - value)))
                        {
                            //prevent from running over the edge of an image
                            if (counter < (edgeImage.Height - y))
                            {
                                color = edgeImage.GetPixel(botPoint.X, botPoint.Y);
                                botPoint.Y++;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint bot = new IntPoint(botPoint.X, botPoint.Y);
                        edgePoints.Add(bot);

                        //bottom left point
                        System.Drawing.Point botLeftPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while ((color.R <= (startColor.R + value)) && (color.R >= (startColor.R - value)) && (color.G <= (startColor.G + value)) &&
                            (color.G >= (startColor.G - value)) && (color.B <= (startColor.B + value)) && (color.B >= (startColor.B - value)))
                        {
                            //prevent from running over the edge of an image
                            if (counter < (edgeImage.Height - y) && counter < x)
                            {
                                color = edgeImage.GetPixel(botLeftPoint.X, botLeftPoint.Y);
                                botLeftPoint.Y++;
                                botLeftPoint.X--;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint botLeft = new IntPoint(botLeftPoint.X, botLeftPoint.Y);
                        edgePoints.Add(botLeft);

                        //left edge point
                        System.Drawing.Point leftPoint = new System.Drawing.Point(x, y);
                        color = edgeImage.GetPixel(x, y);
                        //reset counter
                        counter = 0;
                        while ((color.R <= (startColor.R + value)) && (color.R >= (startColor.R - value)) && (color.G <= (startColor.G + value)) &&
                            (color.G >= (startColor.G - value)) && (color.B <= (startColor.B + value)) && (color.B >= (startColor.B - value)))
                        {
                            //prevent from running over the edge of an image
                            if (counter < x)
                            {
                                color = edgeImage.GetPixel(leftPoint.X, leftPoint.Y);
                                leftPoint.X--;
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //add new point
                        IntPoint left = new IntPoint(leftPoint.X, leftPoint.Y);
                        edgePoints.Add(left);
                    }
                }
            }
            else { }

            return edgePoints;
        }

        /// <summary>
        /// Draw a polygonal shape.
        /// </summary>
        /// <param name="points">list of corners of the polygon</param>
        /// <param name="image">image polygon shall be drawn on</param>
        /// <returns>image with drawn polygon</returns>
        public static Bitmap drawPolygon(List<IntPoint> points, Bitmap image)
        {
            //init helper instance
            Helper helper = new Helper();
            //convert points to an array
            System.Drawing.Point[] a = helper.convertIntPointsToPoints(points);

            //draw polygon
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pen = new Pen(System.Drawing.Color.DarkMagenta, 5);
            if (a.Length != 0)
            {
                g.DrawPolygon(pen, a);
                return image;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Creates a black/white copy of the detected circle and crops the 
        /// image to this section in shape of a rectangle.
        /// </summary>
        /// <param name="points">list of points of the detected rectangle</param>
        /// <returns>black/white image</returns>
        public Bitmap copyCircle(List<IntPoint> points)
        {
            //init helper instance
            Helper helper = new Helper();

            //copy rectangle shape of the segment
            Bitmap copyRect = new Bitmap(getRect(points).Width, getRect(points).Height);

            //Transformiere die Punkte auf das RechteckSegment
            int MinX = getRect(points).X; //minimale X-Wert = linker,obere Punkt
            int MinY = getRect(points).Y;

            //neue Liste für die lokalen Punkte des Segmentes
            List<IntPoint> localPoints = new List<IntPoint>();
            foreach (IntPoint p in points)
            {
                int xValue = p.X - MinX;
                int yValue = p.Y - MinY;
                localPoints.Add(new IntPoint(xValue, yValue));
            }

            //Fülle copyRect schwarz
            Graphics c = Graphics.FromImage(copyRect);
            SolidBrush brushRect = new SolidBrush(Color.Black);
            c.FillRectangle(brushRect, 0, 0, copyRect.Width, copyRect.Height);
            //Füge den Auschnitt als weißes Segment hinzu
            Graphics circle = Graphics.FromImage(copyRect);
            SolidBrush brushCircle = new SolidBrush(Color.White);
            circle.FillEllipse(brushCircle, getRect(localPoints));

            //gib die Kopie zurück
            return copyRect;
        }

        /// <summary>
        /// Makes a smooth edge for each cropped circle.
        /// </summary>
        /// <param name="segment">created circle segment</param>
        /// <param name="points">edge points of the circle/rectangle</param>
        /// <returns>circle with smooth edge</returns>
        public Bitmap cleanCircleEdge(Bitmap segment, List<IntPoint> points)
        {
            //Transformiere die Punkte auf das RechteckSegment
            int MinX = getRect(points).X; //minimale X-Wert = linker,obere Punkt
            int MinY = getRect(points).Y;
            //neue Liste für die lokalen Punkte des Segmentes
            List<IntPoint> localPoints = new List<IntPoint>();
            foreach (IntPoint p in points)
            {
                int xValue = p.X - MinX;
                int yValue = p.Y - MinY;
                localPoints.Add(new IntPoint(xValue, yValue));
            }

            //Kantenglättung des Segments
            Graphics s = Graphics.FromImage(segment);
            s.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            System.Drawing.Pen penCircle = new System.Drawing.Pen(System.Drawing.Color.White, 3);
            s.DrawEllipse(penCircle, getRect(localPoints));
            s.Dispose();
            penCircle.Dispose();

            return segment;
        }

        /// <summary>
        /// This function returns black if the background is too reddish
        /// </summary>
        /// <param name="color">actual color of the set point</param>
        /// <returns>red or black as best fit for the background</returns>
        public static System.Drawing.Color getColor(System.Drawing.Color color)
        {
            //color that will be returned
            Color newColor = System.Drawing.Color.Red;
            //if backgrund is too reddish return black
            if (color.R >= 200 && color.G <= 100 && color.B <= 100)
            {
                newColor = System.Drawing.Color.Black;
            }
            else if (color.R >= 130 && color.G <= 50 && color.B <= 50)
            {
                newColor = System.Drawing.Color.Black;
            }
            return newColor;
        }
    }

}

