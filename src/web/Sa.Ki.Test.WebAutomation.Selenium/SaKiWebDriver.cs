namespace Sa.Ki.Test.WebAutomation.Selenium
{
    using OpenQA.Selenium;
    using Sa.Ki.Test.Logging;
    using Sa.Ki.Test.WebAutomation.Selenium.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class SaKiWebDriver
    {
        public int DefaultTimeoutForPollingInSec = 1;
        public int DefaultTimeoutForSearchInSec = 20;
        public int DefaultTimeoutForTrySearchInMilisec = 500;
        public int DefaultPageLoadTimeoutInSec = 30;
        public bool IsScreenshotOnErrorEnabled = true;

        private IWebDriver _driver { get; set; }
        private IWebDriverFactory _webDriverFactory;

        private string _driverType;
        private string _driverInfo;

        public string DriverType => _driverType;
        public string DriverInfo => _driverInfo;

        private Action<IWebDriver> _driverConfigurator { get; set; }

        public SaKiWebDriver(IWebDriverFactory webDriverFactory)
        {
            _webDriverFactory = webDriverFactory;
        }

        public bool IsDriverCreated()
        {
            return _driver != null;
        }

        public void SetDriverConfigurator(Action<IWebDriver> driverConfigurator)
        {
            _driverConfigurator = driverConfigurator;
        }

        private IWebDriver CreateDriver()
        {
            try
            {
                _driver = _webDriverFactory.CreateDriver(out _driverType, out _driverInfo);
            }
            catch (Exception ex)
            {
                throw SaKiWebdriverException.ErrorDuring("WebDriver creation", ex);
            }

            try
            {
                (_driverConfigurator
                    ?? ((driver) => _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(DefaultPageLoadTimeoutInSec)))
                    .Invoke(_driver);

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(DefaultTimeoutForSearchInSec);
            }
            catch(Exception ex)
            {
                throw SaKiWebdriverException.ErrorDuring("WebDriver creation", ex);
            }

            return _driver;
        }

        public IWebDriver GetDriver()
        {
            return _driver ?? CreateDriver();
        }

        public void DisposeDriver()
        {
            if (IsDriverCreated())
            {
                try
                {
                    _driver.Quit();
                }
                catch { }
            }
        }

        public IJavaScriptExecutor GetJavaScriptExecutor()
        {
            return (IJavaScriptExecutor)GetDriver();
        }
        public ITakesScreenshot GetTakesScreenshot()
        {
            return (ITakesScreenshot)GetDriver();
        }

        public string GetCurrentUrl(ILogger log)
        {
            log = log?.CreateChildLogger("Getting current url");
            var url = GetDriver().Url;
            log?.INFO($"Current url: {url}");
            return url;
        }

        public IWebElement FindElement(By by)
        {
            return GetDriver().FindElement(by);
        }
        public IWebElement FindElement(WebElementInfo elementInfo, ILogger log)
        {
            log = log?.CreateChildLogger($"Find {elementInfo}");
            log?.INFO($"Description: {elementInfo.Description}");

            var search = elementInfo.GetWebSearch();
            var searchStack = new Stack<WebSearchInfo>();
            searchStack.Push(search);

            while (search.ParentSearch != null)
            {
                searchStack.Push(search.ParentSearch);
                search = search.ParentSearch;
            }

            var searchLog = log?.CreateChildLogger("Search element");
            searchLog?.INFO($"Search contains: {searchStack.Count} items");

            IWebElement foundElement = null;

            var counter = 0;
            while (searchStack.Count > 0)
            {
                var currentSearch = searchStack.Pop();
                searchLog?.INFO($"Search for {++counter} item");
                searchLog?.INFO($"Locator type: {currentSearch.LocatorType}");
                searchLog?.INFO($"Locator value: {currentSearch.LocatorValue}");

                var by = GetBy(currentSearch);

                try
                {
                    if (foundElement == null)
                        foundElement = FindElement(by);
                    else
                        foundElement = FindElementRelativeTo(foundElement, by);
                }
                catch (Exception ex)
                {
                    var err = new SaKiWebElementException("WebElement could't be found", elementInfo, ex);
                    log?.ERROR(err.Message, err);
                    throw err;
                }
            }

            log?.INFO("Element found successfully");

            return foundElement;
        }
        public IWebElement FindElementRelativeTo(IWebElement parentElement, By by)
        {
            return parentElement.FindElement(by);
        }
        public IWebElement FindElementRelativeTo(WebElementInfo parentElementInfo, By by, ILogger log)
        {
            log = log?.CreateChildLogger($"Find element relativeTo: {parentElementInfo}");
            log?.INFO($"using locator: {by}");

            var parentElement = FindElement(parentElementInfo, log);
            var foundElement = FindElementRelativeTo(parentElement, by);

            return foundElement;
        }

        public IWebElement TryFindElementWithTimeFrame(WebElementInfo elementInfo, int timeoutInMiliSec = 0, bool changeTimeouts = true)
        {
            if (timeoutInMiliSec == 0)
                timeoutInMiliSec = DefaultTimeoutForTrySearchInMilisec;

            try
            {
                if (changeTimeouts)
                    GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(timeoutInMiliSec);

                return FindElement(elementInfo, null);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (changeTimeouts)
                    GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(DefaultTimeoutForSearchInSec);
            }
        }

        public IWebElement WaitForElementState(WebElementInfo elementInfo, WebElementState elementState, ILogger log, int timeoutInSec = 0, int pollingInMiliSec = 0)
        {
            if (timeoutInSec == 0)
                timeoutInSec = DefaultTimeoutForSearchInSec;
            if (pollingInMiliSec == 0)
                pollingInMiliSec = DefaultTimeoutForPollingInSec;

            log = log?.CreateChildLogger($"Wait for state: {elementState} for {elementInfo}");
            log?.INFO($"With timeout in {timeoutInSec} seconds and with polling interval in {pollingInMiliSec} seconds");


            var flags = Enum.GetValues(typeof(WebElementState)).Cast<WebElementState>();
            var expectedFlags = flags.Where(f => elementState.HasFlag(f)).ToList();

            try
            {
                GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(DefaultTimeoutForTrySearchInMilisec);

                var sw = Stopwatch.StartNew();
                long mseconds = 0;
                while (sw.Elapsed.TotalSeconds <= timeoutInSec)
                {
                    mseconds = sw.ElapsedMilliseconds;
                    var state = GetElementState(elementInfo, out var element, null, false);

                    if (expectedFlags.All(f => state.HasFlag(f)))
                        return element;

                    var toSleep = pollingInMiliSec * 1000 - (sw.ElapsedMilliseconds - mseconds);
                    if (toSleep > 0)
                        Thread.Sleep((int)toSleep);
                }

                var err = SaKiWebElementException.TimeoutDuring($"waiting for element state: {elementState}", timeoutInSec, elementInfo);
                log?.ERROR(err.Message, err);
                throw err;
            }
            finally
            {
                GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(DefaultTimeoutForSearchInSec);
            }
        }
        public WebElementState GetElementState(WebElementInfo elementInfo, out IWebElement element, ILogger log, bool changeTimeouts = true)
        {
            log = log?.CreateChildLogger($"Get state for {elementInfo}");
            log?.INFO($"Description: {elementInfo.Description}");

            element = TryFindElementWithTimeFrame(elementInfo, DefaultTimeoutForTrySearchInMilisec, changeTimeouts);
            if (element == null)
            {
                log?.INFO($"State: {WebElementState.NotPresent}");
                return WebElementState.NotPresent;
            }

            if (!element.Displayed)
            {
                log?.INFO($"State: {WebElementState.NotVisible}");
                return WebElementState.NotVisible;
            }

            if (!element.Enabled)
            {
                log?.INFO($"State: {WebElementState.Disabled}");
                return WebElementState.Disabled;
            }

            log?.INFO($"State: {WebElementState.Enabled}");
            return WebElementState.Enabled;
        }

        public void Click(WebElementInfo elementInfo, ILogger log)
        {
            try
            {
                log = log?.CreateChildLogger($"Click on {elementInfo}");
                LogElementPath(elementInfo, log);
                var element = WaitForElementState(elementInfo, WebElementState.ReadyForAction, log);
                element.Click();
            }
            catch(Exception ex)
            {
                if (IsScreenshotOnErrorEnabled)
                    TryLogScreenShot($"Failed to Click on {elementInfo}", log);
                throw SaKiWebElementException.ErrorDuring("Click on element", elementInfo, ex);
            }
        }
        public void Type(string text, WebElementInfo elementInfo, ILogger log)
        {
            try
            {
                log = log?.CreateChildLogger($"Type text '{text}' to {elementInfo}");
                LogElementPath(elementInfo, log);
                var element = WaitForElementState(elementInfo, WebElementState.ReadyForAction, log);
                element.SendKeys(text);
            }
            catch (Exception ex)
            {
                if (IsScreenshotOnErrorEnabled)
                    TryLogScreenShot($"Failed to Type text '{text}' to {elementInfo}", log);
                throw SaKiWebElementException.ErrorDuring("Type to element", elementInfo, ex);
            }
        }

        public void SelectInDropDown(WebElementInfo elementInfo, string value, ILogger log)
        {
            log = log?.CreateChildLogger($"Select option with value: {value} in: {elementInfo}");


            if (!(elementInfo is DropDownElementInfo dropDown))
                throw new Exception($"Element: {elementInfo}" +
                    $"Is not a DropDownElement");

            LogElementPath(elementInfo, log, "DropDown");

            var input = dropDown.GetInputElement()
                ?? throw new Exception($"{elementInfo} doesn't have specified Input child element");

            LogElementPath(elementInfo, log, "DropDown Input");

            var option = dropDown.GetOptionElement()
                ?? throw new Exception($"{elementInfo} doesn't have specified Option child element");
            option = option.GetCopyWithResolvedDynamicLocator(value);

            LogElementPath(elementInfo, log, "DropDown Option to select");

            var selectLog = log?.CreateChildLogger("Select option");

            var optionState = GetElementState(option, out var optionElement, selectLog);
            if (!optionState.HasFlag(WebElementState.Visible))
            {
                selectLog?.INFO("As option is not visible. Try to expand DropDown");
                Click(input, selectLog);
            }

            Click(option, selectLog);
            log?.INFO($"Option with value: {value} is selected");
        }
        public void SelectInRadioGroup(WebElementInfo elementInfo, string value, ILogger log)
        {
            log = log?.CreateChildLogger($"Select option with value: {value} in: {elementInfo}");

            if (!(elementInfo is RadioGroupElementInfo radioGroup))
                throw new Exception($"Element: {elementInfo}" +
                    $"Is not a RadioGroupElement");

            LogElementPath(elementInfo, log, "RadioGroup");

            var option = radioGroup.GetOptionElement()
                ?? throw new Exception($"{elementInfo} doesn't have specified Option child element");
            option = option.GetCopyWithResolvedDynamicLocator(value);

            LogElementPath(elementInfo, log, "RadioGroup Option to select");

            Click(option, log);

            log?.INFO($"Option with value: {value} is selected");
        }


        public object ExecuteJavaScript(WebElementInfo elementInfo, string javaScript, ILogger log)
        {
            var jsExecutor = GetJavaScriptExecutor();

            log = log?.CreateChildLogger($"Execute JavaScript on {elementInfo}");
            log?.CreateChildLogger("Logged JavaScript")?.INFO(javaScript);
            LogElementPath(elementInfo, log);

            var element = FindElement(elementInfo, log);

            return jsExecutor.ExecuteScript(javaScript, element);
        }
        public object ExecuteJavaScript(string javaScript, ILogger log)
        {
            var jsExecutor = GetJavaScriptExecutor();

            log = log?.CreateChildLogger($"Execute JavaScript");
            log?.CreateChildLogger("Logged JavaScript")?.INFO(javaScript);

            return jsExecutor.ExecuteScript(javaScript);
        }

        public byte[] GetScreenShot()
        {
            var ts = GetTakesScreenshot();
            var ss = ts.GetScreenshot();

            return ss.AsByteArray;
        }
        public void TryLogScreenShot(string message, ILogger log)
        {
            try
            {
                log?.FILE(message, GetScreenShot(), ".png", false);
            }
            catch
            {
                log?.INFO("Failed to take screenshot");
            }
        }

        private void LogElementPath(WebElementInfo elementInfo, ILogger log, string elementType = "Element")
        {
            log = log?.CreateChildLogger($"{elementType} location");
            var search = elementInfo.GetWebSearch();
            var stack = new Stack<WebSearchInfo>();
            while (search != null)
            {
                stack.Push(search);
                search = search.ParentSearch;
            }

            var isFirst = true;
            while (stack.Count != 0)
            {
                search = stack.Pop();
                log?.INFO($"{(isFirst ? "Locator" : "Followed by")}: {search.LocatorType} | {search.LocatorValue}");
                isFirst = false;
            }
        }
        private By GetBy(WebSearchInfo webSearch)
        {
            switch (webSearch.LocatorType)
            {
                case WebLocatorType.XPath:
                    return By.XPath(webSearch.LocatorValue);
                case WebLocatorType.Css:
                    return By.CssSelector(webSearch.LocatorValue);
                case WebLocatorType.Id:
                    return By.Id(webSearch.LocatorValue);
                default:
                    throw new NotImplementedException($"Unknown locator type {webSearch.LocatorType}");
            }
        }
    }
}
