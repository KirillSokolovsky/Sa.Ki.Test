namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.DesktopApp.Dialogs;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class CloneWebElementCommand : WebElementCommand
    {
        public CloneWebElementCommand(WebElementsTreeUserControl webElementsTreeUserControl)
            : base(webElementsTreeUserControl)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return WebElementCommandsHelper.CanBeCloned(_webElementsTreeUserControl.SelectedWebElement);
        }

        protected override void ExecuteCommand()
        {
            if (!CanExecute(null)) return;

            var existedNames = WebElementCommandsHelper.GetExistedNamesForChildWebElements(
                Selected,
                _webElementsTreeUserControl,
                null);

            var textDialog = new TextDialog("Clonned WebElement name");
            textDialog.Validate = str => !existedNames.Contains(str);

            if (textDialog.ShowDialog() != true) return;

            var info = WebElementsViewModelsFactory.CreateInfoFromModel(Selected);
            var model = WebElementsViewModelsFactory.CreateModelFromInfo(info);
            model.Name = textDialog.Text;

            if (Selected.Parent == null)
            {
                if (model is WebContextInfoViewModel wc)
                    _webElementsTreeUserControl.WebContexts.Add(wc);
                else MessageBox.Show("WebElement to clone is not a WebContext. Magic Error.");
            }
            else
            {
                model.Parent = Selected.Parent;
                Selected.Parent.Elements.Add(model);
            }
        }
    }
}
