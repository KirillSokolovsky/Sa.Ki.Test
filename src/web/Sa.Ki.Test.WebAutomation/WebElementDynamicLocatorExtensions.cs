namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class WebElementDynamicLocatorExtensions
    {
        public static IDynamicWebLocatorResolver DefaultDynamicLocatorResolver { get; set; }
            = new DefaultDynamicWebLocatorResolver();

        public static WebElementInfo GetCopyWithResolvedDynamicLocator(
            this WebElementInfo elementInfo,
            IDynamicWebLocatorResolver locatorResolver,
            params (string parName, object parValue)[] values)
        {
            var copied = elementInfo.GetCopyWithoutParent();

            copied.Parent = elementInfo.Parent;
            copied.Locator.LocatorValue =
                (locatorResolver ?? DefaultDynamicLocatorResolver)
                    .ResolveDynamicLocatorValue(elementInfo.Locator.LocatorValue, values);

            return copied;
        }

        public static WebElementInfo GetCopyWithResolvedDynamicLocator(
            this WebElementInfo elementInfo, 
            params (string parName, object parValue)[] values)
        {
            return elementInfo.GetCopyWithResolvedDynamicLocator(null, values);
        }

        public static WebElementInfo GetCopyWithResolvedDynamicLocator(
            this WebElementInfo elementInfo,
            IDynamicWebLocatorResolver locatorResolver,
            params object[] values)
        {
            return elementInfo.GetCopyWithResolvedDynamicLocator(locatorResolver, values);
        }

        public static WebElementInfo GetCopyWithResolvedDynamicLocator(
            this WebElementInfo elementInfo,
            params object[] values)
        {
            return elementInfo.GetCopyWithResolvedDynamicLocator(null, values);
        }
    }
}
