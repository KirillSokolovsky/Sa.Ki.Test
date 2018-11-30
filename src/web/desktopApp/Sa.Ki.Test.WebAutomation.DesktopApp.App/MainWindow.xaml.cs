namespace Sa.Ki.Test.WebAutomation.DesktopApp.App
{
    using MahApps.Metro.Controls;
    using ReactiveUI;
    using Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.Models;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Dialogs;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using Sa.Ki.Test.WebAutomation.ElementsRepository;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
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

    public partial class MainWindow : MetroWindow
    {
        public class TempModel : ReactiveObject
        {
            public TempModel()
            {
                //this.WhenAnyValue(p => p.WebElement)
                //    .Subscribe(v => Trace.WriteLine($"Updated. {v?.Name ?? "NULL"}"));
            }

            public WebLocatorInfoViewModel Locator { get; set; }
            public ObservableCollection<string> Strings { get; set; }
            public ObservableCollection<string> AvailableStrings { get; set; }

            private WebElementInfoViewModel _webElement;
            public WebElementInfoViewModel WebElement
            {
                get => _webElement;
                set => this.RaiseAndSetIfChanged(ref _webElement, value);
            }

            public ObservableCollection<CombinedWebElementInfoViewModel> WebElements { get; set; }

            private bool _isReadOnly;
            public bool IsReadOnly
            {
                get => _isReadOnly;
                set => this.RaiseAndSetIfChanged(ref _isReadOnly, value);
            }

            private BrowserFrame _rootFrame;
            public BrowserFrame RootFrame
            {
                get => _rootFrame;
                set => this.RaiseAndSetIfChanged(ref _rootFrame, value);
            }
        }

        private WebElementsRepository _webElementsRepository;

        public MainWindow()
        {
            _webElementsRepository = new WebElementsRepository("elements.json");
            _webElementsRepository.Load();

            var model = new TempModel();
            model.WebElements = new ObservableCollection<CombinedWebElementInfoViewModel>();

            foreach (var wc in _webElementsRepository.WebElements)
            {
                var cmbModel = (CombinedWebElementInfoViewModel)WebElementsViewModelsHelper.CreateModelFromInfo(wc);
                model.WebElements.Add(cmbModel);
            }

            //for (int i = 1; i < 5; i++)
            //{
            //    var wc = TempDataGenerator.GenerateContext(i, 5);
            //    model.WebElements.Add(new WebContextInfoViewModel(wc));
            //}

            this.DataContext = model;

            InitializeComponent();
        }

        private void SaveWebElementsButton_Click(object sender, RoutedEventArgs e)
        {
            _webElementsRepository.WebElements = (DataContext as TempModel)
                .WebElements.Select(wc => WebElementsViewModelsHelper.CreateInfoFromModel(wc))
                .Cast<CombinedWebElementInfo>().ToList();

            _webElementsRepository.Save();
        }
        
        private void SelectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as TempModel;

            var d = new WebElementPickerDialog(dc.WebElements.ToList(),
                false,
                null,
                null,
                new List<string> { WebElementTypes.DropDown });
            d.ShowDialog();
        }
    }
}
