namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateWebElementCommand : WebElementCommand
    {
        private string _elementType;

        public CreateWebElementCommand(string elementType, WebElementsTreeUserControl webElementsTreeUserControl)
            : base(webElementsTreeUserControl)
        {
            _elementType = elementType;
        }

        protected override void ExecuteCommand(WebElementInfoViewModel elementModel)
        {
            
        }
    }
}
