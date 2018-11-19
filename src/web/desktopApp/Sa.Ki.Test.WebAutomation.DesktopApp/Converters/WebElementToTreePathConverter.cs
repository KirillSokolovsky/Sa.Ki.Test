namespace Sa.Ki.Test.WebAutomation.DesktopApp.Converters
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using Sa.Ki.Test.SakiTree;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using System.Windows;
    using System.Collections.ObjectModel;

    [ValueConversion(typeof(WebElementInfoViewModel), typeof(string))]
    public class WebElementToTreePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is WebElementInfoViewModel el)) return null;

            return el.GetTreePath();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("WebElementToBreadStringConverter doesn't support back convertation");
        }
    }
}
