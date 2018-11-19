using System;
using System.Collections.Generic;
using System.Text;

namespace Sa.Ki.Test.WebAutomation
{
    public interface ICombinedWebElementInfo : IWebElementInfo
    {
        IEnumerable<IWebElementInfo> Elements { get; }
    }
}
