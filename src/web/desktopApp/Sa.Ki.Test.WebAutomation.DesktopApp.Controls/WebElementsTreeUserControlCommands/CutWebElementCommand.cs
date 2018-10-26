namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CutWebElementCommand : WebElementCommand
    {
        public CutWebElementCommand(WebElementsTreeUserControl webElementsTreeUserControl)
            : base(webElementsTreeUserControl)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return _webElementsTreeUserControl.CutWebElement != null;
        }

        protected override void ExecuteCommand(WebElementInfoViewModel elementInfo)
        {
            _webElementsTreeUserControl.CutWebElement = _webElementsTreeUserControl.SelectedWebElement;
            _webElementsTreeUserControl.CopiedWebElement = null;
        }
    }
}
