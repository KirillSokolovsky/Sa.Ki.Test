using System;
using System.Collections.Generic;
using System.Text;

namespace Sa.Ki.Test.WebAutomation.Exceptions
{
    public class SakiWebElementException : SakiWebAutomationException
    {
        public IWebElementInfo WebElementInfo { get; set; }

        public SakiWebElementException(string message, IWebElementInfo webElementInfo, Exception innerException = null) 
            : base(message, innerException)
        {
            WebElementInfo = webElementInfo;
        }
    }
}
