namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class FrameWebElementInfo : WebElementInfo
    {
        public WebContextInfo WebContext { get; set; }
        public string Path { get; set; }

        public FrameWebElementInfo()
        {
            ElementType = WebElementTypes.Frame;
        }
    }
}
