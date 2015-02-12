using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DataStore
{
    public class ImplDataElement:DataElement,IComparable
    {

        protected DataConnection.Connection connection = null;

        private Uri uri = null;
        private String sName = "";
        private String sDescription = "";

        private List<Annotation> annotations = new List<Annotation>();


        protected ImplDataElement(DataConnection.Connection connection, Uri uri, String sName, String sDescription)
        {
            this.connection = connection;
            this.elementUri = uri;
            this.setName(sName);
            this.setDesctiption(sDescription);
        }

        public DataConnection.Connection getConnection()
        {
            return this.connection;
        }

        public void setName(string sName)
        {
            this.sName = sName;
        }

        public void setDesctiption(string sDescription)
        {
            this.sDescription = sDescription;
        }

        public string getName()
        {
            return this.sName;
        }

        public string getDescription()
        {
            return this.sDescription;
        }

        /// <summary>
        /// save the Element in Mediawiki
        /// </summary>
        public void update()
        {
            this.connection.addNewElement(this);
        }

        public List<Annotation> getAnnotations()
        {
            return this.annotations;
        }

        public void setAnnotations(List<Annotation> pAnnotationList)
        {
            this.annotations = pAnnotationList;
        }

        public void addAnnotation(Annotation pAnnotation)
        {
            this.annotations.Add(pAnnotation);
        }

        public void addAnnotation(List<Annotation> pAnnotationList)
        {
            this.annotations.AddRange(pAnnotationList);
        }


        public List<Attribut> getAttributes()
        {
            List<Attribut> rList = new List<Attribut>();

            foreach (Annotation a in annotations)
            {
                if (a is Attribut)
                {
                    rList.Add((Attribut)a);
                }
            }

            return rList;
        }

        public List<Link> getLinks()
        {
            List<Link> rList = new List<Link>();

            foreach (Annotation a in annotations)
            {
                if (a is Attribut)
                {
                    rList.Add((Link)a);
                }
            }

            return rList;

        }


        public List<Rating> getRating()
        {
            throw new NotImplementedException();
        }

        public Uri elementUri
        {
            get { return this.uri; }
            set { this.uri = value; }

        }

        public override int GetHashCode()
        {
            return elementUri.GetHashCode();
        }

        public int CompareTo(Object d)
        {
            int hc = this.GetHashCode().CompareTo(d.GetHashCode());
            Console.WriteLine("HC: " + hc);
            return hc;

        }
    }
}
