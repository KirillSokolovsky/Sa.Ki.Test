namespace Sa.Ki.Test.WebAutomation.Exceptions
{
    using Sa.Ki.Test.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SaKiWebAutomationException : SaKiException
    {
        public SaKiWebAutomationException(string message, Exception innerException = null) 
            : base(nameof(WebAutomation), message, innerException)
        {
        }
    }
}
