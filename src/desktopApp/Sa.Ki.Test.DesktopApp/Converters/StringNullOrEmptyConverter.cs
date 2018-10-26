namespace Sa.Ki.Test.DesktopApp.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(bool))]
    public class StringNullOrEmptyConverter : IValueConverter
    {
        public StringNullOrEmptyConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("StringNullOrEmptyConverter value converter doesn't support back convertation");
        }
    }
}
