using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WikiNectLayout.Interfaces.Kinect
{
    // Keyboards should be implemented as : Window, WikiNectKeyboard
    public interface WikiNectKeyboard
    {
        /// <summary>
        /// Method that provides text that is already in the textBox of the keyboard when the user opens it
        /// </summary>
        /// <param name="text"></param>
        void setText(String text);
        // { this.textBox = text; }

        /// <summary>
        /// Method that provides the opening of the Keyboard when chosen by the KeyboardHandler
        /// </summary>
        void showKeyboard();
        // { this.ShowDialog(); }
    }
}
