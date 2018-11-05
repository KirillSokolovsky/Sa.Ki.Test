namespace Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.Controls
{
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
    using Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.Models;

    public partial class BrowserFramesUserControl : UserControl
    {
        public BrowserFrame RootFrame
        {
            get { return (BrowserFrame)GetValue(RootFrameProperty); }
            set { SetValue(RootFrameProperty, value); }
        }
        public static readonly DependencyProperty RootFrameProperty =
            DependencyProperty.Register("RootFrame", typeof(BrowserFrame), typeof(BrowserFramesUserControl), new PropertyMetadata(null));

        public BrowserFramesUserControl()
        {
            InitializeComponent();

            LayoutGrid.DataContext = this;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}
