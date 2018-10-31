namespace Sa.Ki.Test.WebAutomation.DesktopApp.Converters
{
    using ReactiveUI;
    using Sa.Ki.Test.DesktopApp.Models.SaKiMenu;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Controls;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(WebElementInfoViewModel), typeof(List<SaKiMenuItemViewModel>))]
    public class WebElementToCommandsConverter : DependencyObject, IValueConverter
    {
        private WebElementMenuItemsFactory _webElementMenuItemsFactory;

        public WebElementsTreeUserControl TreeControl
        {
            get { return (WebElementsTreeUserControl)GetValue(TreeControlProperty); }
            set { SetValue(TreeControlProperty, value); }
        }
        public static readonly DependencyProperty TreeControlProperty =
            DependencyProperty.Register("TreeControl", typeof(WebElementsTreeUserControl), typeof(WebElementToCommandsConverter), new PropertyMetadata(null));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (TreeControl == null) return null;

            if (_webElementMenuItemsFactory == null)
                _webElementMenuItemsFactory = new WebElementMenuItemsFactory(TreeControl);

            return _webElementMenuItemsFactory.CreateMenuItemsForWebElement();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("WebElementToCommandsConverter doesn't support back convertation");
        }
    }
}
