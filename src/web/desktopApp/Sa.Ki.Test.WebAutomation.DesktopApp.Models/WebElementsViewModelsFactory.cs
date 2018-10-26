using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
