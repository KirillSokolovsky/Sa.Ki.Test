namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls
{
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
    using Sa.Ki.Test.WebAutomation.DesktopApp.Dialogs;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using Sa.Ki.Test.SakiTree;

    public partial class WebElementInfoUserControl : UserControl
    {
        public WebElementInfoViewModel WebElement
        {
            get { return (WebElementInfoViewModel)GetValue(WebElementProperty); }
            set { SetValue(WebElementProperty, value); }
        }
        public static readonly DependencyProperty WebElementProperty =
            DependencyProperty.Register("WebElement", typeof(WebElementInfoViewModel), typeof(WebElementInfoUserControl), new PropertyMetadata(null));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(WebElementInfoUserControl), new PropertyMetadata(false));

        public ObservableCollection<CombinedWebElementInfoViewModel> WebElements
        {
            get { return (ObservableCollection<CombinedWebElementInfoViewModel>)GetValue(WebElementsProperty); }
            set { SetValue(WebElementsProperty, value); }
        }
        public static readonly DependencyProperty WebElementsProperty =
            DependencyProperty.Register("WebElements", typeof(ObservableCollection<CombinedWebElementInfoViewModel>), typeof(WebElementInfoUserControl), new PropertyMetadata(null));

        public WebElementInfoUserControl()
        {
            InitializeComponent();

            LayoutGrid.DataContext = this;
        }

        private void EditReferencedElementButton_Click(object sender, RoutedEventArgs e)
        {
            if (WebElements == null
                || !(WebElement is WebElementWithReferenceViewModel rm))
            {
                MessageBox.Show("Edit operaion is not supported. WebElements source is not specified or element doesn't support references");
                return;
            }

            var picker = new WebElementPickerDialog(WebElements.ToList(),
                rm.ReferenceBreadString,
                new List<string> { WebElement.GetTreePath() },
                WebElementsViewModelsHelper.GetBlockedElementTypesForElementType(rm.ElementType)
                );

            if (picker.ShowDialog() != true) return;

            if (rm.ReferenceBreadString == picker.SelectedWebElementTreePath) return;

            rm.ReferenceBreadString = picker.SelectedWebElementTreePath;

            var r = (WebElementInfoViewModel)WebElements.FindNodeByTreePath(rm.ReferenceBreadString);
            var copy = WebElementsViewModelsHelper.CreateFullModelCopy(r);

            if (rm.ElementType == WebElementTypes.Reference)
            {
                var baseInfo = new WebElementInfoViewModel
                {
                    Name = copy.Name,
                    Description = copy.Description,
                    Tags = copy.Tags,
                    ElementType = null,
                    Locator = null,
                    InnerKey = null,
                };

                WebElementsViewModelsHelper.FillModelWithBaseInfo(rm, baseInfo, true);
            }

            rm.Elements?.Clear();
            if (rm.Elements == null)
                rm.Elements = new ObservableCollection<WebElementInfoViewModel>();

            if(rm.ElementType == WebElementTypes.Reference)
            {
                if(copy is CombinedWebElementInfoViewModel combinedRef)
                {
                    if(combinedRef.Elements != null)
                    {
                        foreach (var c in combinedRef.Elements)
                        {
                            rm.Elements.Add(c);
                            c.Parent = rm;
                        }
                    }
                }
            }
            else
            {
                if (rm.ReferencedWebElement != null)
                {
                    rm.ReferencedWebElement.Parent = null;
                    rm.ReferencedWebElement = null;
                }

                rm.Elements.Add(copy);
            }

            rm.ReferencedWebElement = copy;
            copy.Parent = rm;
            rm.HasLocator = false;

            WebElement = WebElementsViewModelsHelper.CreateFullModelCopy(rm);
            UpdateLayout();
            WebElement = rm;
        }
    }
}
