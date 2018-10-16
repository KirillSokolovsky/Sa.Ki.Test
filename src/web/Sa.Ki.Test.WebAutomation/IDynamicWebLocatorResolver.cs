namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IDynamicWebLocatorResolver
    {
        string ResolveDynamicLocatorValue(string locator, params (string parName, object parValue)[] values);
        string ResolveDynamicLocatorValue(string locator, params object[] values);
    }
}
