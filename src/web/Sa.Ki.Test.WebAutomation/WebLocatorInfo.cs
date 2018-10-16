namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WebLocatorInfo
    {
        public string LocatorValue { get; set; }
        public WebLocatorType LocatorType { get; set; }
        public bool IsRelative { get; set; }

        public WebLocatorInfo GetCopy()
        {
            return new WebLocatorInfo
            {
                IsRelative = IsRelative,
                LocatorType = LocatorType,
                LocatorValue = LocatorValue
            };
        }
    }
}
