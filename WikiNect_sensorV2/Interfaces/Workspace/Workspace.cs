using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wikinect
{
    interface Workspace
    {
        String getName();
        
        DataStore.DataElement getSelectedElement();
        List<DataStore.DataElement> searchForElement(String query);
        void removeSelectedElement(DataStore.DataElement targetElement);
        void removeAllElementsFromWorkspace();
        
    }
}
