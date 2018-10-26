using Sa.Ki.Test.DesktopApp;
using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class WebElementsTreeUserControl : UserControl
    {
        public ObservableCollection<WebContextInfoViewModel> WebContexts
        {
            get { return (ObservableCollection<WebContextInfoViewModel>)GetValue(WebContextsProperty); }
            set { SetValue(WebContextsProperty, value); }
        }
        public static readonly DependencyProperty WebContextsProperty =
            DependencyProperty.Register("WebContexts", typeof(ObservableCollection<WebContextInfoViewModel>), typeof(WebElementsTreeUserControl), new PropertyMetadata(null));

        public WebElementInfoViewModel SelectedWebElement
        {
            get { return (WebElementInfoViewModel)GetValue(SelectedWebElementProperty); }
            set { SetValue(SelectedWebElementProperty, value); }
        }
        public static readonly DependencyProperty SelectedWebElementProperty =
            DependencyProperty.Register("SelectedWebElement", typeof(WebElementInfoViewModel), typeof(WebElementsTreeUserControl), new PropertyMetadata(null, OnSelectedWebElementChanged));
        private static void OnSelectedWebElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var model = e.NewValue as WebElementInfoViewModel;
            if (model != null)
            {
                var tvi = SaKiWpfHelper.FindTreeViewItemForObject((sender as WebElementsTreeUserControl).WebElementsTreeView, model);
                if (tvi != null)
                    tvi.SetValue(TreeViewItem.IsSelectedProperty, true);
            }
            else
            {
                model = (sender as WebElementsTreeUserControl).WebElementsTreeView.SelectedItem as WebElementInfoViewModel;
                if(model != null)
                {
                    var tvi = SaKiWpfHelper.FindTreeViewItemForObject((sender as WebElementsTreeUserControl).WebElementsTreeView, model);
                    if (tvi != null)
                        tvi.SetValue(TreeViewItem.IsSelectedProperty, false);
                }
            }
        }

        public WebElementInfoViewModel CopiedWebElement
        {
            get { return (WebElementInfoViewModel)GetValue(CopiedWebElementProperty); }
            set { SetValue(CopiedWebElementProperty, value); }
        }public static readonly DependencyProperty CopiedWebElementProperty =
            DependencyProperty.Register("CopiedWebElement", typeof(WebElementInfoViewModel), typeof(WebElementsTreeUserControl), new PropertyMetadata(null));

        public WebElementInfoViewModel CutWebElement
        {
            get { return (WebElementInfoViewModel)GetValue(CutWebElementProperty); }
            set { SetValue(CutWebElementProperty, value); }
        }
        public static readonly DependencyProperty CutWebElementProperty =
            DependencyProperty.Register("CutWebElement", typeof(WebElementInfoViewModel), typeof(WebElementsTreeUserControl), new PropertyMetadata(null));



        public WebElementsTreeUserControl()
        {
            InitializeComponent();

            RootLayout.DataContext = this;
        }

        private void WebElementsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedWebElement = e.NewValue as WebElementInfoViewModel;
        }
    }
}
