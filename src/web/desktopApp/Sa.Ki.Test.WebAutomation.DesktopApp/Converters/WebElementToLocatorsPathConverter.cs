namespace Sa.Ki.Test.WebAutomation.DesktopApp.Converters
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Data;

    [ValueConversion(typeof(WebElementInfoViewModel), typeof(List<WebSearchInfoModel>))]
    public class WebElementToLocatorsPathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2) return null;
            if (!(values[1] is WebElementInfoViewModel webElement))
                return null;

            var ws = BuildWebSearch(webElement);

            var list = new List<WebSearchInfoModel>();

            var cur = ws;
            while (cur != null)
            {
                list.Add(cur);
                cur = cur.ParentSearch;
            }

            list.Reverse();

            return list;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("WebElementToLocatorsPathConverter doesn't support back convertation");
        }


        public static WebSearchInfoModel BuildWebSearch(WebElementInfoViewModel elementInfo)
        {
            var locator = elementInfo.Locator;

            if (!elementInfo.Locator.IsRelative || elementInfo.Parent == null)
            {
                return new WebSearchInfoModel
                {
                    ParentSearch = null,
                    LocatorType = locator.LocatorType,
                    LocatorValue = locator.LocatorValue
                };
            }

            var currentSearch = new WebSearchInfoModel
            {
                LocatorType = locator.LocatorType,
                LocatorValue = locator.LocatorValue
            };
            var parentSearch = BuildWebSearch(elementInfo.Parent);

            if (parentSearch.LocatorType == currentSearch.LocatorType
                && (currentSearch.LocatorType == WebLocatorType.Css
                 || currentSearch.LocatorType == WebLocatorType.XPath))
            {
                if (currentSearch.LocatorType == WebLocatorType.XPath)
                {
                    currentSearch.LocatorValue = WebSearchInfo.MergeXPath(parentSearch.LocatorValue, currentSearch.LocatorValue);
                }
                else
                {
                    currentSearch.LocatorValue = WebSearchInfo.MergeCss(parentSearch.LocatorValue, currentSearch.LocatorValue);
                }
                currentSearch.ParentSearch = parentSearch.ParentSearch;
            }
            else
            {
                currentSearch.ParentSearch = parentSearch;
            }

            return currentSearch;
        }
    }
}
