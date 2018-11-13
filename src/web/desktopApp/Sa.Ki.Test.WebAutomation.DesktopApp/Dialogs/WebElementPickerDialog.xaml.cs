using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace Sa.Ki.Test.WebAutomation.DesktopApp.Dialogs
{
    public partial class WebElementPickerDialog : MetroWindow
    {
        public ObservableCollection<CombinedWebElementInfoViewModel> WebContexts
        {
            get { return (ObservableCollection<CombinedWebElementInfoViewModel>)GetValue(WebContextsProperty); }
            set { SetValue(WebContextsProperty, value); }
        }
        public static readonly DependencyProperty WebContextsProperty =
            DependencyProperty.Register("WebContexts", typeof(ObservableCollection<CombinedWebElementInfoViewModel>), typeof(WebElementPickerDialog), new PropertyMetadata(null));

        public WebElementInfoViewModel SelectedWebElement
        {
            get { return (WebElementInfoViewModel)GetValue(SelectedWebElementProperty); }
            set { SetValue(SelectedWebElementProperty, value); }
        }
        public static readonly DependencyProperty SelectedWebElementProperty =
            DependencyProperty.Register("SelectedWebElement", typeof(WebElementInfoViewModel), typeof(WebElementPickerDialog), new PropertyMetadata(null));

        public WebElementInfoViewModel OriginalWebElement
        {
            get { return (WebElementInfoViewModel)GetValue(OriginalWebElementProperty); }
            set { SetValue(OriginalWebElementProperty, value); }
        }
        public static readonly DependencyProperty OriginalWebElementProperty =
            DependencyProperty.Register("OriginalWebElement", typeof(WebElementInfoViewModel), typeof(WebElementPickerDialog), new PropertyMetadata(null));

        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }
        public static readonly DependencyProperty IsEditModeProperty =
            DependencyProperty.Register("IsEditMode", typeof(bool), typeof(WebElementPickerDialog), new PropertyMetadata(false));

        public WebElementPickerDialog(List<CombinedWebElementInfoViewModel> contexts, WebElementInfoViewModel originalWebElement = null)
        {
            Prepare(contexts, originalWebElement);

            InitializeComponent();

            DataContext = this;
        }

        public void Prepare(List<CombinedWebElementInfoViewModel> contexts, WebElementInfoViewModel originalWebElement)
        {
            IsEditMode = originalWebElement != null;

            WebContexts = new ObservableCollection<CombinedWebElementInfoViewModel>();
            foreach (var context in contexts)
            {
                var info = WebElementsViewModelsFactory.CreateInfoFromModel(context);
                var model = WebElementsViewModelsFactory.CreateModelFromInfo(info);
                WebContexts.Add(model as CombinedWebElementInfoViewModel);
            }

            OriginalWebElement = originalWebElement;

            if (OriginalWebElement != null)
            {
                SelectedWebElement = WebContexts.FindByBreadString(OriginalWebElement.ToBreadString());
            }
        }

        private void CancelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SelecMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
