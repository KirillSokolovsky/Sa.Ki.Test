namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using ReactiveUI;
    using Sa.Ki.Test.DesktopApp.Models.SaKiMenu;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class WebElementInfoViewModel : ReactiveObject
    {
        protected WebElementInfo _sourceWebElement { get; set; }

        public WebElementInfoViewModel(WebElementInfo webElementInfo = null)
        {
            _sourceWebElement = webElementInfo ?? new WebElementInfo();

            Name = _sourceWebElement.Name;
            Description = _sourceWebElement.Description;
            ElementType = _sourceWebElement.ElementType;
            InnerKey = _sourceWebElement.InnerKey;
            IsKey = _sourceWebElement.IsKey;

            if (_sourceWebElement.Tags != null)
                Tags = new ObservableCollection<string>(_sourceWebElement.Tags);
            Locator = new WebLocatorInfoViewModel(_sourceWebElement.Locator);
        }

        private CombinedWebElementInfoViewModel _parent;
        public CombinedWebElementInfoViewModel Parent
        {
            get => _parent;
            set => this.RaiseAndSetIfChanged(ref _parent, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        private string _elementType;
        public string ElementType
        {
            get => _elementType;
            set => this.RaiseAndSetIfChanged(ref _elementType, value);
        }

        private string _innerKey;
        public string InnerKey
        {
            get => _innerKey;
            set => this.RaiseAndSetIfChanged(ref _innerKey, value);
        }

        private bool _isKey;
        public bool IsKey
        {
            get => _isKey;
            set => this.RaiseAndSetIfChanged(ref _isKey, value);
        }

        private ObservableCollection<string> _tags;
        public ObservableCollection<string> Tags
        {
            get => _tags;
            set => this.RaiseAndSetIfChanged(ref _tags, value);
        }

        private WebLocatorInfoViewModel _locator;
        public WebLocatorInfoViewModel Locator
        {
            get => _locator;
            set => this.RaiseAndSetIfChanged(ref _locator, value);
        }

        public override string ToString()
        {
            return $"{ElementType} | {Name}";
        }

        public bool IsDescendantdOf(CombinedWebElementInfoViewModel combinedWebElementInfo)
        {
            if (combinedWebElementInfo == null) return false;
            if (combinedWebElementInfo == Parent) return true;

            return IsDescendantdOf(combinedWebElementInfo.Parent);
        }
    }
}
