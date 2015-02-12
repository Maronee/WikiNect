using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    /// <summary>
    /// Interface for all Users
    /// </summary>
    public interface User:WikiNectElement
    {
        /// <summary>
        /// Set or get the Username
        /// </summary>
        
        String getUsername();
        void setUsername(String sUsername);

        /// <summary>
        /// Set or get the Usermail for password
        /// </summary>

        String getUsermail();
        void setUsermail(String sUsermail);

        /// <summary>
        /// get the UserId
        /// </summary>
        long getUserId();

        /// <summary>
        /// Get or Set if the User is Anonymous
        /// </summary>
        bool getAnonymous();
        void setAnonymous(bool bValue);

        /// <summary>
        /// Get all Annotations who are created from the User
        /// </summary>
        List<Annotation> getAnnotations();

        /// <summary>
        /// Get all Ratings who are createt from the User
        /// </summary>
        List<Rating> getRatings();

    }
}
