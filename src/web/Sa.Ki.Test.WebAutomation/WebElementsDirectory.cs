namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WebElementsDirectory : CombinedWebElementInfo
    {
        public WebElementsDirectory()
        {
            ElementType = WebElementTypes.Directory;
        }


        public override WebElementInfo GetCopyWithoutParent(WebElementInfo webElementInfo = null)
        {
            var element = webElementInfo as WebElementsDirectory
                ?? new WebElementsDirectory();

            return base.GetCopyWithoutParent(element);
        }
    }
}
