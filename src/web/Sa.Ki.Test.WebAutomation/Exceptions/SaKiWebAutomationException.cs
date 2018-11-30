namespace Sa.Ki.Test.WebAutomation.Exceptions
{
    using Sa.Ki.Test.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SakiWebAutomationException : SaKiException
    {
        public SakiWebAutomationException(string type, string message, Exception innerException = null) 
            : base($"WebAutomation.{type}", message, innerException)
        {
        }
    }
}
