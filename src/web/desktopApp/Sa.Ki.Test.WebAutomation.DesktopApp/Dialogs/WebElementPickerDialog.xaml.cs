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

        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }
        public static readonly DependencyProperty IsEditModeProperty =
            DependencyProperty.Register("IsEditMode", typeof(bool), typeof(WebElementPickerDialog), new PropertyMetadata(false));

        public string SelectedWebElementTreePath
        {
            get { return (string)GetValue(SelectedWebElementTreePathProperty); }
            set { SetValue(SelectedWebElementTreePathProperty, value); }
        }
        public static readonly DependencyProperty SelectedWebElementTreePathProperty =
            DependencyProperty.Register("SelectedWebElementTreePath", typeof(string), typeof(WebElementPickerDialog), new PropertyMetadata(null));

        public string OriginalWebElementTreePath
        {
            get { return (string)GetValue(OriginalWebElementTreePathProperty); }
            set { SetValue(OriginalWebElementTreePathProperty, value); }
        }
        public static readonly DependencyProperty OriginalWebElementTreePathProperty =
            DependencyProperty.Register("OriginalWebElementTreePath", typeof(string), typeof(WebElementPickerDialog), new PropertyMetadata(null));

        private bool _isReferenceForFrame;

        public WebElementPickerDialog(
            List<CombinedWebElementInfoViewModel> contexts,
            bool isReferenceForFrame,
            string originalWebElementTreePath = null,
            List<string> blockedElementsBreadStrings = null,
            List<string> blockedElementTypes = null)
        {
            _isReferenceForFrame = isReferenceForFrame;

            InitializeComponent();

            DataContext = this;

            Prepare(contexts, originalWebElementTreePath, blockedElementsBreadStrings, blockedElementTypes);
        }

        public void Prepare(List<CombinedWebElementInfoViewModel> contexts,
            string originalWebElementTreePath,
            List<string> blockedElementsBreadStrings,
            List<string> blockedElementTypes)
        {
            IsEditMode = originalWebElementTreePath != null;

            WebElements = new ObservableCollection<CombinedWebElementInfoViewModel>();

            foreach (var context in contexts)
            {
                var info = WebElementsViewModelsHelper.CreateInfoFromModel(context);
                var model = WebElementsViewModelsHelper.CreateModelFromInfo(info);
                var cleared = WebElementsViewModelsHelper.ClearAccrodingToBlocked(model,
                    blockedElementsBreadStrings,
                    blockedElementTypes);
                if (cleared != null)
                    WebElements.Add((CombinedWebElementInfoViewModel)cleared);
            }

            OriginalWebElementTreePath = originalWebElementTreePath;

            if (OriginalWebElementTreePath != null)
            {
                SelectedWebElementTreePath = OriginalWebElementTreePath;
            }
        }

        private void CancelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SelectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWebElementTreePath == null)
            {
                MessageBox.Show("Nothing is selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var element = (WebElementInfoViewModel)WebElements.FindNodeByTreePath(SelectedWebElementTreePath);
            if(element == null)
            {
                MessageBox.Show("Magic!!! Element not found by path.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (element.ElementType == WebElementTypes.Directory)
            {
                MessageBox.Show("Directory couldn't be selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(!_isReferenceForFrame && element.ElementType == WebElementTypes.Page)
            {
                MessageBox.Show("Page couldn't be selected to be referenced.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void SelectSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = SelectedWebElementTreePath ?? OriginalWebElementTreePath;
            SelectedWebElementTreePath = null;
            SelectedWebElementTreePath = selected;
        }
    }
}
