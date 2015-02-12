using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Specialized;

namespace DataStore
{
    /// <summary>
    /// Interface for Images extend ContentElement
    /// </summary>
    public interface Image : ContentElement
    {
        /// <summary>
        /// Get the Image
        /// </summary>
        /// <returns></returns>
        System.Drawing.Image getImage();

        /// <summary>
        /// Set the Image - Data
        /// </summary>
        void setImage(System.Drawing.Image img);

        /// <summary>
        /// loads the Image
        /// </summary>
        /// <returns></returns>
        System.Drawing.Image LoadImage();

        void setInfos(Dictionary<String, String> infos);

        void addInfo(String key, String value);

        Dictionary<String, String> getInfos();

        String getInfo(String key);

        /// <summary>
        /// Get the URL
        /// </summary>
        /// <returns></returns>
        String getURL();

        /// <summary>
        /// Set the URL
        /// </summary>
        void setURL(String URL);
    }
}
