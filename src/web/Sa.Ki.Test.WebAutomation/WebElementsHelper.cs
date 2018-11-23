namespace Sa.Ki.Test.WebAutomation
{
    using Sa.Ki.Test.WebAutomation.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class WebElementsHelper
    {
        public static WebSearchInfo BuildWebSearch(IWebElementInfo elementInfo)
        {
            //if it's directory, return null
            if (elementInfo.ElementType == WebElementTypes.Directory) return null;

            var locator = elementInfo.Locator;
            WebSearchInfo searchInfo = null;

            //if it's element with reference. Frame or Reference
            if (elementInfo is IWebElementWithReferenceInfo elWithRef)
            {
                //if it's Reference, check locator overrides
                if (elWithRef.ElementType == WebElementTypes.Reference)
                {
                    //Reference shouldn't be null
                    if (elWithRef.ReferencedElement == null)
                        throw new SakiWebElementException($"Element with type: {elementInfo.ElementType} has nullable reference", elementInfo);

                    if (elWithRef.Locator == null)
                        locator = elWithRef.Locator;
                }
                //if it's Frame, check frame locator type
                else if (elWithRef.ElementType == WebElementTypes.Frame)
                {
                    if (locator is IFrameWebLocatorInfo frameLocator)
                    {
                        searchInfo = new FrameWebSearchInfo
                        {
                            FrameLocatorType = frameLocator.FrameLocatorType,
                            LocatorValue = frameLocator.LocatorValue,
                            LocatorType = frameLocator.LocatorType
                        };

                        //if locator is not web locator, go to parent frames calculation
                        if (frameLocator.FrameLocatorType != FrameLocatorType.Locator)
                        {
                            BuildWebSearchFrames(elementInfo, searchInfo);
                            return searchInfo;
                        }
                    }
                    else throw new SakiWebElementException($"Frame element has non frame locator.", elementInfo);
                }
                else throw new SakiWebElementException($"Unknown element with reference. Element type {elementInfo.ElementType}", elementInfo);
            }

            searchInfo = searchInfo
                ?? new WebSearchInfo
                {
                    LocatorType = locator.LocatorType,
                    LocatorValue = locator.LocatorValue,
                    ParentSearch = null
                };

            if (locator.IsRelative)
            {
                var parent = elementInfo.Parent;

                if (parent?.Parent.ElementType == WebElementTypes.Reference)
                {
                    if (parent.Parent.Locator != null)
                        parent = parent.Parent;
                }

                if (parent != null
                        && parent.ElementType != WebElementTypes.Frame
                        && parent.ElementType != WebElementTypes.Directory)
                {
                    var parentSearch = parent.GetWebSearch();

                    if (searchInfo.LocatorType == parentSearch.LocatorType
                            && (searchInfo.LocatorType == WebLocatorType.XPath || searchInfo.LocatorType == WebLocatorType.Css))
                    {
                        if (searchInfo.LocatorType == WebLocatorType.XPath)
                            searchInfo.LocatorValue = MergeXPath(parentSearch.LocatorValue, searchInfo.LocatorValue);
                        else
                            searchInfo.LocatorValue = MergeCss(parentSearch.LocatorValue, searchInfo.LocatorValue);

                        searchInfo.ParentSearch = parentSearch.ParentSearch;
                    }
                    else
                    {
                        if (parentSearch.LocatorType == WebLocatorType.XPath
                            && parentSearch.LocatorValue == ".")
                            searchInfo.ParentSearch = parentSearch.ParentSearch;
                        else
                            searchInfo.ParentSearch = parentSearch;
                    }

                    if (searchInfo.LocatorType == WebLocatorType.XPath
                        && searchInfo.LocatorValue == ".")
                        return parentSearch;

                    return searchInfo;
                }
            }

            BuildWebSearchFrames(elementInfo, searchInfo);
            return searchInfo;
        }

        public static void BuildWebSearchFrames(IWebElementInfo elementInfo, WebSearchInfo webSearchInfo)
        {
            var parent = elementInfo.Parent;
            while (parent != null && parent.ElementType != WebElementTypes.Directory)
            {
                if (parent.ElementType == WebElementTypes.Frame)
                {
                    webSearchInfo.ParentSearch = BuildWebSearch(parent);
                    break;
                }
                parent = parent.Parent;
            }
        }

        private static WebSearchInfo CreateWebSearchFor(IWebElementInfo elementInfo)
        {
            var locator = elementInfo.Locator;
            WebSearchInfo webSearchInfo = null;

            if (elementInfo.ElementType == WebElementTypes.Frame)
            {
                if (locator is IFrameWebLocatorInfo fwli)
                {
                    webSearchInfo = new FrameWebSearchInfo
                    {
                        FrameLocatorType = fwli.FrameLocatorType
                    };
                }
                else
                    throw new Exception("Wow frame element has non frame locator info");
            }
            else webSearchInfo = new WebSearchInfo();

            webSearchInfo.LocatorType = locator.LocatorType;
            webSearchInfo.LocatorValue = locator.LocatorValue;

            return webSearchInfo;
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
    }
}
