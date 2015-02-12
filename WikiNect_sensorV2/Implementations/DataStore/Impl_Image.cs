using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace DataStore
{
    public class Impl_Image : ImplContentElement, Image
    {

        private Uri uri;
        private string sName;
        private string sDescription;
        private string url;
        private Dictionary<String, String> infos = new Dictionary<String, String>();
        System.Drawing.Image img;

        public Impl_Image(DataConnection.Connection connection, Uri uri, String sName, String sDescription, String url)
            : base(connection, uri, sName, sDescription) 
        {
            this.connection = connection;
            this.uri = uri;
            this.sName = sName;
            this.sDescription = sDescription;
            this.url = url;
        }

        public Impl_Image(DataConnection.Connection connection, Uri uri, String sName, String sDescription, String url, Dictionary<String, String> infos)
            : base(connection, uri, sName, sDescription)
        {
            this.connection = connection;
            this.uri = uri;
            this.sName = sName;
            this.sDescription = sDescription;
            this.url = url;
            this.infos = infos;
        }

        /// <summary>
        /// Gets the image from URLok
        /// (from http://dotnet-snippets.de/snippet/image-aus-url-laden/509)
        /// </summary>
        /// <param name="url">The URL</param>
        /// <returns></returns>
        public static System.Drawing.Image getImageFromURL(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream stream = httpWebReponse.GetResponseStream();
            return System.Drawing.Image.FromStream(stream);
        }

        public void setInfos(Dictionary<String, String> infos)
        {
            this.infos = infos;
        }

        public void addInfo(String key, String value)
        {
            this.infos.Add(key, value);
        }

        public Dictionary<String, String> getInfos()
        {
            return this.infos;
        }

        public String getInfo(String key)
        {
            return this.infos[key];

        }

        public String getURL()
        {
            return this.url;
        }

        public void setURL(String url)
        {
            this.url = url;
        }

        public System.Drawing.Image getImage()
        {
            return this.img;
        }

        public void setImage(System.Drawing.Image img)
        {
            this.img = img;
        }


        public System.Drawing.Image LoadImage()
        {
            this.img = getImageFromURL(getURL());
            return img;
        }
    }
}
