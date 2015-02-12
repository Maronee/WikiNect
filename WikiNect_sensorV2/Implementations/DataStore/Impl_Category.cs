using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    class CategoryImpl:ImplDataElement,Category
    {

        private List<Category> superCategories = new List<Category>();
        private List<Category> subCategories = new List<Category>();

        public CategoryImpl(DataConnection.Connection connection, Uri uri, String sName, String sDescription)
            : base(connection, uri, sName, sDescription)
        {
                    
        }

        private void init()
        {

            this.subCategories = this.connection.getSubCategories(this);
            this.subCategories = this.connection.getSuperCategories(this);    
        
        }

        public void setSuperCategories(List<Category> pCategoryList)
        {
            this.superCategories = pCategoryList;
        }

        public List<Category> getSuperCategories()
        {
            return this.superCategories;
        }

        public void setSubCategories(List<Category> pCategoryList)
        {
            this.subCategories = pCategoryList;
        }

        public List<Category> getSubCategories()
        {
            return this.subCategories;
        }

        public List<DataElement> getElements()
        {

            List<DataElement> returnList = new List<DataElement>();

            foreach(Category c in subCategories)
            {
                returnList.AddRange(c.getElements());    
            }

            return returnList;

        }

        public List<DataElement> getElements(bool bRecursive)
        {

            List<DataElement> returnList = new List<DataElement>();

            foreach (Category c in subCategories)
            {
                if (c.getSubCategories().Count > 0)
                {
                    returnList.AddRange(c.getElements(bRecursive));
                }
                else
                {
                    returnList.AddRange(c.getElements());
                }
                
            }

            return returnList;

        }

        public void addSuperCategorie(List<Category> pCategoryList)
        {
            this.superCategories.AddRange(pCategoryList);
        }

        public void addSuperCategorie(Category pCategory)
        {
            this.superCategories.Add(pCategory);
        }

        public void addSubCategorie(List<Category> pCategoryList)
        {
            this.subCategories.AddRange(pCategoryList);
        }

        public void addSubCategorie(Category pCategory)
        {
            this.subCategories.Add(pCategory);
        }

        public void removeSuperCategorie(List<Category> pCategoryList)
        {
            foreach (Category c in pCategoryList)
            {
                removeSuperCategorie(c);
            }
        }

        public void removeSuperCategorie(Category pCategory)
        {
            this.superCategories.Remove(pCategory);
        }

        public void removeSubCategorie(List<Category> pCategoryList)
        {
            foreach (Category c in pCategoryList)
            {
                removeSubCategorie(c);
            }            
        }

        public void removeSubCategorie(Category pCategory)
        {
            this.subCategories.Remove(pCategory);
        }


        void Category.setSuperCategories(List<Category> pCategoryList)
        {
            this.superCategories = pCategoryList;
        }

        List<Category> Category.getSuperCategories()
        {
            return this.superCategories;
        }

        void Category.setSubCategories(List<Category> pCategoryList)
        {
            this.subCategories = pCategoryList;
        }

        List<Category> Category.getSubCategories()
        {
            return this.getSubCategories();
        }

        List<DataElement> Category.getElements()
        {
            return getElements(false);
        }

        List<DataElement> Category.getElements(bool bRecursive)
        {
            List<DataElement> returnList = new List<DataElement>();

            foreach (Category c in subCategories)
            {
                returnList.AddRange(c.getElements(bRecursive));
            }

            return returnList;
        }

        void Category.addSuperCategorie(List<Category> pCategoryList)
        {
            this.superCategories.AddRange(pCategoryList);
        }

        void Category.addSuperCategorie(Category pCategory)
        {
            this.superCategories.Add(pCategory);
        }

        void Category.addSubCategorie(List<Category> pCategoryList)
        {
            this.subCategories.AddRange(pCategoryList);
        }

        void Category.addSubCategorie(Category pCategory)
        {
            this.subCategories.Add(pCategory);
        }

        void Category.removeSuperCategorie(List<Category> pCategoryList)
        {
            foreach (Category c in pCategoryList)
            {
                this.removeSuperCategorie(c);
            }
        }

        void Category.removeSuperCategorie(Category pCategory)
        {
            this.superCategories.Remove(pCategory);
        }

        void Category.removeSubCategorie(List<Category> pCategoryList)
        {
            foreach (Category c in pCategoryList)
            {
                this.removeSubCategorie(c);
            }
        }

        void Category.removeSubCategorie(Category pCategory)
        {
            this.subCategories.Remove(pCategory);
        }


        void DataElement.update()
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            return this.getName();
        }


        public DataElement addElement(DataElement pElement)
        {
            throw new NotImplementedException();
        }
    }
}
