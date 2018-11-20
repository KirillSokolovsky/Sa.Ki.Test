namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Dialogs;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using Sa.Ki.Test.SakiTree;
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
            string referenceTreePath = null;
            if (_elementType == WebElementTypes.Reference)
            {
                var blockedTypesToPick = WebElementsViewModelsHelper.GetBlockedElementTypesForElementType(_elementType);

                var picker = new WebElementPickerDialog(_webElementsTreeUserControl.WebElements.ToList(),
                    null,
                    null,
                    blockedTypesToPick);

                if (picker.ShowDialog() != true) return;
                referenceTreePath = picker.SelectedWebElementTreePath;
            }

            var validator = WebElementCommandsHelper.GetCreateUpdateWebElementValidator(_webElementsTreeUserControl, null,
                _elementType != WebElementTypes.Directory);

            //TODO: add ctor override to accept WebElementInfoViewModel with default data
            var dialog = new WebElementCreateEditDialog(validator, _elementType,
                _elementType);
            if (dialog.ShowDialog() != true) return;

            var createdWebElement = WebElementsViewModelsHelper.CreateModelFromWebElementType(_elementType);
            WebElementsViewModelsHelper.FillModelWithBaseInfo(
                createdWebElement,
                dialog.WebElement);

            if (_elementType == WebElementTypes.RadioGroup || _elementType == WebElementTypes.DropDown)
            {
                var combined = createdWebElement as CombinedWebElementInfoViewModel;
                var names = new List<string>();

                if (_elementType == WebElementTypes.DropDown)
                {
                    dialog = new WebElementCreateEditDialog(null, WebElementTypes.Element,
                        $"{createdWebElement.Name} Input",
                        $"Input for {createdWebElement.Name}",
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
                    $"{createdWebElement.Name} Option",
                    $"One Option in {createdWebElement.Name}",
                    "Option", true);

                if (dialog.ShowDialog() != true) return;
                var option = dialog.WebElement;
                combined.Elements.Add(option);
                option.Parent = combined;
            }
            else if (_elementType == WebElementTypes.Reference || _elementType == WebElementTypes.Frame)
            {
                if (_elementType == WebElementTypes.Frame)
                {
                    var blockedTypesToPick = WebElementsViewModelsHelper.GetBlockedElementTypesForElementType(_elementType);

                    var picker = new WebElementPickerDialog(_webElementsTreeUserControl.WebElements.ToList(),
                        null,
                        null,
                        blockedTypesToPick);

                    if (picker.ShowDialog() != true) return;
                    referenceTreePath = picker.SelectedWebElementTreePath;
                }

                (createdWebElement as WebElementWithReferenceViewModel).ReferenceBreadString
                    = referenceTreePath;

                var referencedElement = (WebElementInfoViewModel)_webElementsTreeUserControl.WebElements.FindNodeByTreePath(referenceTreePath);

                var referemcedElementInfo = WebElementsViewModelsHelper.CreateInfoFromModel(referencedElement);
                var referencedElementModel = WebElementsViewModelsHelper.CreateModelFromInfo(referemcedElementInfo);
                (createdWebElement as WebElementWithReferenceViewModel).ReferencedWebElement = referencedElementModel;
            }

            if (Selected == null)
            {
                if (_webElementsTreeUserControl.WebElements == null)
                    _webElementsTreeUserControl.WebElements = new ObservableCollection<CombinedWebElementInfoViewModel>();
                _webElementsTreeUserControl.WebElements.Add(createdWebElement as CombinedWebElementInfoViewModel);
            }
            else
            {
                var comb = Selected as CombinedWebElementInfoViewModel;
                if (comb == null)
                {
                    MessageBox.Show("Magic error. WebElement was created not from CombinedWebElement");
                    return;
                }

                if (comb.Elements == null)
                    comb.Elements = new ObservableCollection<WebElementInfoViewModel>();

                comb.Elements.Add(createdWebElement);
                createdWebElement.Parent = comb;
            }
        }

        private void CreateWebElemetnRefence(Func<WebElementInfoViewModel, string> validator)
        {
            var dialog = new WebElementCreateEditDialog(validator, _elementType,
                _elementType);
            if (dialog.ShowDialog() != true) return;

            var currrentElement = _webElementsTreeUserControl.SelectedWebElement;
        }
    }
}
