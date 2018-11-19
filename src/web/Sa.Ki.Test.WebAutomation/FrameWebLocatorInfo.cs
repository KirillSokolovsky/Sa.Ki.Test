
namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class FrameWebLocatorInfo : WebLocatorInfo, IFrameWebLocatorInfo
    {
        public FrameLocatorType FrameLocatorType { get; set; }

        public override WebLocatorInfo GetCopy(WebLocatorInfo webLocatorInfo = null)
        {
            var frameLocator = new FrameWebLocatorInfo
            {
                FrameLocatorType = FrameLocatorType
            };
            return base.GetCopy(frameLocator);
        }
    }
}
