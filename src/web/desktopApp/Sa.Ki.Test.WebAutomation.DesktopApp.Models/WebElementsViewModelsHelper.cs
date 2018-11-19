namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using Sa.Ki.Test.SakiTree;

    public static class WebElementsViewModelsHelper
    {
        public static WebElementInfoViewModel CreateModelFromInfo(WebElementInfo elementInfo)
        {
            switch (elementInfo)
            {
                case WebElementReference re:
                    return new WebElementWithReferenceViewModel(re);
                case FrameWebElementInfo wc:
                    return new WebElementWithReferenceViewModel(wc);
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

                            case WebElementTypes.Directory:
                                combined = new WebElementsDirectory();
                                break;

                            case WebElementTypes.Control:
                                combined = new CombinedWebElementInfo();
                                break;

                            default:
                                throw new Exception($"Unexpected combinedModel.ElementType: {combinedModel.ElementType}");
                        }
                        info = combined;
                        combined.Elements = combinedModel.Elements
                            ?.Select(em => CreateInfoFromModel(em, combined))
                            .ToList();
                    }
                    break;

                case WebElementWithReferenceViewModel wr:
                    {
                        switch (wr.ElementType)
                        {
                            case WebElementTypes.Frame:

                                var f = new FrameWebElementInfo();
                                f.TreePathToInnerElement = wr.ReferenceBreadString;
                                info = f;

                                break;
                            case WebElementTypes.Reference:

                                var r = new WebElementReference();
                                r.TreePathToReferencedElement = wr.ReferenceBreadString;
                                info = r;

                                break;

                            default:
                                throw new Exception($"Unexpected WebElementWithReferenceViewModel ElementType: {wr.ElementType}");

                        }
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
            info.Locator = model.Locator?.GetLocatorInfo();
            info.IsKey = model.IsKey;
            info.Parent = parent;

            return info;
        }

        public static WebElementInfoViewModel CreateModelFromWebElementType(string elementType)
        {
            switch (elementType)
            {
                case WebElementTypes.Directory:
                case WebElementTypes.Context:
                case WebElementTypes.Control:
                case WebElementTypes.DropDown:
                case WebElementTypes.RadioGroup:
                    return new CombinedWebElementInfoViewModel();
                case WebElementTypes.Frame:
                case WebElementTypes.Reference:
                    return new WebElementWithReferenceViewModel(WebElementTypes.Reference);
                case WebElementTypes.Element:
                    return new WebElementInfoViewModel();
                default:
                    throw new Exception($"Unknown WebElementTypes to create model: {elementType}");
            }
        }

        public static WebElementWithReferenceViewModel GetCopyOfBaseInformation(WebElementInfoViewModel webElementInfo)
        {
            var info = new WebElementWithReferenceViewModel
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

            if (webElementInfo is WebElementWithReferenceViewModel refs)
                info.ReferenceBreadString = refs.ReferenceBreadString;

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

            if (model is WebElementWithReferenceViewModel refModel)
                refModel.ReferenceBreadString = (info as WebElementWithReferenceViewModel).ReferenceBreadString;

            model.Locator.IsRelative = info.Locator.IsRelative;
            model.Locator.LocatorType = info.Locator.LocatorType;
            model.Locator.LocatorValue = info.Locator.LocatorValue;
        }

        public static void Filter(this ObservableCollection<CombinedWebElementInfoViewModel> contexts, Func<WebElementInfoViewModel, bool> filter, ref int resultsCount)
        {
            if (contexts != null)
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

        public static WebElementInfoViewModel ClearAccrodingToBlocked(WebElementInfoViewModel webElementInfo,
            List<string> blockedElementsBreadStrings,
            List<string> blockedElementTypes)
        {
            var breadStr = webElementInfo.GetTreePath();
            if ((blockedElementsBreadStrings?.Contains(breadStr) ?? false) ||
                (blockedElementTypes?.Contains(webElementInfo.ElementType) ?? false))
                return null;

            if (webElementInfo is CombinedWebElementInfoViewModel cwe)
            {
                if (cwe.Elements != null)
                {
                    for (int i = 0; i < cwe.Elements.Count; i++)
                    {
                        var cleared = ClearAccrodingToBlocked(cwe.Elements[i],
                            blockedElementsBreadStrings,
                            blockedElementTypes);

                        if (cleared == null)
                        {
                            cwe.Elements.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            return webElementInfo;
        }

        public static List<string> GetBlockedElementTypesForElementType(string elementType)
        {
            switch (elementType)
            {
                case WebElementTypes.Reference:
                    return new List<string> { WebElementTypes.Reference };

                case WebElementTypes.Frame:
                    return new List<string>
                    {
                        WebElementTypes.Reference,
                        WebElementTypes.Frame,
                        WebElementTypes.Element,
                        WebElementTypes.RadioGroup,
                        WebElementTypes.DropDown
                    };
                default:
                    return null;
            }
        }

        public static WebLocatorInfoViewModel CreateLocatorModel(WebLocatorInfo webLocatorInfo)
        {
            WebLocatorInfoViewModel model = null;
            if (webLocatorInfo is FrameWebLocatorInfo fli)
            {
                model = new FrameWebLocatorInfoViewModel(webLocatorInfo)
                {
                    FrameLocatorType = fli.FrameLocatorType
                };
            }
            else model = new WebLocatorInfoViewModel(webLocatorInfo);

            return model;
        }

        public static WebSearchInfoModel CreateWebSearchModelFromInfo(WebSearchInfo webSearchInfo)
        {
            WebSearchInfoModel model = null;

            if (webSearchInfo is FrameWebSearchInfo fwsi)
                model = new FrameWebSearchInfoViewModel(fwsi);
            else model = new WebSearchInfoModel(webSearchInfo);

            return model;
        }
    }
}
