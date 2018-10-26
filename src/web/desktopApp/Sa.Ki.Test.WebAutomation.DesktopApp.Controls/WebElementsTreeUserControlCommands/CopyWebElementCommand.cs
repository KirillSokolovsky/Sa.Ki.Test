namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CopyWebElementCommand : WebElementCommand
    {
        public CopyWebElementCommand(WebElementsTreeUserControl webElementsTreeUserControl)
            : base(webElementsTreeUserControl)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return _webElementsTreeUserControl.SelectedWebElement != null;
        }

        protected override void ExecuteCommand(WebElementInfoViewModel elementInfo)
        {
            _webElementsTreeUserControl.CopiedWebElement = _webElementsTreeUserControl.SelectedWebElement;
            _webElementsTreeUserControl.CutWebElement = null;
        }
    }
}
