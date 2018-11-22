namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class FrameWebElementInfo : CombinedWebElementInfo, IWebElementWithReferenceInfo
    {
        public WebElementInfo InnerElement { get; set; }
        public string TreePathToInnerElement { get; set; }

        IWebElementInfo IWebElementWithReferenceInfo.ReferencedElement => InnerElement;

        public FrameWebElementInfo()
        {
            ElementType = WebElementTypes.Frame;
        }

        public override WebElementInfo GetCopyWithoutParent(WebElementInfo webElementInfo = null)
        {
            var element = webElementInfo as FrameWebElementInfo ??
                new FrameWebElementInfo();

            element.TreePathToInnerElement = TreePathToInnerElement;

            return base.GetCopyWithoutParent(webElementInfo);
        }
    }
}
