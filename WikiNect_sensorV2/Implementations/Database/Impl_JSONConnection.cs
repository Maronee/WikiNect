using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using DataStore;
using Wikinect.Wrapper;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;

namespace DataConnection
{

    class JSONConnection:DataConnection.Connection
    {
        //URL where the Mediawiki is
        String sURL = "";

        WebClient wc = null;

        String tokenID = "";
        String sessionID = "";

        //Tokens of the session, without you cann't change something
        String editToken = "";
        String deleteToken = "";

        int userid = -1;

        //standard login date for mediawiki
        String user = "DUMMY";
        String password = "foobar";

        //if allready loged in
        bool isLogin = false;

        /// <summary>
        /// login the user with the standard login dates
        /// </summary>
        /// <returns>bool, if login was successfull</returns>
        public bool login()
        {
            
            String webPageText;
            byte[] bytes;

            //WebClient will used in this session
            using (this.wc)
            {
                // define Post-Parameters
                // to get the informations through the api
                var data = new NameValueCollection();
                data["lgname"] = user; //Loginname
                data["lgpassword"] = password; //Loginpassword
                data["action"] = "login"; // because we want to login
                data["format"] = "json"; // we allways want the JSON-Format

                // First login for getting the Token
                Uri u = new Uri(this.sURL+"api.php");

                // get the return of the API
                bytes = this.wc.UploadValues(u, data);
                // change into a String
                webPageText = new System.Text.UTF8Encoding().GetString(bytes);
                
                //get the TokenID and SessionID, first convert the String so we can get them through to keys
                this.tokenID = (JsonConvert.DeserializeObject<MediaWiki>(webPageText)).Login.token;
                this.sessionID = (JsonConvert.DeserializeObject<MediaWiki>(webPageText)).Login.Sessionid;
                
                // now we have the Token to success the Login
                data["lgtoken"] = this.tokenID;

                bytes = this.wc.UploadValues(u, data);
                webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

                // if the result ist true, then the login was successfull
                this.isLogin = json.Login.Result.Equals("Success") ? true : false;

                // change the login-status
                if (this.isLogin)
                {
                    this.userid = json.Login.userid;
                }
            }
            return isLogin;
        }

        /// <summary>
        /// login a special user with the given password
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="password">Userpassword</param>
        /// <returns>bool, if login was successfull</returns>
        public bool login(string user, string password)
        {
            // change the parameters and then normal login
            this.user = user;
            this.password = password;
            return login();
        }

        /// <summary>
        /// logout user
        /// </summary>
        public void logout()
        {
            using (this.wc)
            {
                Uri u = new Uri(this.sURL + "api.php?action=logout");
                this.wc.OpenWrite(u);
            }
        }

        /// <summary>
        /// start with standard login
        /// </summary>
        /// <param name="sURL">URL of Mediawiki</param>
        public JSONConnection(String sURL)
        {
            // save to given URL
            this.sURL = sURL;

            // we take the CoockieWebClient as WebClient, to get the access to the Mediawiki
            this.wc = new CoockieWebClient();

            login();
        }

        /// <summary>
        /// start with login
        /// </summary>
        /// <param name="sURL">URL of Mediawiki</param>
        /// <param name="sUser">Username</param>
        /// <param name="sPassword">Password</param>
        public JSONConnection(String sURL, String sUser, String sPassword)
        {
            this.sURL = sURL;
            this.wc = new CoockieWebClient();

            login(sUser, sPassword);
        }

        /// <summary>
        /// Get the SuperCategories of Category from the Name
        /// </summary>
        /// <param name="nameCategory">Categoryname</param>
        /// <returns></returns>
        public List<DataStore.Category> getSuperCategories(string nameCategory)
        {
            return getCategories(nameCategory);
        }

        /// <summary>
        /// Get the SuperCategories of Category
        /// </summary>
        /// <param name="pCategory">Category</param>
        /// <returns></returns>
        public List<DataStore.Category> getSuperCategories(DataStore.Category pCategory)
        {
            return getCategories(pCategory);
        }

        /// <summary>
        /// Get the SubCategories of Category
        /// </summary>
        /// <param name="pCategory">Category</param>
        /// <returns></returns>
        public List<DataStore.Category> getSubCategories(DataStore.Category pCategory)
        {
            List<DataStore.Category> dataelements = new List<DataStore.Category>();

            // text output from a json request
            string webPageText;
            byte[] bytes;

            using (this.wc)
            {
                // define Post-Parameters
                var data = new NameValueCollection();
                data["action"] = "query";
                data["list"] = "categorymembers";
                data["cmtitle"] = "Kategorie:" + (pCategory.getName()).Trim(new Char[] { ' ', '"', '\'' });
                data["format"] = "json";
                data["cmlimit"] = "500";

                Uri u = new Uri(this.sURL + "api.php");

                bytes = this.wc.UploadValues(u, data);
                webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

                foreach (CategoryMember allcat in json.Query.Pages)
                {
                    if ((allcat.Title.ToString()).StartsWith("Kategorie:"))
                    {
                        dataelements.Add(new CategoryImpl(this, new Uri(Wikinect.WikiNectConst.sAnnotationNamespace + allcat.Title), allcat.Title, ""));
                    }
                }
            }
            return dataelements;
        }

        /// <summary>
        /// Find all Categories
        /// </summary>
        /// <returns>List of Categories</returns>
        public List<Category> getCategories()
        {
            List<DataStore.Category> categories = new List<DataStore.Category>();

            // text output from a json request
            string webPageText;

            using (this.wc)
            {
                // Use MediaWiki API to list all the categories with list=allcategories
                Uri u = new Uri(this.sURL + "/api.php?action=query&list=allcategories&acprop=size|hidden&aclimit=500&format=json");
                webPageText = this.wc.DownloadString(u);
            }

            {
                var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

                foreach (Allcategory allcat in json.Query.Categories)
                {
                    categories.Add(new CategoryImpl(this, new Uri(Wikinect.WikiNectConst.sAnnotationNamespace + allcat.Title), allcat.Title, ""));
                }
            }
            return categories;
        }

        /// <summary>
        /// Find the Categories of a element
        /// </summary>
        /// <param name="lCategory"></param>
        /// <returns>Categories</returns>
        public List<Category> getCategories(DataElement pElement)
        {
            List<DataStore.Category> categories = new List<DataStore.Category>();

            // text output from a json request
            string webPageText;

            using (this.wc)
            {
                // so we get list alle Categories of the Element
                Uri u = new Uri(this.sURL + "/api.php?action=query&titles=" + pElement.getName() + "&prop=categories&format=json");
                webPageText = this.wc.DownloadString(u);
            }

            {
                var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

                foreach (Allcategory allcat in json.Query.Categories)
                {
                    categories.Add(new CategoryImpl(this, new Uri(Wikinect.WikiNectConst.sAnnotationNamespace + allcat.Title), allcat.Title, ""));
                }
            }
            return categories;
        }

        /// <summary>
        /// Find the Categories of a element
        /// </summary>
        /// <param name="lCategory"></param>
        /// <returns>Categories</returns>
        public List<Category> getCategories(string nameElement)
        {
            List<DataStore.Category> categories = new List<DataStore.Category>();

            // text output from a json request
            string webPageText;

            using (this.wc)
            {
                // so we get list alle Categories of the Element
                Uri u = new Uri(this.sURL + "/api.php?action=query&titles=" + nameElement + "&prop=categories&format=json");
                webPageText = this.wc.DownloadString(u);
            }

            string delete_string_1 = "\"title\":\"";
            string delete_string_2 = "\"}";
            List<String> categorienamen = new List<String>();
            webPageText = webPageText.Substring(webPageText.LastIndexOf("categories") + 9);
            do
            {
                string webPageText_1 = webPageText.Substring(webPageText.LastIndexOf(delete_string_1) + 9);
                string webPageText_2 = webPageText_1.Substring(0, webPageText_1.IndexOf(delete_string_2));
                webPageText = webPageText.Substring(0, webPageText.LastIndexOf(webPageText_1) - 9);
                categories.Add(new CategoryImpl(this, new Uri(Wikinect.WikiNectConst.sAnnotationNamespace + webPageText_2), webPageText_2, ""));
            } while (webPageText.Contains(delete_string_1));
            return categories;
        }

        /// <summary>
        /// Find the Category with this number
        /// </summary>
        /// <param name="lCategory"></param>
        /// <returns>Category</returns>
        public Category getCategory(long lCategory)
        {
            // text output from a json request
            string webPageText;

            using (this.wc)
            {
                Uri u = new Uri(this.sURL + "api.php?action=query&prop=info&pageids=" + lCategory + "&inprop=url&format=json");
                Console.WriteLine(u);
                webPageText = this.wc.DownloadString(u);
            }
            string find_string_1 = "title\":";
            string webPageText_1 = webPageText.Substring(webPageText.IndexOf(find_string_1) + 8);

            string webPageText_2 = webPageText_1.Substring(0, webPageText_1.IndexOf("\",\"") > 0 ? webPageText_1.IndexOf("\",\"") : 0);
            return new CategoryImpl(this, new Uri(Wikinect.WikiNectConst.sCategoryNamespace+webPageText_1), webPageText_2, "");
        }

        /// <summary>
        /// Search for all Elements in the given Category
        /// </summary>
        /// <param name="lCategory"></param>
        /// <returns>Element of the Category</returns>
        public List<DataElement> getElements(long lCategory)
        {
            return getElements(getCategory(lCategory));
        }

        /// <summary>
        /// Search for all Elements in the given Categories
        /// </summary>
        /// <param name="pListCategory"></param>
        /// <returns>Elements of all Categories in the List</returns>
        public List<DataElement> getElements(List<long> pListCategory)
        {
            List<DataStore.Category> categories = new List<DataStore.Category>();

            foreach (long long_c in pListCategory)
            {
                categories.Add(getCategory(long_c));
            }

            return getElements(categories);
        }

        public List<DataElement> getElements(Category pCategory)
        {
            List<DataStore.DataElement> dataelements = new List<DataStore.DataElement>();

            // text output from a json request
            string webPageText;

            using (this.wc)
            {
                // Use MediaWiki API to list all the categories with list=allcategories
                Uri u = new Uri(this.sURL + "api.php?action=query&list=categorymembers&cmtitle=Kategorie:" + (pCategory.getName()).Trim(new Char[] { ' ', '"', '\'' }) + "&format=json&cmlimit=500");
                webPageText = this.wc.DownloadString(u);
            }

            {
                var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

                foreach (CategoryMember allcat in json.Query.Pages)
                {
                    if ((allcat.Title.ToString()).StartsWith("Kategorie:") == false)
                    {
                        Console.WriteLine(allcat.PageId);
                        dataelements.Add(new CategoryImpl(this, new Uri(Wikinect.WikiNectConst.sElementNamespace + allcat.PageId), allcat.Title, ""));
                    }
                }
            }

            return dataelements;
        }
        
        /// <summary>
        /// get alle Elements of a List of Categories
        /// </summary>
        /// <param name="pListCategory"></param>
        /// <returns></returns>
        public List<DataElement> getElements(List<Category> pListCategory)
        {
            List<DataStore.DataElement> dataelements = new List<DataStore.DataElement>();

            // take every Category and find the Elements
            foreach (Category c in pListCategory)
            {
                dataelements.AddRange(getElements(c));
            }

            return dataelements;
        }

        /// <summary>
        /// The whole element-inforamtions
        /// so we have the text and can get all Informations of this site we want
        /// </summary>
        /// <param name="element"></param>
        /// <returns>String with JSON</returns>
        private string getAllofElement(string elementName)
        {
            // text output from a json request
            string webPageText;
            byte[] bytes;

            using (this.wc)
            {
                // define Post-Parameters
                var data = new NameValueCollection();
                data["action"] = "query";
                data["titles"] = elementName;
                data["prop"] = "revisions";
                data["rvprop"] = "content";
                data["format"] = "json";

                Uri u = new Uri(this.sURL + "api.php");

                bytes = this.wc.UploadValues(u, data);
                webPageText = new System.Text.UTF8Encoding().GetString(bytes);

            }
            return webPageText;
        }

        /// <summary>
        /// The whole element-inforamtions
        /// so we have the text and can get all Informations of this site we want
        /// </summary>
        /// <param name="element"></param>
        /// <returns>String with JSON</returns>
        private string getAllofElement(string elementName, string format)
        {
            // text output from a json request
            string webPageText;
            byte[] bytes;
            
            using (this.wc)
            {
                try
                {
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "query";
                    data["titles"] = elementName;
                    data["prop"] = "revisions";
                    data["rvprop"] = "content";
                    data["format"] = format;

                    Uri u = new Uri(this.sURL + "api.php");

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);
                }
                catch
                { 
                    return "";
                }
            }
            return webPageText;
        }

        /// <summary>
        /// The whole element-inforamtions
        /// so we have the text and can get all Informations of this site we want
        /// </summary>
        /// <param name="element"></param>
        /// <returns>String with JSON</returns>
        private string getAllofElement(DataElement pElement)
        {
            return getAllofElement(pElement.getName());
        }

        /// <summary>
        /// get alle Ratings of all Elements in the Mediawiki
        /// </summary>
        /// <returns></returns>
        public List<Rating> getRatings()
        {
            List<Rating> ratings = new List<Rating>();

            List<Category> categories = getCategories();
            foreach (Category c in categories)
            {
                List<DataElement> elements = getElements(c);
                foreach (DataElement e in elements)
                {
                    List<Rating> temp_rating = getRatings(e);

                    ratings = ratings.Union(temp_rating).ToList();
                }
            }
            return ratings;
        }

        /// <summary>
        /// get the Rating with a special ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Rating getRating(long id)
        {
            List<Rating> lRating = new List<Rating>();
            lRating = getRatings();

            foreach (Rating r in lRating)
            {
                if (r.getRatingId() == id)
                {
                    return r;
                }
            }
            return null;
        }

        /// <summary>
        /// get all Ratings of a Element
        /// </summary>
        /// <param name="pElement">the Element, you want to get the Ratings from</param>
        /// <returns></returns>
        public List<Rating> getRatings(DataElement pElement)
        {
            List<Rating> ratings = new List<Rating>();
            String imgName = "";
            Uri u = new Uri(this.sURL + "api.php");
            //Ratings are current saved as Images, so we get a Image
            System.Drawing.Image img = null;

            try
            {
                //first we try to find the Rating in the Elementpage
                string text = getAllofElement(pElement);

                int start_link_nr = text.IndexOf("RATING=Datei:", 0) + 7;

                int stop_link_nr = text.IndexOf("|", start_link_nr + 11);

                imgName = text.Substring(start_link_nr, stop_link_nr - start_link_nr);

                // then we want to get the Rating-Image through to Mediawiki API
                using(this.wc)
                {
                    string webPageText;
                    byte[] bytes;
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "query";
                    data["list"] = "allimages";
                    data["aiprefix"] = imgName.Substring(6);
                    data["ailimit"] = "1";
                    data["aiprop"] = "url";
                    data["format"] = "json";

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);
                    var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

                    // add the new Rating to a List of Ratings
                    ratings.Add(new Impl_Rating(this, new Uri(Wikinect.WikiNectConst.sRatingNamespace + imgName.Substring(6)), imgName.Substring(6), "", img, json.Query.Images[0].Url));
                }
            }
            catch
            {
                // no new rating could be found
            }
            return ratings;
        }

        /// <summary>
        /// get all Ratings of the Elements in one Category
        /// </summary>
        /// <param name="pCategory"></param>
        /// <returns></returns>
        public List<Rating> getRatings(Category pCategory)
        {
            List<Rating> ratings = new List<Rating>();

            List<DataElement> elements = getElements(pCategory);
            foreach (DataElement e in elements)
            {
                List<Rating> temp_rating = getRatings(e);
                ratings = ratings.Union(temp_rating).ToList();
            }
            return ratings;
        }

        /// <summary>
        /// get all Users of the Mediawiki
        /// </summary>
        /// <returns>List of all Users</returns>
        public List<User> getUsers()
        {
            // text output from a json request
            string webPageText;

            using (this.wc)
            {
                // Use MediaWiki API to list all the Users
                Uri u = new Uri(this.sURL + "api.php?action=query&list=allusers&format=json");
                webPageText = this.wc.DownloadString(u);
            }
            var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

            List<User> lUser = new List<User>();

            foreach (AllUsers users in json.Query.Users)
            {
                //allways create a new User-Element
                User u = new Impl_User(users.userid, users.name, this);
                lUser.Add(u);
            }

            return lUser;
        }

        /// <summary>
        /// get a user from the UserID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User getUser(long id)
        {
            List<User> lUser = new List<User>();
            lUser = getUsers();

           foreach (User u in lUser)
            {
                if (u.getUserId() == id)
                {
                    return u;
                }
            }
           return null;
        }

        /// <summary>
        /// get all Annotations
        /// bAll = true, only Annotation from both sides are there
        /// bAll = false, get all Annotations
        /// </summary>
        /// <param name="bAll"></param>
        /// <returns></returns>
        public List<Annotation> getAnnotations(bool bAll)
        {

            List<Annotation> annotations = new List<Annotation>();

            if (bAll)
            {
                List<Category> categories = getCategories();
                foreach (Category c in categories)
                {
                    List<DataElement> elements = getElements(c);
                    foreach (DataElement e in elements)
                    {
                        List<Annotation> temp_anno = getAnnotations(e);

                        foreach (Annotation a in temp_anno)
                        {
                            annotations.Add(a);
                        }
                    }
                }
                annotations = annotations.Union(getAttributs()).ToList();
                annotations = annotations.Union(getLinks()).ToList();
            }
            else
            {
                List<Category> categories = getCategories();
                foreach (Category c in categories)
                {
                    List<DataElement> elements = getElements(c);
                    foreach (DataElement e in elements)
                    {
                        List<Annotation> temp_anno = getAnnotations(e);

                        foreach (Annotation a in temp_anno)
                        {
                            annotations.Add(a);
                        }
                    }
                }
            }
            return annotations;

        }

        /// <summary>
        /// get all Annotations
        /// </summary>
        /// <returns></returns>
        public List<Annotation> getAnnotations()
        {
            return getAnnotations(false);
        }

        /// <summary>
        /// get Annotations of one user
        /// </summary>
        /// <param name="pUser"></param>
        /// <param name="bAll"></param>
        /// <returns></returns>
        public List<Annotation> getAnnotations(User pUser, bool bAll)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get all Annotations of one user
        /// </summary>
        /// <param name="pUser"></param>
        /// <param name="bAll"></param>
        /// <returns></returns>
        public List<Annotation> getAnnotations(User pUser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get Annotations of one user with special AnnotationType
        /// </summary>
        /// <param name="pUser"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public List<Annotation> getAnnotations(User pUser, AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get Annotations of one user with special AnnotationType and of one element
        /// </summary>
        /// <param name="pUser"></param>
        /// <param name="pType"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public List<Annotation> getAnnotations(User pUser, AnnotationType pType, DataElement element)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get Annotations of one Element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="bAll"></param>
        /// <returns></returns>
        public List<Annotation> getAnnotations(DataElement element, bool bAll)
        {
            List<Annotation> annotations = new List<Annotation>();
            string text = getAllofElement(element);

            int start_anno_nr = text.IndexOf("== Untersegmente ==", 0);

            int stop_anno_nr = text.IndexOf("==", start_anno_nr + 11);

            text = text.Substring(start_anno_nr, stop_anno_nr - start_anno_nr);

            string[] annos = Regex.Split(text, "Datei:");
            for (int i = 1; i < annos.Length; i++)
            {
                string anno = annos[i];
                int stop_anno = anno.IndexOf("|", 0);
                annotations.Add(new Impl_Annotation(this, new Uri(Wikinect.WikiNectConst.sAnnotationNamespace + anno.Substring(0, stop_anno)), anno.Substring(0, stop_anno), ""));
            }
            if (bAll)
            {
                annotations = annotations.Union(getAttributs(element)).ToList();
                annotations = annotations.Union(getLinks(element)).ToList();
            }
            return annotations;
        }

        /// <summary>
        /// get Annotations of one Element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public List<Annotation> getAnnotations(DataElement element)
        {
            return getAnnotations(element, false);
        }

        /// <summary>
        /// get Annotations of one Element with special AnnotationType
        /// </summary>
        /// <param name="element"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public List<Annotation> getAnnotations(DataElement element, AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get Annotations of all Element with special AnnotationType
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        public List<Annotation> getAnnotations(AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get all Links of the Mediawiki as a List
        /// </summary>
        /// <returns></returns>
        public List<Annotation> getLinks()
        {
            List<Annotation> annotations = new List<Annotation>();

            List<Category> categories = getCategories();
            foreach (Category c in categories)
            {
                List<DataElement> elements = getElements(c);
                foreach (DataElement e in elements)
                {
                    List<Annotation> temp_links = getLinks(e);

                    annotations = annotations.Union(temp_links).ToList();
                }
            }
            return annotations;
        }

        /// <summary>
        /// get the Links on Userpage
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns>List of Links</returns>
        public List<Annotation> getLinks(User pUser)
        {
            List<Annotation> annotations = new List<Annotation>();

            // text output from a json request
            string webPageText;

            using (this.wc)
            {
                Uri u = new Uri(this.sURL + " api.php?action=query&titles=User:" + pUser.getUsername() + "&prop=links&format=json");
                webPageText = this.wc.DownloadString(u);
            }
            {
                var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

                foreach (LinkMember alllinks in json.Query.Links)
                {
                    annotations.Add(new Impl_Link(this, new Uri(Wikinect.WikiNectConst.sLinkNamespace+"z"), alllinks.Title, ""));
                }
            }

            return annotations;
        }

        /// <summary>
        /// get all Links of one User, with a special AnnotationType and of one Element
        /// </summary>
        /// <param name="pUser"></param>
        /// <param name="pType"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public List<Annotation> getLinks(User pUser, AnnotationType pType, DataElement element)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search for all Links at one Element
        /// </summary>
        /// <param name="element"></param>
        /// <returns>List of Links</returns>
        public List<Annotation> getLinks(DataElement element)
        {
            List<Annotation> annotations = new List<Annotation>();
            string text = getAllofElement(element);

            int start_link_nr = text.IndexOf("== Links ==", 0);

            int stop_link_nr = text.IndexOf("==", start_link_nr + 11);

            text = text.Substring(start_link_nr, stop_link_nr - start_link_nr);

            string[] links = Regex.Split(text, "Links::");
            for (int i = 1; i < links.Length; i++)
            {
                string link = links[i];
                int stop_link = link.IndexOf("]]", 0);
                annotations.Add(new Impl_Attribut(this, new Uri(Wikinect.WikiNectConst.sLinkNamespace+link), link.Substring(0, stop_link), ""));
            }
            return annotations;
        }

        /// <summary>
        /// get all Links of a Element
        /// </summary>
        /// <param name="sElement"></param>
        /// <returns></returns>
        public List<Annotation> getLinks(String sElement)
        {
            DataElement element = new CategoryImpl(this, new Uri(Wikinect.WikiNectConst.sElementNamespace + sElement), sElement, "");

            return getLinks(element);
        }

        /// <summary>
        /// Get all Links from Element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="pType">get only bidirected or directed Links</param>
        /// <returns>List of Links</returns>
        public List<Annotation> getLinks(DataElement element, AnnotationType pType)
        {
            // find all links
            List<Annotation> links = getLinks(element);
            List<Annotation> new_links = new List<Annotation>();
            List<Annotation> relinks = new List<Annotation>();

            // for all links, search, if AnnotationType is correct
            foreach (Annotation a in links)
            {
                if (pType == 0)
                {
                    relinks = getLinks(a.ToString());

                    bool bBidirected = false;

                    foreach (Annotation a2 in links)
                    {
                        if (a.ToString() == a2.ToString())
                        {
                            bBidirected = true;
                        }
                    }
                    if (bBidirected == false)
                    {
                        new_links.Add(a);
                    }
                }
                else
                {
                    relinks = getLinks(a.ToString());

                    foreach (Annotation a2 in links)
                    {
                        if (a.ToString() == a2.ToString())
                        {
                            new_links.Add(a);
                        }
                    }
                }
            }

            return new_links;
        }

        /// <summary>
        /// get the Links of special AnnotationType
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        public List<Annotation> getLinks(AnnotationType pType)
        {
            List<Annotation> annotations = new List<Annotation>();

            List<Category> categories = getCategories();
            foreach (Category c in categories)
            {
                List<DataElement> elements = getElements(c);
                foreach (DataElement e in elements)
                {
                    List<Annotation> temp_links = getLinks(e, pType);

                    annotations = annotations.Union(temp_links).ToList();
                }
            }
            return annotations;
        }

        /// <summary>
        /// get all Attributs of the Mediawiki
        /// </summary>
        /// <returns></returns>
        public List<Attribut> getAttributs()
        {
            List<Attribut> attributs = new List<Attribut>();

            // first get all Categories
            List<Category> categories = getCategories();
            foreach (Category c in categories)
            {
                List<DataElement> elements = getElements(c);
                // then get all Elements
                foreach (DataElement e in elements)
                {
                    List<Attribut> temp_attributs = getAttributs(e);
                    // then get all Attributs
                    foreach (Attribut a in temp_attributs)
                    {
                        attributs.Add(a);
                    }
                }
            }
            return attributs;
        }

        /// <summary>
        /// get all Attributs of a special User
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public List<Attribut> getAttributs(User pUser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get all Attributs of a special user, with special AnnotationType and Element
        /// </summary>
        /// <param name="pUser"></param>
        /// <param name="pType"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public List<Attribut> getAttributs(User pUser, AnnotationType pType, DataElement element)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get all Attributs of one Element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public List<Attribut> getAttributs(DataElement element)
        {
            List<Attribut> attributs = new List<Attribut>();
            string text = getAllofElement(element);

            int start_tag_nr = text.IndexOf("== Tags ==", 0);

            int stop_tag_nr = text.IndexOf("==", start_tag_nr + 11);

            text = text.Substring(start_tag_nr, stop_tag_nr - start_tag_nr);

            string[] tags = Regex.Split(text, "Tags::");
            for (int i = 1; i < tags.Length; i++)
            {
                string tag = tags[i];
                int stop_tag = tag.IndexOf("]]", 0);
                attributs.Add(new Impl_Attribut(this, new Uri(Wikinect.WikiNectConst.sAttributeNamespace + tag), tag.Substring(0, stop_tag), ""));
            }
            return attributs;
        }

        /// <summary>
        /// get all Attributs of one Element with special AnnotationType
        /// </summary>
        /// <param name="element"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public List<Attribut> getAttributs(DataElement element, AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to get the Edit-Token
        /// </summary>
        /// <returns></returns>
        private String getEditToken()
        {
            // only changed one time per session
            // so we don't need to change, if it exists
            if (this.editToken.Length > 0)
            {
                return this.editToken;
            }

            String webPageText = "";

            var data = new NameValueCollection();
            data["action"] = "query";
            data["prop"] = "info";
            data["intoken"] = "edit";
            data["titles"] = "Hauptseite";
            data["format"] = "json";

            Uri u = new Uri(this.sURL + "api.php");

            byte[] bytes = this.wc.UploadValues(u, data);

            webPageText = new System.Text.UTF8Encoding().GetString(bytes);
            var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
            
            this.editToken = json.Query.InfoPage.Page.Edittoken;
            
            return this.editToken;

        }

        /// <summary>
        /// Method to get the Delete-Token
        /// </summary>
        /// <returns></returns>
        private String getDeleteToken()
        {
            // only changed one time per session
            // so we don't need to change, if it exists
            if (this.deleteToken.Length > 0)
            {
                return this.deleteToken;
            }

            String webPageText = "";

            var data = new NameValueCollection();
            data["action"] = "query";
            data["prop"] = "info";
            data["intoken"] = "delete";
            data["titles"] = "Hauptseite";
            data["format"] = "json";

            Uri u = new Uri(this.sURL + "api.php");

            byte[] bytes = this.wc.UploadValues(u, data);

            webPageText = new System.Text.UTF8Encoding().GetString(bytes);
            var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

            this.deleteToken = json.Query.InfoPage.Page.Deletetoken;

            return this.deleteToken;

        }

        /// <summary>
        /// add a DataElement to the Mediawiki
        /// </summary>
        /// <param name="pElement"></param>
        /// <returns></returns>
        public DataElement addNewElement(DataElement pElement)
        {
            // first delete old version of the element
            // otherwise we only write behind the old text
            removeDataElement(pElement);

            // we have to build the content of the Element
            String sText = ""; // get a string out of the given element

            // the Infobox with the Image
            sText += "{{Kastendesign Bild SMW\n | BORDER = #97BF87\n | BACKGROUND = #AADDAA\n | BILD = Datei:" + pElement.getName() + ".jpeg|miniatur\n |PARAM1 = 123}}";

            // all Annotations of the Element
            sText += "== Untersegmente ==\n";
            foreach (Annotation ann in pElement.getAnnotations())
            {
                sText += "[[Datei:" + ann.ToString() + "]]<br>";
            }

            // all Links of the Element
            sText += "== Links ==\n";
            foreach (Annotation l in pElement.getLinks())
            {
                sText += "[[Links::" + l.ToString() + "]]<br>";
            }

            // all Attributs of the Element
            sText += "== Tags ==\n";
            foreach (Annotation att in pElement.getAttributes())
            {
                sText += "[[Tags::" + att.ToString() + "]]<br>";
            }

            // the rating-Image 
            // (current there will be saved the Image of the Rating, nothing else, maybe changed in the future)
            sText += "== Ratings ==\n {{Kastendesign Rating neu|\n BORDER = #97BF87| \n BACKGROUND = #AADDAA| \n RATING=";
            sText += pElement.getRating()[0].getName() + "| \n }}";

            byte[] bytes;
            String webPageText;

            // finish to upload the new version or the new Element to Mediawiki 
            using (this.wc)
            {
                // define Post-Parameters
                var data = new NameValueCollection();
                data["action"] = "edit";
                data["section"] = "Links";
                data["title"] = pElement.getName();
                data["text"] = sText;
                // zum Ändern einer Seite benötigt man einen Edit-Token. Dieser ist für alle Seiten der gleiche, allerdings Ändert er sich bei jedem Login. 
                data["token"] = this.getEditToken();
                data["format"] = "json";
                Uri u = new Uri(this.sURL + "api.php");

                bytes = this.wc.UploadValues(u, data);
                webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
            }

            return pElement;
        }

        /// <summary>
        /// remove a DataElement to the Mediawiki
        /// </summary>
        /// <param name="pElement"></param>
        /// <returns></returns>
        public bool removeDataElement(DataElement pElement)
        {
            byte[] bytes;
            String webPageText;
            try
            {
                using (this.wc)
                {
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "delete";
                    data["title"] = pElement.getName();
                    // to delete one Element, we need a Delete-Token
                    // the delete-Token only change one time of a session
                    data["token"] = this.getDeleteToken();
                    data["format"] = "json";
                    Uri u = new Uri(this.sURL + "api.php");

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                    var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// remove a Attribut to the Mediawiki
        /// </summary>
        /// <param name="pAttribut"></param>
        /// <returns></returns>
        public bool removeAttribut(Attribut pAttribut)
        {
            DataElement pElement = pAttribut.getAnnotationSubject();

            byte[] bytes;
            String webPageText;
            try
            {
                using (this.wc)
                {
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "edit";
                    data["section"] = "3";
                    data["title"] = pElement.getName();
                    String sText = "";
                    foreach (Annotation att in pElement.getAttributes())
                    {
                        if (att.ToString() != (pAttribut.getAnnotationObject()).ToString())
                        {
                            sText += "[[Tags::" + att.ToString() + "]]<br>";
                        }
                    }

                    data["text"] = sText;
                    // zum Ändern einer Seite benötigt man einen Edit-Token. Dieser ist für alle Seiten der gleiche, allerdings Ändert er sich bei jedem Login. 
                    data["token"] = this.getEditToken();
                    data["format"] = "json";
                    Uri u = new Uri(this.sURL + "api.php");

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                    var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// add a Link to the Mediawiki
        /// </summary>
        /// <param name="pLink"></param>
        /// <returns></returns>
        public Link addLink(Link pLink)
        {
            DataElement pElement = pLink.getAnnotationSubject();

            byte[] bytes;
            String webPageText;
            try
            {
                using (this.wc)
                {
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "edit";
                    data["section"] = "2";
                    data["title"] = pElement.getName();
                    String sText = "";
                    foreach (Annotation l in pElement.getLinks())
                    {
                        sText += "[[Links::" + l.ToString() + "]]<br>";
                    }
                    sText += "[[Links::" + pLink.getAnnotationObject().ToString() + "]]<br>";

                    data["text"] = sText;
                    // zum Ändern einer Seite benötigt man einen Edit-Token. Dieser ist für alle Seiten der gleiche, allerdings Ändert er sich bei jedem Login. 
                    data["token"] = this.getEditToken();
                    data["format"] = "json";
                    Uri u = new Uri(this.sURL + "api.php");

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                    var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
                }
                return pLink;
            }
            catch
            {
                return pLink;
            }
        }

        /// <summary>
        /// remove a Link to the Mediawiki
        /// </summary>
        /// <param name="pLink"></param>
        /// <returns></returns>
        public bool removeLink(Link pLink)
        {
            DataElement pElement = pLink.getAnnotationSubject();

            byte[] bytes;
            String webPageText;
            try
            {
                using (this.wc)
                {
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "edit";
                    data["section"] = "2";
                    data["title"] = pElement.getName();
                    String sText = "";
                    foreach (Annotation l in pElement.getLinks())
                    {
                        if (l.ToString() != (pLink.getAnnotationObject()).ToString())
                        {
                            sText += "[[Links::" + l.ToString() + "]]<br>";
                        }
                    }

                    data["text"] = sText;
                    // zum Ändern einer Seite benötigt man einen Edit-Token. Dieser ist für alle Seiten der gleiche, allerdings Ändert er sich bei jedem Login. 
                    data["token"] = this.getEditToken();
                    data["format"] = "json";
                    Uri u = new Uri(this.sURL + "api.php");

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                    var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// add a Annotation to the Mediawiki
        /// </summary>
        /// <param name="pAnnotation"></param>
        /// <returns></returns>
        public Annotation addAnnotation(Annotation pAnnotation)
        {
            DataElement pElement = pAnnotation.getAnnotationSubject();

            byte[] bytes;
            String webPageText;
            try
            {
                using (this.wc)
                {
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "edit";
                    data["section"] = "1";
                    data["title"] = pElement.getName();
                    String sText = "";
                    foreach (Annotation ann in pElement.getAnnotations())
                    {
                        sText += "[[Datei:" + ann.ToString() + "]]<br>";
                    }
                    sText += "[[Datei:" + pAnnotation.ToString() + "]]<br>";

                    data["text"] = sText;
                    // zum Ändern einer Seite benötigt man einen Edit-Token. Dieser ist für alle Seiten der gleiche, allerdings Ändert er sich bei jedem Login. 
                    data["token"] = this.getEditToken();
                    data["format"] = "json";
                    Uri u = new Uri(this.sURL + "api.php");

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                    var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
                }
                return pAnnotation;
            }
            catch
            {
                return pAnnotation;
            }
        }

        /// <summary>
        /// remove a Annotation to the Mediawiki
        /// </summary>
        /// <param name="pAnnotation"></param>
        /// <returns></returns>
        public bool removeAnnotation(Annotation pAnnotation)
        {
            DataElement pElement = pAnnotation.getAnnotationSubject();

            byte[] bytes;
            String webPageText;
            try
            {
                using (this.wc)
                {
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "edit";
                    data["section"] = "1";
                    data["title"] = pElement.getName();
                    String sText = "";
                    foreach (Annotation ann in pElement.getAnnotations())
                    {
                        if (ann.ToString() != pAnnotation.ToString())
                        {
                            sText += "[[Links::" + ann.ToString() + "]]<br>";
                        }
                    }

                    data["text"] = sText;
                    // zum Ändern einer Seite benötigt man einen Edit-Token. Dieser ist für alle Seiten der gleiche, allerdings Ändert er sich bei jedem Login. 
                    data["token"] = this.getEditToken();
                    data["format"] = "json";
                    Uri u = new Uri(this.sURL + "api.php");

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                    var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// add a Rating to the Mediawiki
        /// </summary>
        /// <param name="pRating"></param>
        /// <returns></returns>
        public Rating addRating(Rating pRating)
        {
            DataElement pElement = pRating.getElement();

            byte[] bytes;
            String webPageText;
            try
            {
                using (this.wc)
                {
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "edit";
                    data["section"] = "4";
                    data["title"] = pElement.getName();
                    String sText = "";
                    data["text"] = sText;
                    // zum Ändern einer Seite benötigt man einen Edit-Token. Dieser ist für alle Seiten der gleiche, allerdings Ändert er sich bei jedem Login. 
                    data["token"] = this.getEditToken();
                    data["format"] = "json";
                    Uri u = new Uri(this.sURL + "api.php");

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                    var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
                }
                return pRating;
            }
            catch
            {
                return pRating;
            }
        }

        /// <summary>
        /// remove a Rating to the Mediawiki
        /// </summary>
        /// <param name="pRating"></param>
        /// <returns></returns>
        public bool removeRating(Rating pRating)
        {
            DataElement pElement = pRating.getElement();

            byte[] bytes;
            String webPageText;
            try
            {
                using (this.wc)
                {
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "edit";
                    data["section"] = "4";
                    data["title"] = pElement.getName();
                    String sText = "";
                    data["text"] = sText;
                    // zum Ändern einer Seite benötigt man einen Edit-Token. Dieser ist für alle Seiten der gleiche, allerdings Ändert er sich bei jedem Login. 
                    data["token"] = this.getEditToken();
                    data["format"] = "json";
                    Uri u = new Uri(this.sURL + "api.php");

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                    var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Methode to add a User to the Mediawiki
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns>the added User</returns>
        public User addUser(User pUser)
        {
            // Problem, because action = createaccount is no action in this mediawiki
            try
            {
                string webPageText;
                byte[] bytes;

                using (this.wc)
                {
                    // define Post-Parameters
                    var data = new NameValueCollection();
                    data["action"] = "createaccount";
                    data["name"] = pUser.getUsername();
                    data["email"] = pUser.getUsermail();
                    data["realname"] = "";
                    data["mailpassword"] = "true";
                    data["language"] = "en";
                    data["token"] = "";
                    data["format"] = "json";

                    // First login for getting the Token
                    Uri u = new Uri(this.sURL + "api.php");

                    bytes = this.wc.UploadValues(u, data);
                    webPageText = new System.Text.UTF8Encoding().GetString(bytes);
                }
            }
            catch
            {
                
            }
            return pUser;
        }

        /// <summary>
        /// Get all Images of a category
        /// </summary>
        /// <param name="pCategory">a Category</param>
        /// <returns>List of Images</returns>
        public List<Image> getImages(Category pCategory)
        {
            List<Image> images = new List<Image>();

            byte[] bytes;
            string webPageText;
            string imgName;
            Uri u = new Uri(this.sURL + "api.php");

            using (this.wc)
            {
                // Use MediaWiki API to list all the DataElements of the given category
                // define Post-Parameters
                var data = new NameValueCollection();
                data["action"] = "query";
                data["list"] = "categorymembers";
                data["cmtitle"] = "Kategorie:" + (pCategory.getName()).Trim(new Char[] { ' ', '"', '\'' });
                data["cmlimit"] = "500";
                data["format"] = "json";

                bytes = this.wc.UploadValues(u, data);
                webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

                foreach (CategoryMember allcat in json.Query.Pages)
                {
                    if ((allcat.Title.ToString()).StartsWith("Kategorie:") == false)
                    {
                        // If Element is a Image then save as a Image

                        String sElement = getAllofElement(allcat.Title.ToString());
                        bool isImage = false;
                        int start_link_nr = 0;

                        // all possible spellings 
                        if (sElement.Contains("| BILD = "))
                        {
                            isImage = true;
                            start_link_nr = sElement.IndexOf("| BILD = ", 0) + 9;
                        }
                        else if (sElement.Contains("| BILD ="))
                        {
                            isImage = true;
                            start_link_nr = sElement.IndexOf("| BILD =", 0) + 8;
                        }
                        else if (sElement.Contains("| BILD="))
                        {
                            isImage = true;
                            start_link_nr = sElement.IndexOf("| BILD=", 0) + 7;
                        }
                        else if (sElement.Contains("| BILD= "))
                        {
                            isImage = true;
                            start_link_nr = sElement.IndexOf("| BILD= ", 0) + 8;
                        }

                        if (isImage)
                        {
                            // all possible endings
                            String fileType = ".jpeg";

                            if (sElement.Contains(".jpeg"))
                            {
                                fileType = ".jpeg";    
                            }
                            else if (sElement.Contains(".JPEG"))
                            {
                                fileType = ".JPEG";
                            }
                            else if (sElement.Contains(".jpg"))
                            {
                                fileType = ".jpg"; 
                            }
                            else if (sElement.Contains(".JPG"))
                            {
                                fileType = ".JPG";
                            }
                            else if (sElement.Contains(".png"))
                            {
                                fileType = ".png"; 
                            }
                            else if (sElement.Contains(".PNG"))
                            {
                                fileType = ".PNG";
                            }

                            int stop_link_nr = stop_link_nr = sElement.IndexOf(fileType, start_link_nr + 9) + fileType.Count();

                            imgName = sElement.Substring(start_link_nr, stop_link_nr - start_link_nr);

                            // define Post-Parameters
                            var data2 = new NameValueCollection();
                            data2["action"] = "query";
                            data2["list"] = "allimages";
                            data2["aiprefix"] = imgName.Substring(6);
                            data2["ailimit"] = "1";
                            data2["aiprop"] = "url";
                            data2["format"] = "json";

                            bytes = this.wc.UploadValues(u, data2);

                            webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                            json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);
                            Image tempImg = null;
                            try
                            {
                                //get all Informations about the Image
                                Dictionary<String, String> imgInfos = new Dictionary<String, String>();
                                webPageText = getAllofElement(imgName, "xml");
                                webPageText = webPageText.Replace("\n", "");

                                string[] sInformations = webPageText.Split('|');
                                List<string> sList = new List<string>(sInformations);
                                sList.RemoveAt(0); // deletes first not needed Element
                                sList.RemoveAt(sList.Count - 1); // deletes last not needed Element
                                sList.Sort();

                                foreach (string sInfo in sList)
                                {
                                    string[] sInfoteil = sInfo.Split('=');
                                    if (sInfoteil.Length > 1)
                                    {
                                       imgInfos[sInfoteil[0]] = sInfoteil[1];
                                    }
                                }

                                if (json.Query.Images.Length > 0)
                                {
                                    if (imgInfos.Count > 0)
                                    {
                                        tempImg = new Impl_Image(this, new Uri(Wikinect.WikiNectConst.sImageNamespace + allcat.PageId), imgName.Substring(6), "", json.Query.Images[0].Url, imgInfos);
                                    }
                                    else
                                    {
                                        tempImg = new Impl_Image(this, new Uri(Wikinect.WikiNectConst.sImageNamespace + allcat.PageId), imgName.Substring(6), "", json.Query.Images[0].Url);
                                    }
                                }

                            }
                            catch (Exception e)
                            {
                                Console.Error.WriteLine(e);
                            }
                            if(tempImg!=null){
                                images.Add(tempImg);
                            }

                            
                        }
                    }
                }
            }
        return images;
        }

        /// <summary>
        /// get a Random Image of a Element, if only one Image is needed
        /// </summary>
        /// <param name="pCategory"></param>
        /// <returns></returns>
        public Image getRandomImage(Category pCategory)
        {
            byte[] bytes;
            string webPageText;
            string imgName;
            Uri u = new Uri(this.sURL + "api.php");

            Image tempImg = null;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // Use MediaWiki API to list all the DataElements of the given category
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            using (this.wc)
            {
                // define Post-Parameters
                var data = new NameValueCollection();
                data["action"] = "query";
                data["list"] = "categorymembers";
                data["cmtitle"] = "Kategorie:" + (pCategory.getName()).Trim(new Char[] { ' ', '"', '\'' });
                data["cmlimit"] = "500";
                data["format"] = "json";

                bytes = this.wc.UploadValues(u, data);
                webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                var json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

                foreach (CategoryMember allcat in json.Query.Pages)
                {
                    if ((allcat.Title.ToString()).StartsWith("Kategorie:") == false)
                    {
                        // If Element is a Image then save as a Image

                        String sElement = getAllofElement(allcat.Title.ToString());
                        bool isImage = false;
                        int start_link_nr = 0;

                        if (sElement.Contains("| BILD = "))
                        {
                            isImage = true;
                            start_link_nr = sElement.IndexOf("| BILD = ", 0) + 9;
                        }
                        else if (sElement.Contains("| BILD ="))
                        {
                            isImage = true;
                            start_link_nr = sElement.IndexOf("| BILD =", 0) + 8;
                        }
                        else if (sElement.Contains("| BILD="))
                        {
                            isImage = true;
                            start_link_nr = sElement.IndexOf("| BILD=", 0) + 7;
                        }
                        else if (sElement.Contains("| BILD= "))
                        {
                            isImage = true;
                            start_link_nr = sElement.IndexOf("| BILD= ", 0) + 8;
                        }

                        if (isImage)
                        {

                            String fileType = ".jpeg";

                            if (sElement.Contains(".jpeg"))
                            {
                                fileType = ".jpeg";
                            }
                            else if (sElement.Contains(".JPEG"))
                            {
                                fileType = ".JPEG";
                            }
                            else if (sElement.Contains(".jpg"))
                            {
                                fileType = ".jpg";
                            }
                            else if (sElement.Contains(".JPG"))
                            {
                                fileType = ".JPG";
                            }
                            else if (sElement.Contains(".png"))
                            {
                                fileType = ".png";
                            }
                            else if (sElement.Contains(".PNG"))
                            {
                                fileType = ".PNG";
                            }

                            int stop_link_nr = stop_link_nr = sElement.IndexOf(fileType, start_link_nr + 9) + fileType.Count();

                            imgName = sElement.Substring(start_link_nr, stop_link_nr - start_link_nr);

                            // define Post-Parameters
                            var data2 = new NameValueCollection();
                            data2["action"] = "query";
                            data2["list"] = "allimages";
                            data2["aiprefix"] = imgName.Substring(6);
                            data2["ailimit"] = "1";
                            data2["aiprop"] = "url";
                            data2["format"] = "json";

                            bytes = this.wc.UploadValues(u, data2);

                            webPageText = new System.Text.UTF8Encoding().GetString(bytes);

                            json = JsonConvert.DeserializeObject<MediaWiki>(webPageText);

                            if (json.Query.Images.Length > 0)
                            {
                                
                                try
                                {
                                    int count = 0;
                                    
                                    
                                    tempImg = new Impl_Image(this, new Uri(Wikinect.WikiNectConst.sImageNamespace + allcat.PageId), imgName.Substring(6), "", json.Query.Images[count].Url);
                                    return tempImg;

                                }
                                catch (Exception e)
                                {
                                    Console.Error.WriteLine(e);
                                }                        


                            }

                        }
                    }
                }
            }

            return tempImg;    

        }
    }
}
