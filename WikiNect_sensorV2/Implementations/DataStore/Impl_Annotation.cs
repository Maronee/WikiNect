using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    class Impl_Annotation : ImplContentElement, Annotation
    {
        public Impl_Annotation(DataConnection.Connection connection, Uri uri, String sName, String sDescription)
            : base(connection, uri, sName, sDescription) 
        { 
        
            
    
        }

        public override String ToString()
        {
            return this.getName();
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
