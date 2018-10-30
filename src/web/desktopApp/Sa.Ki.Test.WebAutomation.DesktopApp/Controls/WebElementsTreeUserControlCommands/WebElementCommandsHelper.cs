namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class WebElementCommandsHelper
    {
        public static bool CanElementHasCustomChildren(WebElementInfoViewModel elementInfo)
        {
            return
              elementInfo == null //It's a tree view root, so we could add at least Context
              || (elementInfo is CombinedWebElementInfoViewModel
                  && !(elementInfo.ElementType == WebElementTypes.DropDown || elementInfo.ElementType == WebElementTypes.RadioGroup));
        }

        public static bool CanBeCut(WebElementInfoViewModel el)
        {
            if (el == null) return false;
            if (el.Parent == null) return false;
            if (el.Parent.ElementType == WebElementTypes.DropDown || el.Parent.ElementType == WebElementTypes.RadioGroup)
                return false;
            if (el.Parent.ElementType == WebElementTypes.Control || el.Parent.ElementType == WebElementTypes.Context)
                return true;

            return false;
        }
        public static bool CanBeCloned(WebElementInfoViewModel el)
        {
            if (el == null) return false;
            if (el.ElementType == WebElementTypes.Context && el.Parent == null)
                return true;
            if (el.Parent == null) return false;
            if (el.Parent.ElementType == WebElementTypes.DropDown || el.Parent.ElementType == WebElementTypes.RadioGroup)
                return false;
            if (el.Parent.ElementType == WebElementTypes.Control || el.Parent.ElementType == WebElementTypes.Context)
                return true;

            return false;
        }

        public static Func<WebElementInfoViewModel, string> GetCreateUpdateWebElementValidator(
            WebElementsTreeUserControl webElementsTreeUserControl,
            string originalName)
        {
            Func<WebElementInfoViewModel, string> validator = el =>
            {
                StringBuilder result = new StringBuilder();

                if (!VerifyName(el, webElementsTreeUserControl, originalName, out var error))
                    result.AppendLine(error);

                if (string.IsNullOrWhiteSpace(el.Name))
                    result.AppendLine("WebElement Name couldn't be empty");
                if (string.IsNullOrWhiteSpace(el.Description))
                    result.AppendLine("WebElement Description couldn't be empty");
                if (string.IsNullOrWhiteSpace(el.Locator.LocatorValue))
                    result.AppendLine("WebElement Locator.LocatorValue couldn't be empty");

                return result.ToString();
            };

            return validator;
        }

        public static bool VerifyName(WebElementInfoViewModel el,
            WebElementsTreeUserControl webElementsTreeUserControl,
            string originalName,
            out string error)
        {
            error = null;

            var existedNames = GetExistedNamesForChildWebElements(
                el,
                webElementsTreeUserControl,
                originalName);

            if (existedNames.Contains(el.Name))
            {
                error = $"WebElement with name: {el.Name} already exists on the level";
                return false;
            }
            return true;
        }

        public static List<string> GetExistedNamesForChildWebElements(WebElementInfoViewModel el,
            WebElementsTreeUserControl webElementsTreeUserControl,
            string originalName)
        {

            List<string> existedNames = el.Parent == null
                ? webElementsTreeUserControl.WebContexts?.Select(c => c.Name).ToList()
                : el.Parent.Elements?.Select(e => e.Name).ToList();

            if (originalName != null)
                existedNames.Remove(originalName);

            return existedNames;
        }
    }
}
