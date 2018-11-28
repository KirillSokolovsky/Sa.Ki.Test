using System;
using System.Collections.Generic;
using System.Text;

namespace Sa.Ki.Test.WebAutomation
{
    public class WebPageInfo : CombinedWebElementInfo
    {
        public string DefaultUrl { get; set; }
        public string UrlRegexString { get; set; }

        public WebPageInfo()
        {
            ElementType = WebElementTypes.Page;
        }

        public override WebElementInfo GetCopyWithoutParent(WebElementInfo webElementInfo = null)
        {
            var element = webElementInfo as WebPageInfo
                ?? new WebPageInfo();

            return base.GetCopyWithoutParent(element);
        }
    }
}
