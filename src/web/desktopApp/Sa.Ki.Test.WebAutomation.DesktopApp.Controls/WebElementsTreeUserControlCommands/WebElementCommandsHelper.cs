namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class WebElementCommandsHelper
    {
        public static Func<WebElementInfoViewModel, bool> CanElementHasCustomChildren = elementInfo => 
            elementInfo == null //It's a tree view root, so we could add at least Context
            || (elementInfo is CombinedWebElementInfoViewModel
                && !(elementInfo.ElementType == WebElementTypes.DropDown || elementInfo.ElementType == WebElementTypes.RadioGroup));
    }
}
