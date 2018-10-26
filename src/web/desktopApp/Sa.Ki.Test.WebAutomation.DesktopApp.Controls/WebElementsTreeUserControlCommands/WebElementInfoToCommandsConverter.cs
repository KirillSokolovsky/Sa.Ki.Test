namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using ReactiveUI;
    using Sa.Ki.Test.DesktopApp.Models.SaKiMenu;
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

    public class WebElementInfoToCommandsConverter : DependencyObject, IValueConverter
    {
        public WebElementsTreeUserControl TreeControl
        {
            get { return (WebElementsTreeUserControl)GetValue(TreeControlProperty); }
            set { SetValue(TreeControlProperty, value); }
        }
        public static readonly DependencyProperty TreeControlProperty =
            DependencyProperty.Register("TreeControl", typeof(WebElementsTreeUserControl), typeof(WebElementInfoToCommandsConverter), new PropertyMetadata(null));
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = new List<SaKiMenuItemViewModel>();

            var group = new SaKiGroupMenuItemViewModel
            {
                Name = "Create",
                Description = "Create new WebElemenInfo with specified type",
                Items = new ObservableCollection<SaKiMenuItemViewModel>
                {
                    new SaKiCommandMenuItemViewModel
                    {
                        Name = "Element",
                        Description = "New WebElementInfo",
                        Command = new CopyNameCommand(TreeControl)
                    },
                    new SaKiCommandMenuItemViewModel
                    {
                        Name="Control",
                        Description = "New CombinedWebElement",
                        Command = new CopyNameCommand(TreeControl)
                    }
                }
            };

            list.Add(new SaKiCommandMenuItemViewModel
            {
                Name = "Copy Name",
                Description = "Copy name of WebElementInfo",
                Command = new CopyNameCommand(TreeControl)
            });

            list.Add(group);

            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("WebElementInfoToCommandsConverter doesn't support back convertation");
        }
    }
}
