namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.DesktopApp.Models.SaKiMenu;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class WebElementMenuItemsFactory
    {
        private WebElementsTreeUserControl _webElementsTreeUserControl;
        private Dictionary<string, SaKiMenuItemViewModel> _cache = new Dictionary<string, SaKiMenuItemViewModel>();
        private Dictionary<string, List<SaKiMenuItemViewModel>> _menusCache = new Dictionary<string, List<SaKiMenuItemViewModel>>();

        public WebElementMenuItemsFactory(WebElementsTreeUserControl webElementsTreeUserControl)
        {
            _webElementsTreeUserControl = webElementsTreeUserControl;
        }

        public List<SaKiMenuItemViewModel> CreateMenuItemsForWebElement()
        {
            var elementInfo = _webElementsTreeUserControl.SelectedWebElement;

            var et = elementInfo?.ElementType ?? "Root";
            var p = elementInfo?.Parent?.ElementType ?? "Root";
            var key = $"Menu Items for {et} {p}";

            if (!_menusCache.ContainsKey(key))
            {
                var items = new List<SaKiMenuItemViewModel>();

                if (elementInfo != null)
                {
                    items.Add(CreateCopyNameMenuItem());
                    items.Add(CreateEditMenuItem());
                }

                if (WebElementCommandsHelper.CanElementHasCustomChildren(elementInfo))
                    items.Add(CreateCreateMenuItemGroup(et));

                if (elementInfo != null)
                    items.Add(CreateActionsMenuItemGroup(et, p));

                _menusCache[key] = items;
            }
            return _menusCache[key];
        }

        public SaKiMenuItemViewModel CreateCopyNameMenuItem()
        {
            var key = "Copy Name";
            if (!_cache.ContainsKey(key))
            {
                _cache[key] = new SaKiCommandMenuItemViewModel
                {
                    Name = "Copy Name",
                    Description = "Copy name of selected WebElementInfo",
                    Command = new CopyNameCommand(_webElementsTreeUserControl)
                };
            }
            return _cache[key];
        }

        public SaKiMenuItemViewModel CreateEditMenuItem()
        {
            var key = "Edit";
            if (!_cache.ContainsKey(key))
            {
                _cache[key] = new SaKiCommandMenuItemViewModel
                {
                    Name = "Edit",
                    Description = "Edit selected WebElementInfo",
                    Command = new EditWebElementCommand(_webElementsTreeUserControl)
                };
            }
            return _cache[key];
        }

        public SaKiMenuItemViewModel CreateCreateMenuItemGroup(string elementType)
        {
            var key = $"Create New group for {elementType}";
            if (!_cache.ContainsKey(key))
            {
                var group = new SaKiGroupMenuItemViewModel
                {
                    Name = "Create New",
                    Description = "Create new child WebElementInfo",
                    Items = new ObservableCollection<SaKiMenuItemViewModel>()
                };

                switch (elementType)
                {
                    case "Root":
                        group.Items.Add(CreateCreateCommandMenuItem(WebElementTypes.Context));
                        break;
                    case WebElementTypes.Context:
                    case WebElementTypes.Control:
                        group.Items.Add(CreateCreateCommandMenuItem(WebElementTypes.Element));
                        group.Items.Add(CreateCreateCommandMenuItem(WebElementTypes.Control));
                        group.Items.Add(CreateCreateCommandMenuItem(WebElementTypes.DropDown));
                        group.Items.Add(CreateCreateCommandMenuItem(WebElementTypes.RadioGroup));
                        group.Items.Add(CreateCreateCommandMenuItem(WebElementTypes.Reference));
                        break;
                    default:
                        MessageBox.Show($"Unknown element type: {elementType} to create Create New menu items group");
                        break;
                }

                _cache[key] = group;
            }
            return _cache[key];
        }

        public SaKiMenuItemViewModel CreateCreateCommandMenuItem(string elementType)
        {
            var key = $"Create New for {elementType}";
            if (!_cache.ContainsKey(key))
            {
                var description = "";
                switch (elementType)
                {
                    case WebElementTypes.Context:
                        description = $"Context WebElement is used to describe separate (relative to site) set of elements with own functionality." +
                            $"{Environment.NewLine}E.g. Page, Header, Popup, Dialog etc.";
                        break;
                    case WebElementTypes.Control:
                        description = $"Control WebElement is used to describe separate (relative to Context) set of elements with own functionality." +
                            $"{Environment.NewLine}E.g. Form, Form control groups, Sets of functional elements, Div, etc.";
                        break;
                    case WebElementTypes.Element:
                        description = $"Element WebElement is used to describe lowest piec of web elements." +
                            $"{Environment.NewLine}E.g. Input, Button, a etc.";
                        break;
                    case WebElementTypes.DropDown:
                        description = $"DropDown WebElement is used to describe expandable lists" +
                            $"{Environment.NewLine}E.g. DropDown, ComboBox, Select with options etc.";
                        break;
                    case WebElementTypes.RadioGroup:
                        description = $"RadioGroup WebElement is used to describe set of radio inputs, where just one valued could be selected";
                        break;
                    case WebElementTypes.Reference:
                        description = $"Reference WebElement is used to referenc to any existed web element";
                        break;
                    default:
                        MessageBox.Show($"Unknown element type: {elementType} to provide description");
                        break;
                }

                _cache[key] = new SaKiCommandMenuItemViewModel
                {
                    Name = elementType,
                    Description = description,
                    Command = new CreateWebElementCommand(elementType, _webElementsTreeUserControl)
                };
            }
            return _cache[key];
        }

        public SaKiMenuItemViewModel CreateActionsMenuItemGroup(string elementType, string parentType)
        {
            var key = $"Create actions for {elementType}";
            if (elementType == WebElementTypes.Element)
                key += $" {parentType}";

            if (!_cache.ContainsKey(key))
            {
                var group = new SaKiGroupMenuItemViewModel
                {
                    Name = "Actions",
                    Description = "Actions to manage WebElements",
                    Items = new ObservableCollection<SaKiMenuItemViewModel>()
                };

                switch (elementType)
                {
                    case "Root":
                        break;

                    case WebElementTypes.Context:
                        group.Items.Add(CreateActionsCommandMenuItem("Delete"));
                        group.Items.Add(CreateActionsCommandMenuItem("Paste"));
                        group.Items.Add(CreateActionsCommandMenuItem("Clone"));
                        break;

                    case WebElementTypes.Control:
                    case WebElementTypes.DropDown:
                    case WebElementTypes.RadioGroup:
                        group.Items.Add(CreateActionsCommandMenuItem("Delete"));
                        group.Items.Add(CreateActionsCommandMenuItem("Copy"));
                        group.Items.Add(CreateActionsCommandMenuItem("Cut"));
                        group.Items.Add(CreateActionsCommandMenuItem("Paste"));
                        group.Items.Add(CreateActionsCommandMenuItem("Clone"));
                        break;

                    case WebElementTypes.Element:
                        {
                            switch (parentType)
                            {
                                case WebElementTypes.DropDown:
                                case WebElementTypes.RadioGroup:
                                    group.Items.Add(CreateActionsCommandMenuItem("Copy"));
                                    break;
                                case WebElementTypes.Context:
                                case WebElementTypes.Control:
                                    group.Items.Add(CreateActionsCommandMenuItem("Delete"));
                                    group.Items.Add(CreateActionsCommandMenuItem("Copy"));
                                    group.Items.Add(CreateActionsCommandMenuItem("Cut"));
                                    group.Items.Add(CreateActionsCommandMenuItem("Paste"));
                                    group.Items.Add(CreateActionsCommandMenuItem("Clone"));
                                    break;
                                default:
                                    MessageBox.Show($"Unexpected parent element type: {parentType} for creating actions menu items");
                                    break;
                            }
                        }
                        break;
                    default:
                        MessageBox.Show($"Unexpected element type: {elementType} for creating actions menu items");
                        break;
                }

                _cache[key] = group;
            }
            return _cache[key];
        }

        public SaKiMenuItemViewModel CreateActionsCommandMenuItem(string actionName)
        {
            var key = $"Action {actionName}";
            if (!_cache.ContainsKey(key))
            {
                var command = new SaKiCommandMenuItemViewModel { Name = actionName };

                switch (actionName)
                {
                    case "Delete":
                        command.Description = "Delete selected WebElement";
                        command.Command = new DeleteWebElementCommand(_webElementsTreeUserControl);
                        break;

                    case "Copy":
                        command.Description = "Copy selected WebElement";
                        command.Command = new CopyWebElementCommand(_webElementsTreeUserControl);
                        break;

                    case "Cut":
                        command.Description = "Cut selected WebElement";
                        command.Command = new CutWebElementCommand(_webElementsTreeUserControl);
                        break;

                    case "Paste":
                        command.Description = "Paste copied/cut WebElement";
                        command.Command = new PasteWebElementCommand(_webElementsTreeUserControl);
                        break;

                    case "Clone":
                        command.Description = "Clone selected WebElement with specified name";
                        command.Command = new CloneWebElementCommand(_webElementsTreeUserControl);
                        break;

                    default:
                        break;
                }

                _cache[key] = command;
            }
            return _cache[key];
        }
    }
}
