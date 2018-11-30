namespace Sa.Ki.Test.WebAutomation.Selenium.Exceptions
{
    using Sa.Ki.Test.WebAutomation.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SaKiWebElementException : SaKiWebDriverException
    {
        public static SaKiWebElementException TimeoutDuring(string processDesc, int timeoutInSec, WebElementInfo elementInfo)
            => new SaKiWebElementException(nameof(TimeoutDuring), $"Timeout {timeoutInSec} reached during {processDesc}", elementInfo);
        public static SaKiWebElementException ErrorDuring(string processDesc, WebElementInfo elementInfo, Exception exception)
            => new SaKiWebElementException(nameof(ErrorDuring), $"Error occureed during {processDesc}", elementInfo, exception);

        public WebElementInfo ElementInfo { get; set; }

        public SaKiWebElementException(string type, string message, WebElementInfo elementInfo, Exception innerException = null)
            : base($"WebElement.{type}", message, innerException)
        {
            ElementInfo = elementInfo;
        }
    }
}
