namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Dialogs;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EditWebElementCommand : WebElementCommand
    {
        public EditWebElementCommand(WebElementsTreeUserControl webElementsTreeUserControl) 
            : base(webElementsTreeUserControl)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return _webElementsTreeUserControl.SelectedWebElement != null;
        }

        protected override void ExecuteCommand()
        {
            var validator = WebElementCommandsHelper.GetCreateUpdateWebElementValidator(
                _webElementsTreeUserControl,
                Selected.Name);
            
            var editDialog = new WebElementCreateEditDialog(validator, Selected);
            editDialog.WebElements = _webElementsTreeUserControl.WebElements;
            if (editDialog.ShowDialog() != true) return;
        }
    }
}
