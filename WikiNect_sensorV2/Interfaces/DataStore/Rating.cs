using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DataStore
{
    /// <summary>
    /// Interface for all Ratings
    /// </summary>
    public interface Rating : WikiNectElement
    {
        /// <summary>
        /// Get the Related Element
        /// </summary>
        /// <returns></returns>
        DataElement getElement();

        /// <summary>
        /// Method to get the Name of the Element
        /// </summary>
        /// <returns></returns>
        String getName();

        /// <summary>
        /// List the Rating-Categories
        /// </summary>
        /// <returns></returns>
        List<RatingCategory> getRatingCategory();

        /// <summary>
        /// Get the rating image
        /// </summary>
        /// <returns></returns>
        System.Drawing.Image getRatingImage();

        /// <summary>
        /// Set the rating image
        /// </summary>
        /// <returns></returns>
        void setRatingImage(System.Drawing.Image img);

        /// <summary>
        /// Get the Rating-Value
        /// </summary>
        /// <returns></returns>
        int getRatingValue();

        /// <summary>
        /// Return the Time-Stamp of Rating
        /// </summary>
        /// <returns></returns>
        DateTime getTimeStamp();

        /// <summary>
        /// Get the corresponding User
        /// </summary>
        /// <returns></returns>
        User getUser();

        /// <summary>
        /// get the RatingId
        /// </summary>
        long getRatingId();

    }
}
