using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    /// <summary>
    /// Interface for all ContentElements (Documents, Images)
    /// </summary>
    public interface ContentElement:DataElement
    {
        /// <summary>
        /// Get all Categories
        /// </summary>
        /// <returns></returns>
        List<Category> getCategories();

        /// <summary>
        /// Add a Categorie
        /// </summary>
        /// <param name="pCategory"></param>
        void addCategory(Category pCategory);

        /// <summary>
        /// Set a Category
        /// </summary>
        /// <param name="pListCategory"></param>
        void setCategory(List<Category> pListCategory);

        /// <summary>
        /// Remove the Category from Element
        /// </summary>
        /// <param name="pCategory"></param>
        void removeFromCategory(Category pCategory);

        /// <summary>
        /// Remove the Categories from Element
        /// </summary>
        /// <param name="pListCategory"></param>
        void removeFromCategory(List<Category> pListCategory);

    }
}
