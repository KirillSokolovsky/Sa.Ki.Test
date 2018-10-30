namespace Sa.Ki.Test.WebAutomation.DesktopApp.App
{
    using ReactiveUI;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
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

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

            public ObservableCollection<WebContextInfoViewModel> WebContexts { get; set; }

            private bool _isReadOnly;
            public bool IsReadOnly
            {
                get => _isReadOnly;
                set => this.RaiseAndSetIfChanged(ref _isReadOnly, value);
            }
        }

        public MainWindow()
        {
            var model = new TempModel();
            model.WebContexts = new ObservableCollection<WebContextInfoViewModel>();

            for (int i = 1; i < 5; i++)
            {
                var wc = TempDataGenerator.GenerateContext(i, 5);
                model.WebContexts.Add(new WebContextInfoViewModel(wc));
            }

            this.DataContext = model;
            
            InitializeComponent();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as TempModel;
            Tree.SelectedWebElement = dc.WebContexts[3];
        }

        private void Test_Click1(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as TempModel;
            dc.WebElement = dc.WebContexts[0];
        }
    }
}
