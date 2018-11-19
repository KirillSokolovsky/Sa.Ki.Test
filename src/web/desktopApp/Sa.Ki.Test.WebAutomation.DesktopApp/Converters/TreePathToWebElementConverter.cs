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

    [ValueConversion(typeof(string), typeof(WebElementInfoViewModel))]
    public class TreePathToWebElementConverter : DependencyObject, IValueConverter
    {
        public ObservableCollection<CombinedWebElementInfoViewModel> WebElements
        {
            get { return (ObservableCollection<CombinedWebElementInfoViewModel>)GetValue(WebElementsProperty); }
            set { SetValue(WebElementsProperty, value); }
        }
        public static readonly DependencyProperty WebElementsProperty =
            DependencyProperty.Register("WebElements", typeof(ObservableCollection<CombinedWebElementInfoViewModel>), typeof(TreePathToWebElementConverter), new PropertyMetadata(null));
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path)
            {
                var el = WebElements?.FindNodeByTreePath(path);
                return el;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is WebElementInfoViewModel el)) return null;

            return el.GetTreePath();
        }
    }
}
