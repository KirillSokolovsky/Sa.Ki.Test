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
            WebElementInfoViewModel createdElement = null;
            if (_elementType == WebElementTypes.Reference)
            {
                createdElement = CreateReference();
                if (createdElement == null) return;
            }
            else
            {
                createdElement = CreateWebElementInfo();

                if (createdElement == null) return;

                if (_elementType == WebElementTypes.DropDown
                    || _elementType == WebElementTypes.RadioGroup)
                {
                    var combinedCreatedElement = createdElement as CombinedWebElementInfoViewModel;

                    if (combinedCreatedElement.Elements == null)
                        combinedCreatedElement.Elements = new ObservableCollection<WebElementInfoViewModel>();

                    if (_elementType == WebElementTypes.DropDown)
                    {
                        var inputTemplate = new WebElementInfoViewModel
                        {
                            Name = $"{createdElement.Name} Input",
                            Description = $"Input for {createdElement.Name}",
                            InnerKey = DropDownElementInfo.Keys.Input,
                            ElementType = WebElementTypes.Element,
                            IsKey = true,
                            Locator = new WebLocatorInfoViewModel
                            {
                                IsRelative = true,
                                LocatorType = WebLocatorType.XPath,
                                LocatorValue = ".//"
                            }
                        };

                        var inputElement = CreateWebElementInfo(
                            combinedCreatedElement,
                            inputTemplate);
                        if (inputElement == null) return;

                        inputElement.Parent = combinedCreatedElement;
                        combinedCreatedElement.Elements.Add(inputElement);
                    }

                    var optionTemplate = new WebElementInfoViewModel
                    {
                        Name = $"{createdElement.Name} Option",
                        Description = $"Option for {createdElement.Name}",
                        InnerKey = _elementType == WebElementTypes.DropDown
                            ? DropDownElementInfo.Keys.Option
                            : RadioGroupElementInfo.Keys.Option,
                        ElementType = WebElementTypes.Element,
                        IsKey = true,
                        Locator = new WebLocatorInfoViewModel
                        {
                            IsRelative = true,
                            LocatorType = WebLocatorType.XPath,
                            LocatorValue = ".//"
                        }
                    };

                    var optionElement = CreateWebElementInfo(
                        combinedCreatedElement,
                        optionTemplate);
                    if (optionElement == null) return;

                    optionElement.Parent = combinedCreatedElement;
                    combinedCreatedElement.Elements.Add(optionElement);
                }

                if (_elementType == WebElementTypes.Frame)
                {
                    var frameElement = createdElement as WebElementWithReferenceViewModel;
                    var referenceTreePath = frameElement.ReferenceBreadString;
                    if (referenceTreePath == null)
                    {
                        var blockedTypesToPick = WebElementsViewModelsHelper.GetBlockedElementTypesForElementType(_elementType);

                        var picker = new WebElementPickerDialog(_webElementsTreeUserControl.WebElements.ToList(),
                            true,
                            null,
                            null,
                            blockedTypesToPick);


                        if (picker.ShowDialog() != true) return;
                        referenceTreePath = picker.SelectedWebElementTreePath;
                    }

                    var referencedElement = (WebElementInfoViewModel)_webElementsTreeUserControl.WebElements.FindNodeByTreePath(referenceTreePath);

                    var copy = WebElementsViewModelsHelper.CreateFullModelCopy(referencedElement);
                    copy.Parent = frameElement;
                    if (frameElement.Elements == null)
                        frameElement.Elements = new ObservableCollection<WebElementInfoViewModel>();
                    frameElement.Elements.Clear();
                    frameElement.Elements.Add(copy);
                }
            }
            if (createdElement == null) return;

            if (Selected == null)
            {
                if (_webElementsTreeUserControl.WebElements == null)
                    _webElementsTreeUserControl.WebElements = new ObservableCollection<CombinedWebElementInfoViewModel>();
                _webElementsTreeUserControl.WebElements.Add(createdElement as CombinedWebElementInfoViewModel);
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

                comb.Elements.Add(createdElement);
                createdElement.Parent = comb;
            }

            return;
        }

        private WebElementInfoViewModel CreateReference()
        {
            var blockedTypesToPick = WebElementsViewModelsHelper.GetBlockedElementTypesForElementType(_elementType);

            var picker = new WebElementPickerDialog(_webElementsTreeUserControl.WebElements.ToList(),
                false,
                null,
                null,
                blockedTypesToPick);

            if (picker.ShowDialog() != true) return null;
            var referenceTreePath = picker.SelectedWebElementTreePath;
            var referencedElement = (WebElementInfoViewModel)_webElementsTreeUserControl.WebElements.FindNodeByTreePath(referenceTreePath);
            var referenceCopy = WebElementsViewModelsHelper.CreateFullModelCopy(referencedElement);

            var templateInfo = WebElementsViewModelsHelper.CreateModelFromWebElementType(WebElementTypes.Reference)
                as WebElementWithReferenceViewModel;
            WebElementsViewModelsHelper.FillModelWithBaseInfo(templateInfo, referencedElement);
            templateInfo.Locator = null;
            templateInfo.ElementType = WebElementTypes.Reference;
            templateInfo.ReferenceBreadString = referenceTreePath;
            templateInfo.ReferencedWebElement = referenceCopy;

            var createdElement = CreateWebElementInfo(null, templateInfo) as WebElementWithReferenceViewModel;

            if (createdElement == null) return null;

            createdElement.ReferencedWebElement = referenceCopy;
            if (createdElement.Elements == null)
                createdElement.Elements = new ObservableCollection<WebElementInfoViewModel>();
            referenceCopy.Parent = createdElement;

            return createdElement;
        }

        private WebElementInfoViewModel CreateWebElementInfo(CombinedWebElementInfoViewModel parent = null, WebElementInfoViewModel template = null)
        {
            var validator = WebElementCommandsHelper.GetCreateUpdateWebElementValidator(_webElementsTreeUserControl, null);

            //TODO: add ctor override to accept WebElementInfoViewModel with default data
            var dialog = new WebElementCreateEditDialog(validator,
                parent ?? Selected as CombinedWebElementInfoViewModel,
                _elementType,
                template);
            dialog.WebElements = _webElementsTreeUserControl.WebElements;
            if (dialog.ShowDialog() != true) return null;

            var createdWebElement = WebElementsViewModelsHelper.CreateModelFromWebElementType(_elementType);
            WebElementsViewModelsHelper.FillModelWithBaseInfo(
                createdWebElement,
                dialog.WebElement);

            return createdWebElement;
        }
    }
}
