using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataConnection;

namespace DataStore
{
    class Impl_User : User
    {

        String sUsername = "";
        String sUsermail = "";

        long userId = -1L;

        private Connection pConnection = null;

        bool isAnonymous = false;

        public Impl_User(long id, String userName, Connection pConnection)
        {
            userId = id;
            setUsername(userName);
            this.pConnection = pConnection;
        }

        public override String ToString()
        {
            return this.elementUri.ToString();
        }

        public string getUsername()
        {
            return sUsername;
        }

        public void setUsername(string sUsername)
        {
            this.sUsername = sUsername;
        }

        public string getUsermail()
        {
            return sUsermail;
        }

        public void setUsermail(string sUsermail)
        {
            this.sUsermail = sUsermail;
        }

        public long getUserId()
        {
            return userId;
        }

        public bool getAnonymous()
        {
            return this.isAnonymous;
        }

        public void setAnonymous(bool bValue)
        {
            this.isAnonymous = bValue;
        }

        public List<Annotation> getAnnotations()
        {
            throw new NotImplementedException();
        }

        public List<Rating> getRatings()
        {
            throw new NotImplementedException();
        }


        public List<Annotation> Annotations
        {
            get { throw new NotImplementedException(); }
        }

        public List<Rating> Ratings
        {
            get { throw new NotImplementedException(); }
        }

        public Uri elementUri
        {
            get { return getURI(); }
        }

        private Uri getURI()
        {
            return new Uri(Wikinect.WikiNectConst.sUserNamespace + getUserId());
        }

        public DataConnection.Connection getConnection()
        {
            return this.pConnection;   
        }

    }
}
