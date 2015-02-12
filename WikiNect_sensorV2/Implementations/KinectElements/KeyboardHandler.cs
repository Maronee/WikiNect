using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Windows;
using WikiNectLayout.Implementions.Xamls;
using WikiNectLayout.Interfaces.Kinect;

namespace WikiNectLayout.Implementions.KinectElements
{
    public class KeyboardHandler
    {
        private List<WikiNectKeyboard> myKeyboards = new List<WikiNectKeyboard>();
        private int activeKeyboard = 0;
        private WikiNectKeyboardCaller callingClass;

        public KeyboardHandler()
        {
            //TODO: Read all xml-Files
            var keyboardXml = XDocument.Load(Directory.GetCurrentDirectory() + @"\keyboards\myKeyboard.xml");

            IEnumerable<XElement> design =
                from el in keyboardXml.Root.Descendants("design")
                select el;
            if (design.ElementAt(0).Value == "classic")
            {
                myKeyboards.Add(new WikiNectClassicKeyboard(this, keyboardXml));
            }
            else
            {
                MessageBox.Show("There are Keyboard specifications that can't be read");
            }
        }

        private WikiNectKeyboard getActiveKeyboard()
        {
            return myKeyboards[activeKeyboard];
        }

        public void showKeyboard(String existingText, WikiNectKeyboardCaller cC)
        {
            this.callingClass = cC;
            this.getActiveKeyboard().setText(existingText);
            this.getActiveKeyboard().showKeyboard();
        }

        public void closeWithNewText(String text)
        {
            callingClass.closeWithNewText(text);
        }

        public void closeWithOutNewText()
        {
            callingClass.closeWithoutNewText();
        }
    }
}
