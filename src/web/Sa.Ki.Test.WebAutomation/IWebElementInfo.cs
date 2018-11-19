namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IWebElementInfo
    {
        string ElementType { get; }
        IWebLocatorInfo Locator { get; }
        ICombinedWebElementInfo Parent { get; }
        WebSearchInfo GetWebSearch(bool reBuild = false);
    }
}
