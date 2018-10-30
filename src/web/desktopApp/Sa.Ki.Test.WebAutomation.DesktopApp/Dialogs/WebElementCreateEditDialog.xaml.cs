﻿using MahApps.Metro.Controls;
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

        private Func<WebElementInfoViewModel, string> _validate;

        public WebElementCreateEditDialog(Func<WebElementInfoViewModel, string> validate, WebElementInfoViewModel webElement)
        {
            _validate = validate;

            if (webElement == null)
                throw new ArgumentNullException(nameof(webElement));

            IsEditMode = true;
            Title = $"Edit WebElement: {webElement.Name}";
            SourceWebElement = webElement;
            WebElement = WebElementsViewModelsFactory.GetCopyOfBaseInformation(SourceWebElement);

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
            WebElement = new WebElementInfoViewModel
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
                WebElementsViewModelsFactory.FillModelWithBaseInfo(SourceWebElement, WebElement);
                WebElement = SourceWebElement;
            }

            DialogResult = true;
            Close();
        }

        private void CancelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}