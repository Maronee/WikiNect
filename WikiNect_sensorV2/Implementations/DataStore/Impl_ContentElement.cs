using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    public class ImplContentElement : ImplDataElement, ContentElement
    {

        List<Category> categories = new List<Category>();

        protected ImplContentElement(DataConnection.Connection connection, Uri uri, String sName, String sDescription)
            : base(connection, uri, sName, sDescription) 
        { 
                
        }

        public List<Category> getCategories()
        {
            return categories;    
        }

        public void addCategory(Category pCategory)
        {
            this.categories.Add(pCategory);
        }

        public void removeFromCategory(Category pCategory)
        {
            categories.Remove(pCategory);
        }

        public void removeFromCategory(List<Category> pListCategory)
        {
            
            foreach(Category c in pListCategory)
            {
                this.removeFromCategory(c);
            }

        }

        public void setCategory(List<Category> pListCategory)
        {
            this.categories = pListCategory;
        }
    }
}
