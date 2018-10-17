namespace Sa.Ki.Test.WebAutomation.Selenium
{
    using OpenQA.Selenium;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IWebDriverFactory
    {
        IWebDriver CreateDriver(out string driverType, out string driverInfo);
    }
}
