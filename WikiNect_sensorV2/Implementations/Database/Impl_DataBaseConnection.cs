using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataConnection;

namespace DataConnection
{
    class DataBaseConnection : DataConnection.Connection
    {

        private String databasePath = "";
        private String database = "";
        private String user = "";
        private String password = "";

        public DataBaseConnection()
        {

        }

        public DataBaseConnection(String databasePath, String database, String user, String password)
        {

            this.databasePath = databasePath;
            this.database = database;
            this.user = user;
            this.password = password;

        }




        public List<DataStore.Category> getSuperCategories(DataStore.Category pCategory)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Category> getSubCategories(DataStore.Category pCategory)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Category> getCategories()
        {
            throw new NotImplementedException();
        }

        public DataStore.Category getCategory(long lCategory)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.DataElement> getElements(long lCategory)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.DataElement> getElements(List<long> pListCategory)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.DataElement> getElements(DataStore.Category pCategory)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.DataElement> getElements(List<DataStore.Category> pListCategory)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Rating> getRatings()
        {
            throw new NotImplementedException();
        }

        public DataStore.Rating getRating(long id)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Rating> getRatings(DataStore.DataElement pElement)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Rating> getRatings(DataStore.Category pCategory)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.User> getUsers()
        {
            throw new NotImplementedException();
        }

        public DataStore.User getUser(long id)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getAnnotations()
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getAnnotations(DataStore.User pUser)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getAnnotations(DataStore.User pUser, DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getAnnotations(DataStore.User pUser, DataStore.AnnotationType pType, DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getAnnotations(DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getAnnotations(DataStore.DataElement element, DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getAnnotations(DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getLinks()
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getLinks(DataStore.User pUser)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getLinks(DataStore.User pUser, DataStore.AnnotationType pType, DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getLinks(DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getLinks(DataStore.DataElement element, DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getLinks(DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Attribut> getAttributs()
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Attribut> getAttributs(DataStore.User pUser)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Attribut> getAttributs(DataStore.User pUser, DataStore.AnnotationType pType, DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Attribut> getAttributs(DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Attribut> getAttributs(DataStore.DataElement element, DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }





        public DataStore.DataElement addNewElement(DataStore.DataElement pElement)
        {
            throw new NotImplementedException();
        }

        public bool removeDataElement(DataStore.DataElement pElement)
        {
            throw new NotImplementedException();
        }

        public bool removeAttribut(DataStore.Attribut pAttribut)
        {
            throw new NotImplementedException();
        }

        public bool removeLink(DataStore.Link pLink)
        {
            throw new NotImplementedException();
        }

        public bool removeAnnotation(DataStore.Annotation pAnnotation)
        {
            throw new NotImplementedException();
        }

        public bool removeRating(DataStore.Rating pRating)
        {
            throw new NotImplementedException();
        }

        public DataStore.User addUser(DataStore.User pUser)
        {
            throw new NotImplementedException();
        }

        public DataStore.Rating addRating(DataStore.Rating pRating)
        {
            throw new NotImplementedException();
        }

        public DataStore.Annotation addAnnotation(DataStore.Annotation pAnnotation)
        {
            throw new NotImplementedException();
        }

        public DataStore.Link addLink(DataStore.Link pLink)
        {
            throw new NotImplementedException();
        }


        public List<DataStore.Category> getSuperCategories(string nameCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Category> Connection.getSuperCategories(DataStore.Category pCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Category> Connection.getSuperCategories(string nameCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Category> Connection.getSubCategories(DataStore.Category pCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Category> Connection.getCategories()
        {
            throw new NotImplementedException();
        }

        DataStore.Category Connection.getCategory(long lCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.DataElement> Connection.getElements(long lCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.DataElement> Connection.getElements(List<long> pListCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.DataElement> Connection.getElements(DataStore.Category pCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.DataElement> Connection.getElements(List<DataStore.Category> pListCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Rating> Connection.getRatings()
        {
            throw new NotImplementedException();
        }

        DataStore.Rating Connection.getRating(long id)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Rating> Connection.getRatings(DataStore.DataElement pElement)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Rating> Connection.getRatings(DataStore.Category pCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.User> Connection.getUsers()
        {
            throw new NotImplementedException();
        }

        DataStore.User Connection.getUser(long id)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getAnnotations()
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getAnnotations(DataStore.User pUser)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getAnnotations(DataStore.User pUser, DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getAnnotations(DataStore.User pUser, DataStore.AnnotationType pType, DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getAnnotations(DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getAnnotations(DataStore.DataElement element, DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getAnnotations(DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getLinks()
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getLinks(DataStore.User pUser)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getLinks(DataStore.User pUser, DataStore.AnnotationType pType, DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getLinks(DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getLinks(DataStore.DataElement element, DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getLinks(DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Attribut> Connection.getAttributs()
        {
            throw new NotImplementedException();
        }

        List<DataStore.Attribut> Connection.getAttributs(DataStore.User pUser)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Attribut> Connection.getAttributs(DataStore.User pUser, DataStore.AnnotationType pType, DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Attribut> Connection.getAttributs(DataStore.DataElement element)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Attribut> Connection.getAttributs(DataStore.DataElement element, DataStore.AnnotationType pType)
        {
            throw new NotImplementedException();
        }

        DataStore.DataElement Connection.addNewElement(DataStore.DataElement pElement)
        {
            throw new NotImplementedException();
        }

        bool Connection.removeDataElement(DataStore.DataElement pElement)
        {
            throw new NotImplementedException();
        }

        bool Connection.removeAttribut(DataStore.Attribut pAttribut)
        {
            throw new NotImplementedException();
        }

        bool Connection.removeLink(DataStore.Link pLink)
        {
            throw new NotImplementedException();
        }

        bool Connection.removeAnnotation(DataStore.Annotation pAnnotation)
        {
            throw new NotImplementedException();
        }

        bool Connection.removeRating(DataStore.Rating pRating)
        {
            throw new NotImplementedException();
        }

        DataStore.User Connection.addUser(DataStore.User pUser)
        {
            throw new NotImplementedException();
        }

        DataStore.Rating Connection.addRating(DataStore.Rating pRating)
        {
            throw new NotImplementedException();
        }

        DataStore.Annotation Connection.addAnnotation(DataStore.Annotation pAnnotation)
        {
            throw new NotImplementedException();
        }

        DataStore.Link Connection.addLink(DataStore.Link pLink)
        {
            throw new NotImplementedException();
        }


        List<DataStore.Category> Connection.getCategories(DataStore.DataElement pElement)
        {
            throw new NotImplementedException();
        }


        List<DataStore.Category> Connection.getCategories(string nameElement)
        {
            throw new NotImplementedException();
        }

        public string login()
        {
            throw new NotImplementedException();
        }

        public string login(string user, string password)
        {
            throw new NotImplementedException();
        }

        public void logout()
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getAnnotations(bool bAll)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getAnnotations(DataStore.User pUser, bool bAll)
        {
            throw new NotImplementedException();
        }

        public List<DataStore.Annotation> getAnnotations(DataStore.DataElement element, bool bAll)
        {
            throw new NotImplementedException();
        }

        bool Connection.login()
        {
            throw new NotImplementedException();
        }

        bool Connection.login(string user, string password)
        {
            throw new NotImplementedException();
        }


        void Connection.logout()
        {
            throw new NotImplementedException();
        }

        List<DataStore.Image> Connection.getImages(DataStore.Category pCategory)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getAnnotations(bool bAll)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getAnnotations(DataStore.User pUser, bool bAll)
        {
            throw new NotImplementedException();
        }

        List<DataStore.Annotation> Connection.getAnnotations(DataStore.DataElement element, bool bAll)
        {
            throw new NotImplementedException();
        }



        public DataStore.Image getRandomImage(DataStore.Category pCategory)
        {
            throw new NotImplementedException();
        }
    }
}
