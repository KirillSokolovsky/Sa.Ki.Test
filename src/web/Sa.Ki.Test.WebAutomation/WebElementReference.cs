namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WebElementReference : CombinedWebElementInfo, IWebElementWithReferenceInfo
    {
        public WebElementInfo ReferencedElement { get; set; }
        public string TreePathToReferencedElement { get; set; }

        IWebElementInfo IWebElementWithReferenceInfo.ReferencedElement => ReferencedElement;

        public WebElementReference()
        {
            ElementType = WebElementTypes.Reference;
        }

        public override WebElementInfo GetCopyWithoutParent(WebElementInfo webElementInfo = null)
        {
            var element = webElementInfo as WebElementReference ??
                new WebElementReference();

            element.TreePathToReferencedElement = TreePathToReferencedElement;

            return base.GetCopyWithoutParent(webElementInfo);
        }
    }
}
