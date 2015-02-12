using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WikiNectLayout.Interfaces.Kinect
{
    public interface WikiNectKeyboardCaller
    {
        /*
         * Der Aufruf des Keyboards erfolgt mittels des KeyboardHandlers der initalisiert werden muss
         * KeyboardHandler keybh = new KeyboardHandler();
         * Der Aufruf erfolgt mit folgender Methode: keybH.showKeyboard("String", this);
         * Als String kann dabei ein vorbelegender Text für das Keyboard mitgegeben werden, die Klasse selbst muss für den Rückaufruf übergeben werden.
         * 
         * */

        /// <summary>
        /// Methode die aufgerufen wird, sobald der User einen neuen Text eingegeben hat und diesen akzeptiert und damit das Keyboard schließt
        /// </summary>
        void closeWithNewText(String newText);

        /// <summary>
        /// Methode die aufgerufen wird, sobald der User das Keyboard schließt ohne einen neuen Text übernehmen zu wollen
        /// </summary>
        void closeWithoutNewText();
    }
}
