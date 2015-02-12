using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    class Impl_Attribut : ImplContentElement, Attribut
    {
        public Impl_Attribut(DataConnection.Connection connection, Uri uri, String sName, String sDescription)
            : base(connection, uri, sName, sDescription) 
        { 
        
            
    
        }

        object Attribut.getAnnotationObject()
        {
            throw new NotImplementedException();
        }

        User Annotation.User
        {
            get { throw new NotImplementedException(); }
        }

        AnnotationType Annotation.type
        {
            get { throw new NotImplementedException(); }
        }

        DataElement Annotation.getAnnotationSubject()
        {
            throw new NotImplementedException();
        }

        Uri Annotation.annotationProperty
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

        Uri WikiNectElement.elementUri
        {
            get { throw new NotImplementedException(); }
        }

        DataConnection.Connection WikiNectElement.getConnection()
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            return this.getName();
        }
    }
}
