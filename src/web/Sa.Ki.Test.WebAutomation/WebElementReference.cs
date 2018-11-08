namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WebElementReference : WebElementInfo
    {
        public WebElementInfo ReferencedElement { get; set; }
        public List<string> Path { get; set; }

        public WebElementReference()
        {
            ElementType = WebElementTypes.Reference;
        }
    }
}
