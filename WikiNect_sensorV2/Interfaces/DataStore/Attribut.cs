using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    /// <summary>
    /// Interface for all Attributes
    /// </summary>
    public interface Attribut : Annotation
    {
        /// <summary>
        /// Get the AnnotationObject
        /// </summary>
        /// <returns></returns>
        Object getAnnotationObject();
    
    }
}
