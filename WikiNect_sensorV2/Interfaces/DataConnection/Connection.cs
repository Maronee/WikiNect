using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStore;

namespace DataConnection
{
    /// <summary>
    /// Interface for any Connection
    /// </summary>
    public interface Connection
    {

        bool login();

        bool login(String user, String password);

        void logout();

        /// <summary>
        /// Get all super-categories, based on Category
        /// </summary>
        /// <param name="pCategory">Category</param>
        /// <returns></returns>
        List<Category> getSuperCategories(Category pCategory);

        /// <summary>
        /// Get all super-categories, based on CategoryName
        /// </summary>
        /// <param name="pCategory">Category</param>
        /// <returns></returns>
        List<Category> getSuperCategories(string nameCategory);

        /// <summary>
        /// Get all sub-categories, based on Category
        /// </summary>
        /// <param name="pCategory"></param>
        /// <returns></returns>
        List<Category> getSubCategories(Category pCategory);

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        List<Category> getCategories();

        /// <summary>
        /// Get all categories of Element
        /// </summary>
        /// <returns></returns>
        List<Category> getCategories(DataElement pElement);
        
         /// <summary>
        /// Get all categories of Element
        /// </summary>
        /// <returns></returns>
        List<Category> getCategories(string nameElement);

        /// <summary>
        /// Get all categories based on the category-id
        /// </summary>
        /// <param name="lCategory"></param>
        /// <returns></returns>
        Category getCategory(long lCategory);

        /// <summary>
        /// Get all DataElements based on category-id
        /// </summary>
        /// <param name="lCategory"></param>
        /// <returns></returns>
        List<DataElement> getElements(long lCategory);

        /// <summary>
        /// Get all DataElements based on id-list of categories
        /// </summary>
        /// <param name="pListCategory"></param>
        /// <returns></returns>
        List<DataElement> getElements(List<long> pListCategory);

        /// <summary>
        /// Get all DataElements based on Category
        /// </summary>
        /// <param name="pCategory"></param>
        /// <returns></returns>
        List<DataElement> getElements(Category pCategory);

        

        /// <summary>
        /// Get all DataElements based on the list of Categories
        /// </summary>
        /// <param name="pListCategory"></param>
        /// <returns></returns>
        List<DataElement> getElements(List<Category> pListCategory);

        /// <summary>
        /// Get all Ratings
        /// </summary>
        /// <returns></returns>
        List<Rating> getRatings();
        
        /// <summary>
        /// Get Rating based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Rating getRating(long id);

        /// <summary>
        /// Get all Ratings of pElement
        /// </summary>
        /// <param name="pElement"></param>
        /// <returns></returns>
        List<Rating> getRatings(DataElement pElement);

        /// <summary>
        /// Get all Ratings of all Elements in Category
        /// </summary>
        /// <param name="pCategory"></param>
        /// <returns></returns>
        List<Rating> getRatings(Category pCategory);

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns></returns>
        List<User> getUsers();
        
        /// <summary>
        /// Get User based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User getUser(long id);

        /// <summary>
        /// Get all Annotations
        /// </summary>
        /// <returns></returns>
        List<Annotation> getAnnotations();

        /// <summary>
        /// Get all Annotations
        /// </summary>
        /// <returns></returns>
        List<Annotation> getAnnotations(bool bAll);
        
        /// <summary>
        /// Get all Annotations based on User
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns></returns>
        List<Annotation> getAnnotations(User pUser);


        /// <summary>
        /// Get all Annotations based on User
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns></returns>
        List<Annotation> getAnnotations(User pUser, bool bAll);

        /// <summary>
        /// Get all Annotations based on User and AnnotationType
        /// </summary>
        /// <param name="pUser"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        List<Annotation> getAnnotations(User pUser, AnnotationType pType);

        /// <summary>
        /// Get all Annotations based on User, AnnotationType and the given DataElement
        /// </summary>
        /// <param name="pUser"></param>
        /// <param name="pType"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        List<Annotation> getAnnotations(User pUser, AnnotationType pType, DataElement element);

        /// <summary>
        /// Get all Annotations based on DataElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        List<Annotation> getAnnotations(DataElement element);

        /// <summary>
        /// Get all Annotations based on DataElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        List<Annotation> getAnnotations(DataElement element, bool bAll);

        /// <summary>
        /// Get all Annotations based on DataElement and AnnotationType
        /// </summary>
        /// <param name="element"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        List<Annotation> getAnnotations(DataElement element, AnnotationType pType);

        /// <summary>
        /// Get all Annotations based on AnnotationType
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        List<Annotation> getAnnotations(AnnotationType pType);

        /// <summary>
        /// Get all Links
        /// </summary>
        /// <returns></returns>
        List<Annotation> getLinks();

        /// <summary>
        /// Get all Links based on User
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns></returns>
        List<Annotation> getLinks(User pUser);

        /// <summary>
        /// Get all Links based on User, AnnotationType and the given DataElement
        /// </summary>
        /// <param name="pUser"></param>
        /// <param name="pType"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        List<Annotation> getLinks(User pUser, AnnotationType pType, DataElement element);

        /// <summary>
        /// Get all Links based on DataElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        List<Annotation> getLinks(DataElement element);

        /// <summary>
        /// Get all Links based on DataElement and a AnnotationType
        /// </summary>
        /// <param name="element"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        List<Annotation> getLinks(DataElement element, AnnotationType pType);

        /// <summary>
        /// Get all Links based on AnnotationType
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        List<Annotation> getLinks(AnnotationType pType);

        /// <summary>
        /// Get all Attributes
        /// </summary>
        /// <returns></returns>
        List<Attribut> getAttributs();

        /// <summary>
        /// Get all Attributes based on User
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns></returns>
        List<Attribut> getAttributs(User pUser);

        /// <summary>
        /// Get all Attributes based on User, AnnotationType and DataElement
        /// </summary>
        /// <param name="pUser"></param>
        /// <param name="pType"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        List<Attribut> getAttributs(User pUser, AnnotationType pType, DataElement element);

        /// <summary>
        /// Get all Attributes based on DataElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        List<Attribut> getAttributs(DataElement element);

        /// <summary>
        /// Get all Attributes based on DataElement and AnnotationType
        /// </summary>
        /// <param name="element"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        List<Attribut> getAttributs(DataElement element, AnnotationType pType);

        /// <summary>
        /// Method to add a new Data-Element
        /// </summary>
        /// <param name="pElement"></param>
        /// <returns></returns>
        DataElement addNewElement(DataElement pElement);

        /// <summary>
        /// Method to remove a DataElement
        /// </summary>
        /// <param name="pElement"></param>
        /// <returns></returns>
        bool removeDataElement(DataElement pElement);

        /// <summary>
        /// Method to remove a Attribute
        /// </summary>
        /// <param name="pAttribut"></param>
        /// <returns></returns>
        bool removeAttribut(Attribut pAttribut);

        /// <summary>
        /// Method to remove a Link
        /// </summary>
        /// <param name="pLink"></param>
        /// <returns></returns>
        bool removeLink(Link pLink);

        /// <summary>
        /// Method to remove a Annotation
        /// </summary>
        /// <param name="pAnnotation"></param>
        /// <returns></returns>
        bool removeAnnotation(Annotation pAnnotation);

        /// <summary>
        /// Method to remove a Rating
        /// </summary>
        /// <param name="pRating"></param>
        /// <returns></returns>
        bool removeRating(Rating pRating);

        /// <summary>
        /// Method to add a User
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns></returns>
        User addUser(User pUser);

        /// <summary>
        /// Method to add a Rating
        /// </summary>
        /// <param name="pRating"></param>
        /// <returns></returns>
        Rating addRating(Rating pRating);

        /// <summary>
        /// Method to add a Annotation
        /// </summary>
        /// <param name="pAnnotation"></param>
        /// <returns></returns>
        Annotation addAnnotation(Annotation pAnnotation);

        /// <summary>
        /// Method to add a Link
        /// </summary>
        /// <param name="pLink"></param>
        /// <returns></returns>
        Link addLink(Link pLink);

        /// <summary>
        /// Get all Images based on Category
        /// </summary>
        /// <param name="pCategory"></param>
        /// <returns></returns>
        List<Image> getImages(Category pCategory);


        /// <summary>
        /// Get all Images based on Category
        /// </summary>
        /// <param name="pCategory"></param>
        /// <returns></returns>
        Image getRandomImage(Category pCategory);
    }
}
