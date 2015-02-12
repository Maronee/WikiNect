using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    class Impl_Link : ImplDataElement, Link
    {

        public Impl_Link(DataConnection.Connection connection, Uri uri, String sName, String sDescription)
            : base(connection, uri, sName, sDescription)
        {
                
        }

        public WikiNectElement getAnnotationObject()
        {
            throw new NotImplementedException();
        }

        public User User
        {
            get { throw new NotImplementedException(); }
        }

        public AnnotationType type
        {
            get { throw new NotImplementedException(); }
        }

        public DataElement getAnnotationSubject()
        {
            throw new NotImplementedException();
        }

        public Uri annotationProperty
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
