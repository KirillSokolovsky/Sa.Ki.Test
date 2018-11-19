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
    public partial class WebElementCreateEditDialog : MetroWindow
    {
        public WebElementInfoViewModel WebElement
        {
            get { return (WebElementInfoViewModel)GetValue(WebElementProperty); }
            set { SetValue(WebElementProperty, value); }
        }
        public static readonly DependencyProperty WebElementProperty =
            DependencyProperty.Register("WebElement", typeof(WebElementInfoViewModel), typeof(WebElementCreateEditDialog), new PropertyMetadata(null));

        public WebElementInfoViewModel SourceWebElement
        {
            get { return (WebElementInfoViewModel)GetValue(SourceWebElementProperty); }
            set { SetValue(SourceWebElementProperty, value); }
        }
        public static readonly DependencyProperty SourceWebElementProperty =
            DependencyProperty.Register("SourceWebElement", typeof(WebElementInfoViewModel), typeof(WebElementCreateEditDialog), new PropertyMetadata(null));

        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }
        public static readonly DependencyProperty IsEditModeProperty =
            DependencyProperty.Register("IsEditMode", typeof(bool), typeof(WebElementCreateEditDialog), new PropertyMetadata(false));

        public ObservableCollection<CombinedWebElementInfoViewModel> WebElements
        {
            get { return (ObservableCollection<CombinedWebElementInfoViewModel>)GetValue(WebElementsProperty); }
            set { SetValue(WebElementsProperty, value); }
        }
        public static readonly DependencyProperty WebElementsProperty =
            DependencyProperty.Register("WebElements", typeof(ObservableCollection<CombinedWebElementInfoViewModel>), typeof(WebElementCreateEditDialog), new PropertyMetadata(null));



        private Func<WebElementInfoViewModel, string> _validate;

        public WebElementCreateEditDialog(Func<WebElementInfoViewModel, string> validate, WebElementInfoViewModel webElement)
        {
            _validate = validate;

            if (webElement == null)
                throw new ArgumentNullException(nameof(webElement));

            IsEditMode = true;
            Title = $"Edit WebElement: {webElement.Name}";
            SourceWebElement = webElement;
            WebElement = WebElementsViewModelsHelper.GetCopyOfBaseInformation(webElement);
            WebElement.Parent = webElement.Parent;

            InitializeComponent();

            DataContext = this;
        }

        public WebElementCreateEditDialog(Func<WebElementInfoViewModel, string> validate, string elementType, 
            string prefilledName = null, 
            string prefilledDescription = null,
            string prefilledInnerKey = null,
            bool prefilledLocatorIsRelative = false)
        {
            _validate = validate;

            Title = $"Create new WebElement with type: {elementType}";
            if (prefilledInnerKey != null)
                Title = $"Specify new WebElement with role {prefilledInnerKey}";

            IsEditMode = false;
            WebElement = new WebElementWithReferenceViewModel
            {
                ElementType = elementType,
                Description = prefilledDescription,
                InnerKey = prefilledInnerKey,
                Name = prefilledName,
            };
            WebElement.Locator.IsRelative = prefilledLocatorIsRelative;

            InitializeComponent();

            DataContext = this;
        }

        private void AcceptMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var validationResult = _validate?.Invoke(WebElement);

            if (!string.IsNullOrWhiteSpace(validationResult))
            {
                MessageBox.Show(validationResult, "WebElement data is not valid", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (IsEditMode)
            {
                WebElementsViewModelsHelper.FillModelWithBaseInfo(SourceWebElement, WebElement);
                WebElement = SourceWebElement;
            }

            SourceWebElement = null;
            DialogResult = true;
            Close();
        }

        private void CancelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            SourceWebElement = null;
            Close();
        }
    }
}
