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
    public partial class WebElementLocatorsPathUserControl : UserControl
    {
        public WebElementInfoViewModel WebElement
        {
            get { return (WebElementInfoViewModel)GetValue(WebElementProperty); }
            set { SetValue(WebElementProperty, value); }
        }
        public static readonly DependencyProperty WebElementProperty =
            DependencyProperty.Register("WebElement", typeof(WebElementInfoViewModel), typeof(WebElementLocatorsPathUserControl), new PropertyMetadata(null));

        public WebElementLocatorsPathUserControl()
        {
            InitializeComponent();

            LayoutGrid.DataContext = this;
        }
    }
}
