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

            if(!_menusCache.ContainsKey(key))
            {
                var items = new List<SaKiMenuItemViewModel>();

                if (elementInfo != null)
                    items.Add(CreateCopyNameMenuItem());

                if (WebElementCommandsHelper.CanElementHasCustomChildren(elementInfo))
                    items.Add(CreateCreateMenuItemGroup(et));

                items.Add(CreateActionsMenuItemGroup(et, p));

                _menusCache[key] = items;
            }
            return _menusCache[key];
        }

        public SaKiMenuItemViewModel CreateCopyNameMenuItem()
        {
            var key = "Copy Name";
            if(!_cache.ContainsKey(key))
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
            var key = $"Create New for {elementType}";
            if (!_cache.ContainsKey(key))
            {
                _cache[key] = new SaKiGroupMenuItemViewModel
                {
                    Name = "Create New",
                    Description = "Create new child WebElementInfo"
                };
            }
            return _cache[key];
        }

        public SaKiMenuItemViewModel CreateActionsMenuItemGroup(string elementType, string parentType)
        {
            var key = $"Create actions for {elementType} {parentType}";
            if (!_cache.ContainsKey(key))
            {
                var group = new SaKiGroupMenuItemViewModel
                {
                    Name = "Create New",
                    Description = "Create new child WebElementInfo",
                    Items = new ObservableCollection<SaKiMenuItemViewModel>()
                };

                switch ((elementType, parentType))
                {
                    case (s, a) when s == "" && a == "":
                        group.Items.Add(CreateActionsCommandMenuItem("Paste"));
                        break;
                    case WebElementTypes.:
                        group.Items.Add(CreateActionsCommandMenuItem("Paste"));
                        break;
                    default:
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

                    default:
                        break;
                }

                _cache[key] = command;
            }
            return _cache[key];
        }
    }
}
