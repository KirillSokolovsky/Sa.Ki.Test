namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WebLocatorInfo : IWebLocatorInfo
    {
        public string LocatorValue { get; set; }
        public WebLocatorType LocatorType { get; set; }
        public bool IsRelative { get; set; }

        public virtual WebLocatorInfo GetCopy(WebLocatorInfo webLocatorInfo = null)
        {
            if (webLocatorInfo == null)
                webLocatorInfo = new WebLocatorInfo();

            webLocatorInfo.IsRelative = IsRelative;
            webLocatorInfo.LocatorType = LocatorType;
            webLocatorInfo.LocatorValue = LocatorValue;

            return webLocatorInfo;
        }
    }
}
