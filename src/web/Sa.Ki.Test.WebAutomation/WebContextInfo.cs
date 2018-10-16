namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WebContextInfo : CombinedWebElementInfo
    {
        public WebContextInfo()
        {
            ElementType = WebElementTypes.Context;
        }

        public override WebElementInfo GetCopyWithoutParent(WebElementInfo webElementInfo = null)
        {
            var element = webElementInfo as WebContextInfo
                ?? new WebContextInfo();

            return base.GetCopyWithoutParent(element);
        }
    }
}
