using System;
using System.Collections.Generic;
using System.Text;

namespace Sa.Ki.Test.WebAutomation.Exceptions
{
    public class SakiWebElementException : SakiWebAutomationException
    {
        public IWebElementInfo WebElementInfo { get; set; }

        public SakiWebElementException(string type, string message, IWebElementInfo webElementInfo, Exception innerException = null) 
            : base(type, message, innerException)
        {
            WebElementInfo = webElementInfo;
        }
    }
}
