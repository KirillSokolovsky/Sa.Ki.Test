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

            WebSearchInfo ws = null;

            try
            {
                ws = webElement.GetWebSearch();
            }
            catch(Exception ex)
            {
                ws = new WebSearchInfo
                {
                    LocatorValue = $"Error: {ex.Message}"
                };
            }

            var wsModel = WebElementsViewModelsHelper.CreateWebSearchModelFromInfo(ws);

            var list = new List<WebSearchInfoModel>();

            var cur = wsModel;
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
    }
}
