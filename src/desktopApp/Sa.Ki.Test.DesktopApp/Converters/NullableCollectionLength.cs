namespace Sa.Ki.Test.DesktopApp.Converters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Data;

    [ValueConversion(typeof(ICollection), typeof(int))]
    public class NullableCollectionLength : IValueConverter
    {
        public NullableCollectionLength()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection collection)
            {
                return collection?.Count ?? 0;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("NullableCollectionLength value converter doesn't support back convertation");
        }
    }
}
