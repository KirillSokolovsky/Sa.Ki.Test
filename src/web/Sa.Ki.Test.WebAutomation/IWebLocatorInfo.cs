namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IWebLocatorInfo
    {
        string LocatorValue { get; }
        WebLocatorType LocatorType { get; }
        bool IsRelative { get; }
    }
}
