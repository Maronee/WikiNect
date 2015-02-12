using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataConnection;
using DataStore;

namespace Wikinect
{
    class WikiNect
    {

        private DataConnection.Connection connection = null;

        private List<Category> categories = new List<Category>();

        public WikiNect() 
        {
            //this.connection = new DataBaseConnection();
            this.init();

        }

        private void init()
        {
            // Init Connection --> JSONConnection
            this.connection = new JSONConnection("http://mw1.wikinect.hucompute.org/");
            //this.connection = new JSONConnection("http://de.wikipedia.org/w/");

            //this.connection = new DataBase

            this.categories = this.connection.getCategories();

        }

        public override String ToString()
        {
            String s = "";

                s += "Categories\n";
                //foreach (Category c in this.categories)
                for (int i = 1; i <= 1; i++)
                {
                    Category c = this.categories[i];
                    s += c.ToString() + "\n";
                    s += "Elements:\n";
                    foreach (DataElement e in this.connection.getElements(c))
                    {
                        s += e.ToString() + "\n";
                    }
                    s += "----------------------\n";
                    s += "Categories:\n";
                    foreach (Category c2 in this.connection.getSubCategories(c))
                    {
                        s += c2.ToString() + "\n";
                    }
                    s += "----------------------\n";
                    
                }
                s += "========================\n";

                foreach (Attribut a in this.connection.getAttributs(new CategoryImpl(this.connection, new Uri("http://nouri/"), "Das Abendmahl", "")))
                {
                    s += a.ToString() + "\n";
                }
                s += "----------------------\n";
                //s += string.Join("\n", this.connection.getSuperCategories("Psychologe"));
                //foreach (Category c3 in this.connection.getCategories("Psychologe"))
                //{
                    //s += c3.ToString() + "\n";
                //}
                //s += "\n++++++++++++++++\n";

                //s += (this.connection.getCategory((long)3797903)).ToString() + "\n";
                

            return s;

        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Form f = new Form();

            WikiNect wikinect = new WikiNect();

            Console.WriteLine(wikinect);

            wikinect.connection.getUsers();

            Application.Run(f);

        }
    }
}
