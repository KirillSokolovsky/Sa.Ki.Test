namespace Sa.Ki.Test.WebAutomation.Selenium.Exceptions
{
    using Sa.Ki.Test.WebAutomation.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SaKiWebdriverException : SakiWebAutomationException
    {
        public static SaKiWebdriverException ErrorDuring(string processDesc, Exception exception)
            => new SaKiWebdriverException($"Error occureed during {processDesc}", exception);
        
        public SaKiWebdriverException(string message, Exception innerException = null)
            : base(message, innerException)
        {
            SubType = nameof(Selenium);
        }
    }
}
