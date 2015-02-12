using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DataStore
{
    class Impl_Rating : Rating
    {
        private Uri uri;
        private string sName;
        private string sDescription;
        private DataConnection.Connection connection;
        private string url;
        System.Drawing.Image img;
        long ratingId = -1L;

        public Impl_Rating(DataConnection.Connection connection, Uri uri, String sName, String sDescription, System.Drawing.Image img, String url)
        {
            this.connection = connection;
            this.uri = uri;
            this.sName = sName;
            this.sDescription = sDescription;
            this.setRatingImage(img);
            this.url = url;

        }

        System.Drawing.Image getRatingImage()
        {
            return this.img;
        }

        void setRatingImage(System.Drawing.Image img)
        {
            this.img = img;
        }

        public DataElement getElement()
        {
            throw new NotImplementedException();
        }

        public string getName()
        {
            throw new NotImplementedException();
        }

        public List<RatingCategory> getRatingCategory()
        {
            throw new NotImplementedException();
        }

        System.Drawing.Image Rating.getRatingImage()
        {
            throw new NotImplementedException();
        }

        void Rating.setRatingImage(System.Drawing.Image img)
        {
            throw new NotImplementedException();
        }

        public int getRatingValue()
        {
            throw new NotImplementedException();
        }

        public DateTime getTimeStamp()
        {
            throw new NotImplementedException();
        }

        public User getUser()
        {
            throw new NotImplementedException();
        }

        public Uri elementUri
        {
            get { throw new NotImplementedException(); }
        }

        public DataConnection.Connection getConnection()
        {
            throw new NotImplementedException();
        }


        public long getRatingId()
        {
            return ratingId;
        }
    }
}