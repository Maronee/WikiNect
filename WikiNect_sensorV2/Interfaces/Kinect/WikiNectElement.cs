using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataConnection;
using Wikinect;

namespace DataStore
{
    /// <summary>
    /// Zentrales Interface für Elemente in WikiNect
    /// </summary>

    public interface WikiNectElement
    {
        /// <summary>
        /// Get URI of Element
        /// </summary>
        Uri elementUri
        {
            get;
        }
        /// <summary>
        /// Get Connection-Object
        /// </summary>
        /// <returns>Connection</returns>
        Connection getConnection();
    }
}
