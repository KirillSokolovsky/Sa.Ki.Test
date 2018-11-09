namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Dialogs;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class CreateWebElementCommand : WebElementCommand
    {
        private string _elementType;

        public CreateWebElementCommand(string elementType, WebElementsTreeUserControl webElementsTreeUserControl)
            : base(webElementsTreeUserControl)
        {
            _elementType = elementType;
        }

        protected override void ExecuteCommand()
        {
            var validator = WebElementCommandsHelper.GetCreateUpdateWebElementValidator(_webElementsTreeUserControl, null);

            if (_elementType == WebElementTypes.Reference)
            {
                CreateWebElemetnRefence(validator);
                return;
            }

            var dialog = new WebElementCreateEditDialog(validator, _elementType,
                _elementType);
            if (dialog.ShowDialog() != true) return;

            var element = WebElementsViewModelsFactory.CreateModelFromWebElementType(_elementType);
            WebElementsViewModelsFactory.FillModelWithBaseInfo(
                element,
                dialog.WebElement);

            if(_elementType == WebElementTypes.RadioGroup || _elementType == WebElementTypes.DropDown)
            {
                var combined = element as CombinedWebElementInfoViewModel;
                var names = new List<string>();

                if (_elementType == WebElementTypes.DropDown)
                {
                    dialog = new WebElementCreateEditDialog(null, WebElementTypes.Element,
                        $"{element.Name} Input",
                        $"Input for {element.Name}",
                        "Input", true);
                    if (dialog.ShowDialog() != true) return;
                    var input = dialog.WebElement;
                    combined.Elements.Add(input);
                    input.Parent = combined;
                    names.Add(input.Name);
                }

                validator = (el) => names.Contains(el.Name)
                    ? $"WebElement with name {el.Name} already exists on the level"
                    : null;

                dialog = new WebElementCreateEditDialog(validator, WebElementTypes.Element,
                    $"{element.Name} Option",
                    $"One Option in {element.Name}",
                    "Option", true);

                if (dialog.ShowDialog() != true) return;
                var option = dialog.WebElement;
                combined.Elements.Add(option);
                option.Parent = combined;
            }

            if (Selected == null)
            {
                if (_elementType != WebElementTypes.Context)
                    MessageBox.Show("Magic error. Context WebElement was created not from tree root");

                if (_webElementsTreeUserControl.WebContexts == null)
                    _webElementsTreeUserControl.WebContexts = new ObservableCollection<WebContextInfoViewModel>();
                _webElementsTreeUserControl.WebContexts.Add(element as WebContextInfoViewModel);
            }
            else
            {
                var comb = Selected as CombinedWebElementInfoViewModel;
                if(comb == null)
                    MessageBox.Show("Magic error. WebElement was created not from CombinedWebElement");

                if (comb.Elements == null)
                    comb.Elements = new ObservableCollection<WebElementInfoViewModel>();

                comb.Elements.Add(element);
                element.Parent = comb;
            }
        }

        private void CreateWebElemetnRefence(Func<WebElementInfoViewModel, string> validator)
        {
            var dialog = new WebElementCreateEditDialog(validator, _elementType,
                _elementType);
            if (dialog.ShowDialog() != true) return;
        }
    }
}
