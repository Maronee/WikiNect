using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using Microsoft.Kinect;
using System.Drawing;
using System.IO;
using System.Diagnostics;

using Kinect;
using DataConnection;
using DataStore;
using WikiNectLayout.Implementions.Xamls;
using System.Windows.Controls.Primitives;
using WikiNectLayout.Interfaces.Kinect;
using WikiNectLayout.Implementions.KinectElements;


namespace WikiNectLayout.Implementions.Xamls
{
    /// <summary>
    /// Interaktionslogik für WikiNectKeyboard.xaml
    /// </summary>
    public partial class WikiNectClassicKeyboard : Window, WikiNectKeyboard
    {
        private List<List<KeyboardButton>> buttonList = new List<List<KeyboardButton>>();
        private List<Grid> theLevels = new List<Grid>();
        private List<bool> alternateEnable = new List<bool>();
        private bool alternatemode = false;
        private int activelevel = 0;
        private KeyboardHandler keyboardHandler;

        public WikiNectClassicKeyboard(KeyboardHandler keybh, XDocument keyboardXml)
        {
            InitializeComponent();
            this.keyboardHandler = keybh;
            this.Topmost = true;
            int j = 0;
            Grid theOuterGrid = new Grid();
            IEnumerable<XElement> levels =
                    from el in keyboardXml.Root.Descendants("level")
                    select el;
            foreach (XElement level in levels)
            {
                if (level.Element("alternate_button_UTF8").Attribute("active").Value == "false") { alternateEnable.Add(false); }
                else { alternateEnable.Add(true); }
                Grid theGrid = new Grid();
                theLevels.Add(theGrid);
                if (j != 0) { theGrid.Visibility = System.Windows.Visibility.Collapsed; } 
                buttonList.Add(new List<KeyboardButton>());
                int k = 0;
                IEnumerable<XElement> rows =
                    from el in level.Descendants("row")
                    select el;
                foreach (XElement row in rows)
                {
                    theGrid.RowDefinitions.Add(new RowDefinition());
                    
                    StackPanel stp = new StackPanel();
                    stp.Orientation = Orientation.Horizontal;
                    stp.HorizontalAlignment = HorizontalAlignment.Center;
                    stp.VerticalAlignment = VerticalAlignment.Bottom;
                    stp.Margin = new Thickness(5, 5, 0, 0);
                    foreach (XElement button in row.Descendants("button"))
                    {
                        KeyboardButton kbb;
                        if (button.Element("button_content_alternate") != null) { kbb = new KeyboardButton(50, 50, button.Element("button_content").Value, button.Element("button_content_alternate").Value, 3, "btn_Click"); }
                        else { kbb = new KeyboardButton(50, 50, button.Element("button_content").Value, 3, "btn_Click"); }
                        kbb.Click += new RoutedEventHandler(btn_Click);
                        buttonList[j].Add(kbb);
                        stp.Children.Add(kbb);
                    }
                    theGrid.Children.Add(stp);
                    Grid.SetRow(stp, k);
                    k++;
                }
                theOuterGrid.Children.Add(theGrid);
                Grid.SetRow(theGrid, j);
                j++;
            }
            keyboardInnerGrid.Children.Add(theOuterGrid);
            Grid.SetRow(theOuterGrid, 2);
            shiftBtn.IsEnabled = alternateEnable[activelevel];
        }

        public void windowLoaded(object sender, RoutedEventArgs e)
        {

        }

        public void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        public void setText(String text)
        {
            textInput.Text = text;
        }

        public void showKeyboard()
        {
            this.ShowDialog();
        }

        /// <summary>
        /// This method is activated on a CategoryButton click. It gets the Picture of the selected Categorie and put them into 
        /// the categoriegroup StackPanel.
        /// </summary>
        /// <param name="sender">CategoryButton</param>
        /// <param name="e"></param>
        void btn_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(acceptBtn)) {
                //Text zurueckuebergeben
                keyboardHandler.closeWithNewText(textInput.Text);
                //clear Text
                textInput.Text = "";
                //close KeyboardWindow
                this.Hide();
            }
            else if (sender.Equals(cancelBtn))
            {
                //Information ueber Abbruch zurueckgeben
                keyboardHandler.closeWithOutNewText();
                //clear Text
                textInput.Text = "";
                //close KeyboardWindow
                this.Hide();
            }
            else if (sender.Equals(shiftBtn))
            {
                //every button needs to show its alternate content
                if (alternatemode) { foreach (KeyboardButton kbb in buttonList[activelevel]) { kbb.restoreContent();} }
                else { foreach (KeyboardButton kbb in buttonList[activelevel]) { kbb.changeToAlternateContent(); } }
                alternatemode = !alternatemode;
            }
            else if (sender.Equals(levelBtn))
            {
                //we need to load another "level" of the keyboard like numbers or special characters
                theLevels[activelevel].Visibility = System.Windows.Visibility.Collapsed;
                activelevel = (activelevel + 1) % theLevels.Count;
                theLevels[activelevel].Visibility = System.Windows.Visibility.Visible;
                shiftBtn.IsEnabled = alternateEnable[activelevel];
            }
            else if (sender.Equals(spaceBtn))
            {
                //clear Text
                textInput.AppendText(" ");
            }
            else
            {
                foreach (KeyboardButton kbb in buttonList[activelevel])
                {
                    if (sender.Equals(kbb)) { textInput.AppendText(kbb.Content.ToString()); }             
                }
            };

        }


        private void OnLoadedStoryboardCompleted(object sender, RoutedEventArgs e)
        { 
        }
    }
}
