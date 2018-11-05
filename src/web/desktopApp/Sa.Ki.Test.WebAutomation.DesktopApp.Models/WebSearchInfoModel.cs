namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class WebSearchInfoModel : ReactiveObject
    {
        public WebSearchInfoModel()
        {
        }

        private WebLocatorType _locatorType;
        public WebLocatorType LocatorType
        {
            get => _locatorType;
            set => this.RaiseAndSetIfChanged(ref _locatorType, value);
        }

        private string _locatorValue;
        public string LocatorValue
        {
            get => _locatorValue;
            set => this.RaiseAndSetIfChanged(ref _locatorValue, value);
        }

        public WebSearchInfoModel ParentSearch { get; set; }
    }
}
