using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls
{
    /// <summary>
    /// Interaction logic for WebLocatorUserControl.xaml
    /// </summary>
    public partial class WebLocatorInfoUserControl : UserControl
    {
        public WebLocatorInfoViewModel WebLocator
        {
            get { return (WebLocatorInfoViewModel)GetValue(WebLocatorProperty); }
            set { SetValue(WebLocatorProperty, value); }
        }
        public static readonly DependencyProperty WebLocatorProperty =
            DependencyProperty.Register("WebLocator", typeof(WebLocatorInfoViewModel), typeof(WebLocatorInfoUserControl),
                new PropertyMetadata(null));
               
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(WebLocatorInfoUserControl), new PropertyMetadata(false));
               
        public WebLocatorInfoUserControl()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }
    }
}
