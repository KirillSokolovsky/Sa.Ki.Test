namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IWebElementWithReferenceInfo : IWebElementInfo
    {
        IWebElementInfo ReferencedElement { get; }
    }
}
