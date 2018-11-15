namespace Sa.Ki.Test.WebAutomation.DesktopApp.Dialogs
{
    using MahApps.Metro.Controls;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using Sa.Ki.Test.SakiTree;
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

    public partial class WebElementPickerDialog : MetroWindow
    {
        public ObservableCollection<CombinedWebElementInfoViewModel> WebElements
        {
            get { return (ObservableCollection<CombinedWebElementInfoViewModel>)GetValue(WebElementsProperty); }
            set { SetValue(WebElementsProperty, value); }
        }
        public static readonly DependencyProperty WebElementsProperty =
            DependencyProperty.Register("WebElements", typeof(ObservableCollection<CombinedWebElementInfoViewModel>), typeof(WebElementPickerDialog), new PropertyMetadata(null));

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

        public WebElementPickerDialog(
            List<CombinedWebElementInfoViewModel> contexts,
            WebElementInfoViewModel originalWebElement = null,
            List<string> blockedElementsBreadStrings = null,
            List<string> blockedElementTypes = null)
        {
            InitializeComponent();

            DataContext = this;

            Prepare(contexts, originalWebElement, blockedElementsBreadStrings, blockedElementTypes);
        }

        public void Prepare(List<CombinedWebElementInfoViewModel> contexts,
            WebElementInfoViewModel originalWebElement,
            List<string> blockedElementsBreadStrings,
            List<string> blockedElementTypes)
        {
            IsEditMode = originalWebElement != null;

            WebElements = new ObservableCollection<CombinedWebElementInfoViewModel>();
            foreach (var context in contexts)
            {
                var info = WebElementsViewModelsHelper.CreateInfoFromModel(context);
                var model = WebElementsViewModelsHelper.CreateModelFromInfo(info);
                var cleared = WebElementsViewModelsHelper.ClearAccrodingToBlocked(model,
                    blockedElementsBreadStrings,
                    blockedElementTypes);
                if (cleared != null)
                    WebElements.Add(cleared as CombinedWebElementInfoViewModel);
            }

            OriginalWebElement = originalWebElement;

            if (OriginalWebElement != null)
            {
                //TreeControl.WebElementsTreeView.UpdateLayout();
                SelectedWebElement = (WebElementInfoViewModel)WebElements.FindNodeByTreePath(OriginalWebElement.GetTreePath());
            }
        }

        private void CancelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SelectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWebElement == null)
            {
                MessageBox.Show("Nothing is selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedWebElement.ElementType == WebElementTypes.Directory)
            {
                MessageBox.Show("Directory couldn't be selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }
    }
}
