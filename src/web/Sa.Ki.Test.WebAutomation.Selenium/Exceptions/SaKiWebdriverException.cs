namespace Sa.Ki.Test.WebAutomation.Selenium.Exceptions
{
    using Sa.Ki.Test.WebAutomation.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SaKiWebDriverException : SakiWebAutomationException
    {
        public static SaKiWebDriverException ErrorDuring(string processDesc, Exception exception)
            => new SaKiWebDriverException(nameof(ErrorDuring), $"Error occureed during {processDesc}", exception);
        
        public SaKiWebDriverException(string type, string message, Exception innerException = null)
            : base($"WebDriver.{type}", message, innerException)
        {
        }
    }
}
