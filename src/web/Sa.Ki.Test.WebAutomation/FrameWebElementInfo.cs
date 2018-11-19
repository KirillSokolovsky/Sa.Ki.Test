namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class FrameWebElementInfo : WebElementInfo, IWebElementWithReferenceInfo
    {
        public WebElementInfo InnerElement { get; set; }
        public string TreePathToInnerElement { get; set; }

        IWebElementInfo IWebElementWithReferenceInfo.ReferencedElement => InnerElement;

        public FrameWebElementInfo()
        {
            ElementType = WebElementTypes.Frame;
        }
    }
}
