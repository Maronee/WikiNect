using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    /// <summary>
    /// Interface for Annotations
    /// </summary>
    public interface Annotation:WikiNectElement
    {
        /// <summary>
        /// Get the User who create the Annotation
        /// </summary>
        User User
        {
            get;
        }

        /// <summary>
        /// Get the Annotationstype
        /// </summary>
        AnnotationType type
        {
            get;
        }

        /// <summary>
        /// Get the AnnotationsSubject (S)
        /// </summary>
        /// <returns></returns>
        DataElement getAnnotationSubject();

        /// <summary>
        /// Get and Set the Annotationsproperty (p)
        /// </summary>
        Uri annotationProperty
        {
            get;
            set;
        }

    }
}
