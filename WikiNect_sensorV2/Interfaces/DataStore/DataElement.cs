using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DataStore
{
    /// <summary>
    /// Interface for all DataElements
    /// </summary>

    public interface DataElement:WikiNectElement
    {
        /// <summary>
        /// Method to set the Name of the Element
        /// </summary>
        /// <param name="sName"></param>
        void setName(String sName);

        /// <summary>
        /// Method to set the Description of the Element
        /// </summary>
        /// <param name="sDescription"></param>
        void setDesctiption(String sDescription);

        /// <summary>
        /// Method to get the Name of the Element
        /// </summary>
        /// <returns></returns>
        String getName();

        /// <summary>
        /// Method to set the Description of the Element
        /// </summary>
        /// <returns></returns>
        String getDescription();

        /// <summary>
        /// Method to set the Annotations of the Element
        /// </summary>
        /// <param name="pAnnotationList"></param>
        void setAnnotations(List<Annotation> pAnnotationList);

        /// <summary>
        /// Add one Annotation to the Element
        /// </summary>
        /// <param name="pAnnotation"></param>
        void addAnnotation(Annotation pAnnotation);

        /// <summary>
        /// Add one or more Annotations of the Element
        /// </summary>
        /// <param name="pAnnotationList"></param>
        void addAnnotation(List<Annotation> pAnnotationList);

        /// <summary>
        /// Method to get all Ratings of the Element
        /// </summary>
        /// <returns></returns>
        List<Rating> getRating();
        
        /// <summary>
        /// Method to get all Annotations of the Element
        /// </summary>
        /// <returns></returns>
        List<Annotation> getAnnotations();

        /// <summary>
        /// Method to get all the Attributes of the Element
        /// </summary>
        /// <returns></returns>
        List<Attribut> getAttributes();

        /// <summary>
        /// Method to get all the Attributes of the Element
        /// </summary>
        /// <returns></returns>
        List<Link> getLinks();

        /// <summary>
        /// Method to Update the Element. Compare with Database and set it up to date!
        /// </summary>
        void update();

    }
}
