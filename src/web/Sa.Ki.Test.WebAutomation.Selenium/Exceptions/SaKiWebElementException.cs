namespace Sa.Ki.Test.WebAutomation.Selenium.Exceptions
{
    using Sa.Ki.Test.WebAutomation.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SaKiWebElementException : SaKiWebdriverException
    {
        public static SaKiWebElementException TimeoutDuring(string processDesc, int timeoutInSec, WebElementInfo elementInfo)
            => new SaKiWebElementException($"Timeout {timeoutInSec} reached during {processDesc}", elementInfo);
        public static SaKiWebElementException ErrorDuring(string processDesc, WebElementInfo elementInfo, Exception exception)
            => new SaKiWebElementException($"Error occureed during {processDesc}", elementInfo, exception);

        public WebElementInfo ElementInfo { get; set; }

        public SaKiWebElementException(string message, WebElementInfo elementInfo, Exception innerException = null)
            : base(message, innerException)
        {
            ElementInfo = elementInfo;
        }
    }
}
