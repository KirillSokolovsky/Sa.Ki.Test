namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WebSearchInfo
    {
        public WebSearchInfo ParentSearch { get; set; }
        public WebLocatorType LocatorType { get; set; }
        public string LocatorValue { get; set; }
    }
}
