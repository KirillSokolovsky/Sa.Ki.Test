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
            return Selected != null;
        }

        protected override void ExecuteCommand()
        {
            _webElementsTreeUserControl.CopiedWebElement = Selected;
            _webElementsTreeUserControl.CutWebElement = null;
        }
    }
}
