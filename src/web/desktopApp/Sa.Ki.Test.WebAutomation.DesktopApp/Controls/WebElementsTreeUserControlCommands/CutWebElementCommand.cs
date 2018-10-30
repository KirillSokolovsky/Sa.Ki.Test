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
            return WebElementCommandsHelper.CanBeCut(Selected);
        }

        protected override void ExecuteCommand()
        {
            _webElementsTreeUserControl.CutWebElement = Selected;
            _webElementsTreeUserControl.CopiedWebElement = null;
        }
    }
}
