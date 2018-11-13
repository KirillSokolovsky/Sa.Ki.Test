using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    public static class WebElementsViewModelsFactory
    {
        public static WebElementInfoViewModel CreateModelFromInfo(WebElementInfo elementInfo)
        {
            switch (elementInfo)
            {
                case WebContextInfo wc:
                    return new WebContextInfoViewModel(wc);
                case CombinedWebElementInfo cw:
                    return new CombinedWebElementInfoViewModel(cw);
                default:
                    return new WebElementInfoViewModel(elementInfo);
            }
        }
        public static WebElementInfo CreateInfoFromModel(WebElementInfoViewModel model, CombinedWebElementInfo parent = null)
        {
            WebElementInfo info = null;

            switch (model)
            {
                case CombinedWebElementInfoViewModel combinedModel:
                    {
                        CombinedWebElementInfo combined = null;
                        switch (combinedModel.ElementType)
                        {
                            case WebElementTypes.Context:
                                combined = new WebContextInfo();
                                break;

                            case WebElementTypes.DropDown:
                                combined = new DropDownElementInfo();
                                break;

                            case WebElementTypes.RadioGroup:
                                combined = new RadioGroupElementInfo();
                                break;

                            default:
                                combined = new CombinedWebElementInfo();
                                break;
                        }
                        info = combined;
                        combined.Elements = combinedModel.Elements
                            ?.Select(em => CreateInfoFromModel(em, combined))
                            .ToList();
                    }
                    break;

                default:
                    info = new WebElementInfo();
                    break;
            }

            info.Name = model.Name;
            info.Description = model.Description;
            info.InnerKey = model.InnerKey;
            info.Tags = model.Tags?.ToList();
            info.Locator = model.Locator.GetLocatorInfo();
            info.IsKey = model.IsKey;
            info.Parent = parent;

            return info;
        }

        public static WebElementInfoViewModel CreateModelFromWebElementType(string elementType)
        {
            switch (elementType)
            {
                case WebElementTypes.Context:
                    return new WebContextInfoViewModel();
                case WebElementTypes.Control:
                case WebElementTypes.DropDown:
                case WebElementTypes.RadioGroup:
                    return new CombinedWebElementInfoViewModel();
                case WebElementTypes.Element:
                    return new WebElementInfoViewModel();
                default:
                    return null;
            }
        }

        public static WebElementInfoViewModel GetCopyOfBaseInformation(WebElementInfoViewModel webElementInfo)
        {
            var info = new WebElementInfoViewModel
            {
                Name = webElementInfo.Name,
                Description = webElementInfo.Description,
                ElementType = webElementInfo.ElementType,
                InnerKey = webElementInfo.InnerKey,
                IsKey = webElementInfo.IsKey,
                Tags = webElementInfo.Tags == null
                    ? null
                    : new ObservableCollection<string>(webElementInfo.Tags),
                Locator = new WebLocatorInfoViewModel
                {
                    IsRelative = webElementInfo.Locator.IsRelative,
                    LocatorType = webElementInfo.Locator.LocatorType,
                    LocatorValue = webElementInfo.Locator.LocatorValue
                }
            };

            return info;
        }
        public static void FillModelWithBaseInfo(WebElementInfoViewModel model, WebElementInfoViewModel info)
        {
            model.Name = info.Name;
            model.Description = info.Description;
            model.InnerKey = info.InnerKey;
            model.ElementType = info.ElementType;
            model.IsKey = info.IsKey;

            if (info.Tags == null)
                model.Tags = null;
            else
            {
                if (model.Tags == null)
                    model.Tags = new ObservableCollection<string>();

                var toRemove = model.Tags.Where(mt => !info.Tags.Contains(mt));
                var toAdd = info.Tags.Where(it => !model.Tags.Contains(it));

                toRemove.ToList().ForEach(t => model.Tags.Remove(t));
                toAdd.ToList().ForEach(t => model.Tags.Add(t));
            }

            model.Locator.IsRelative = info.Locator.IsRelative;
            model.Locator.LocatorType = info.Locator.LocatorType;
            model.Locator.LocatorValue = info.Locator.LocatorValue;
        }

        public static void Filter(this ObservableCollection<CombinedWebElementInfoViewModel> contexts, Func<WebElementInfoViewModel, bool> filter, ref int resultsCount)
        {
            if(contexts != null)
            {
                foreach (var c in contexts)
                {
                    Filter(c, filter, ref resultsCount);
                }
            }
        }
        private static bool Filter(WebElementInfoViewModel wbElementInfo, Func<WebElementInfoViewModel, bool> filter, ref int resultsCount)
        {
            var current = resultsCount;
            var result = false;
            if (wbElementInfo is CombinedWebElementInfoViewModel cel)
            {
                if (cel.Elements != null)
                {
                    foreach (var el in cel.Elements)
                    {
                        result = Filter(el, filter, ref resultsCount) || result;
                    }
                }
            }

            if (filter(wbElementInfo))
            {
                resultsCount++;
                wbElementInfo.IsVisible = true;
                result = true;
            }
            else
            {
                result = false || result;
                wbElementInfo.IsVisible = result;
            }

            return result;
        }

        private const string _webElementsHierarchyDelimeter = " > ";
        public static string ToBreadString(this WebElementInfoViewModel webElementInfo)
        {
            var el = webElementInfo;
            var sb = new StringBuilder(el.Name);
            el = el.Parent;

            while (el != null)
            {
                sb.Insert(0, $"{el.Name}{_webElementsHierarchyDelimeter}");
                el = el.Parent;
            }

            return sb.ToString();
        }

        public static WebElementInfoViewModel FindByBreadString(this ObservableCollection<CombinedWebElementInfoViewModel> combinedEls, string breadString)
        {
            var parts = breadString.Split(new[] { _webElementsHierarchyDelimeter }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return null;

            var cEl = combinedEls?.FirstOrDefault(ce => ce.Name == parts[0]);
            WebElementInfoViewModel result = cEl;

            if (result != null && parts.Length > 0)
            { 
                result = FindByBreadString(cEl, parts.Skip(1));
            }

            return result;
        }
        private static WebElementInfoViewModel FindByBreadString(CombinedWebElementInfoViewModel combinedWebElement, IEnumerable<string> breadStrings)
        {
            if (combinedWebElement.Elements == null)
                return null;

            var name = breadStrings.First();

            var el = combinedWebElement.Elements.FirstOrDefault(e => e.Name == name);
            if (el == null) return null;
            if (breadStrings.Count() == 1) return el;

            if(el is CombinedWebElementInfoViewModel cwe)
            {
                return FindByBreadString(cwe, breadStrings.Skip(1));
            }

            return null;
        }
    }
}
