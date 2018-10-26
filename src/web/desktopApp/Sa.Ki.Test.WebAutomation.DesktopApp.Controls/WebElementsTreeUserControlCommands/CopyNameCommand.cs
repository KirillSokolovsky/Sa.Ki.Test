namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class CopyNameCommand : WebElementCommand
    {
        public CopyNameCommand(WebElementsTreeUserControl webElementsTreeUserControl)
            : base(webElementsTreeUserControl)
        {
        }

        protected override void ExecuteCommand(WebElementInfoViewModel elementModel)
        {
            Clipboard.SetText(_webElementsTreeUserControl.SelectedWebElement?.Name);
        }
    }
}
