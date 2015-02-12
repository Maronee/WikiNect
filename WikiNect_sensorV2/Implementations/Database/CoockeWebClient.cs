using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace DataConnection
{
    public class CoockieWebClient : WebClient
    {
        /// <summary>
        /// Öffentlicher Speicher für Cookies. 
        /// </summary>
        public CookieContainer cc = new CookieContainer();

        /// <summary>
        /// Rufe GetWebRequest mit der Uri. Siehe auch Methode WebRequest.GetWebRequest.
        /// </summary>
        /// <param name="address">Die Uri aufrurufen.</param>
        /// <returns>request</returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = cc;
            }
            return request;
        }

        /*

                System.Collections.Specialized.NameValueCollection nvc = null;
                byte[] bytes;
                Uri uri = new Uri("http://mw1.wikinect.hucompute.org/api.php");
                string response = null;
                string logintoken = null;
                string edittoken = null;

                // Send request to login
                nvc = new System.Collections.Specialized.NameValueCollection();
                nvc.Add("action", "login");
                nvc.Add("lgname", "Konrad");
                nvc.Add("lgpassword", "frohm");
                nvc.Add("format", "json");
                bytes = wc.UploadValues(uri, nvc);
                response = new System.Text.UTF8Encoding().GetString(bytes);

                // Deserialize response
                var json = JsonConvert.DeserializeObject<MediaWiki>(response);
                logintoken = json.Login.Token;

                // Send request to login again but with token
                nvc = new System.Collections.Specialized.NameValueCollection();
                nvc.Add("action", "login");
                nvc.Add("lgname", "Konrad");
                nvc.Add("lgpassword", "frohm");
                nvc.Add("lgtoken", logintoken);
                nvc.Add("format", "json");
                bytes = wc.UploadValues(uri, nvc);
                response = new System.Text.UTF8Encoding().GetString(bytes);         
         */
    }
}
