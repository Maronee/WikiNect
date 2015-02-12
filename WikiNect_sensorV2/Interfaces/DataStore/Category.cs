using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    /// <summary>
    /// Interface for Categories
    /// </summary>
    public interface Category : DataElement
    {

        /// <summary>
        /// Method to set all Super-Categories based on the Categorie
        /// </summary>
        /// <param name="pCategoryList"></param>
        void setSuperCategories(List<Category> pCategoryList);

        /// <summary>
        /// Method to get all Super-Categories based on the Categorie
        /// </summary>
        /// <returns></returns>
        List<Category> getSuperCategories();

        /// <summary>
        /// Method to set all Sub-Categories based on the Categorie
        /// </summary>
        /// <param name="pCategoryList"></param>
        void setSubCategories(List<Category> pCategoryList);

        /// <summary>
        /// Method to add a Element to a the Category
        /// </summary>
        /// <param name="pElement"></param>
        /// <returns></returns>
        DataElement addElement(DataElement pElement);

        /// <summary>
        /// Mehtod to get all Sub-Categories based on the Categorie
        /// </summary>
        /// <returns></returns>
        List<Category> getSubCategories();

        /// <summary>
        /// Method to get all Elements in the Categorie
        /// </summary>
        /// <returns></returns>
        List<DataElement> getElements();

        /// <summary>
        /// Method to get all Elements in the Categorie and if the parameter is true, also from all sub categories recursive
        /// </summary>
        /// <param name="bRecursive"></param>
        /// <returns></returns>
        List<DataElement> getElements(bool bRecursive);
        
        /// <summary>
        /// Method to add Super-Categories to the Category
        /// </summary>
        /// <param name="pCategoryList"></param>
        void addSuperCategorie(List<Category> pCategoryList);
        
        /// <summary>
        /// Method to add a Super-Category to the Category
        /// </summary>
        /// <param name="pCategory"></param>
        void addSuperCategorie(Category pCategory);

        /// <summary>
        /// Method to add Sub-Categories to the Category
        /// </summary>
        /// <param name="pCategoryList"></param>
        void addSubCategorie(List<Category> pCategoryList);

        /// <summary>
        /// Method to add a Sub-Category to the Category
        /// </summary>
        /// <param name="pCategory"></param>
        void addSubCategorie(Category pCategory);

        /// <summary>
        /// Method to remove Super-Categories from the Category
        /// </summary>
        /// <param name="pCategoryList"></param>
        void removeSuperCategorie(List<Category> pCategoryList);

        /// <summary>
        /// Method to remove a Super-Category from the Category
        /// </summary>
        /// <param name="pCategory"></param>
        void removeSuperCategorie(Category pCategory);

        /// <summary>
        /// Method to remove Sub-Categories from the Category
        /// </summary>
        /// <param name="pCategoryList"></param>
        void removeSubCategorie(List<Category> pCategoryList);

        /// <summary>
        /// Method to remove a Sub-Category from the Category
        /// </summary>
        /// <param name="pCategory"></param>
        void removeSubCategorie(Category pCategory);


    }
}
