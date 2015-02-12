using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    /// <summary>
    /// Interface for all Links
    /// </summary>
    public interface Link : Annotation
    {
        /// <summary>
        /// Get the Annotatet Object
        /// </summary>
        /// <returns></returns>
        WikiNectElement getAnnotationObject();


    }
}
