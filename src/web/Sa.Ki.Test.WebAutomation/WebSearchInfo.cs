namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WebSearchInfo
    {
        public static WebSearchInfo BuildWebSearch(WebElementInfo elementInfo)
        {
            var locator = elementInfo.Locator;

            if (!elementInfo.Locator.IsRelative || elementInfo.Parent == null)
            {
                return new WebSearchInfo
                {
                    ParentSearch = null,
                    LocatorType = locator.LocatorType,
                    LocatorValue = locator.LocatorValue
                };
            }

            var currentSearch = new WebSearchInfo
            {
                LocatorType = locator.LocatorType,
                LocatorValue = locator.LocatorValue
            };
            var parentSearch = elementInfo.Parent.GetWebSearch();

            if (parentSearch.LocatorType == currentSearch.LocatorType
                && (currentSearch.LocatorType == WebLocatorType.Css
                 || currentSearch.LocatorType == WebLocatorType.XPath))
            {
                if (currentSearch.LocatorType == WebLocatorType.XPath)
                {
                    currentSearch.LocatorValue = MergeXPath(parentSearch.LocatorValue, currentSearch.LocatorValue);
                }
                else
                {
                    currentSearch.LocatorValue = MergeCss(parentSearch.LocatorValue, currentSearch.LocatorValue);
                }
                currentSearch.ParentSearch = parentSearch.ParentSearch;
            }
            else
            {
                currentSearch.ParentSearch = parentSearch;
            }

            return currentSearch;
        }
        public static string MergeXPath(string what, string with)
        {
            var xpath = "";

            var count = -1;
            while (with[++count] == '(') ;

            var prefix = "";
            if (count > 0)
                prefix = new string('(', count);

            if (with[count] == '.')
                count++;
            var postfix = with.Substring(count);

            xpath = prefix + what + postfix;

            return xpath;
        }
        public static string MergeCss(string what, string with)
        {
            return $"{what} {with}";
        }

        public WebSearchInfo ParentSearch { get; set; }
        public WebLocatorType LocatorType { get; set; }
        public string LocatorValue { get; set; }
    }
}
