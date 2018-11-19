namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class WebLocatorInfoViewModel : ReactiveObject, IWebLocatorInfo
    {
        private WebLocatorInfo _sourceLocatorInfo { get; set; }

        public WebLocatorInfoViewModel(WebLocatorInfo locatorInfo = null)
        {
            this.WhenAnyValue(w => w.LocatorType)
                .Subscribe(t => Value = $"{IsRelative} | {t}: {LocatorValue}");
            this.WhenAnyValue(w => w.LocatorValue)
                .Subscribe(v => Value = $"{IsRelative} | {LocatorType}: {v}");
            this.WhenAnyValue(w => w.IsRelative)
                .Subscribe(ir => Value = $"{ir} | {LocatorType}: {LocatorValue}");

            _sourceLocatorInfo = locatorInfo ?? new WebLocatorInfo();
            LocatorType = _sourceLocatorInfo.LocatorType;
            LocatorValue = _sourceLocatorInfo.LocatorValue;
            IsRelative = _sourceLocatorInfo.IsRelative;
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

        private bool _isRelative;
        public bool IsRelative
        {
            get => _isRelative;
            set => this.RaiseAndSetIfChanged(ref _isRelative, value);
        }

        private string _value;
        public string Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }


        private static List<WebLocatorType> _locatorTypes;

        public static List<WebLocatorType> LocatorTypes
        {
            get
            {
                return _locatorTypes ??
                (
                    _locatorTypes = Enum.GetValues(typeof(WebLocatorType))
                        .Cast<WebLocatorType>()
                        .ToList()
                );
            }
        }

        public virtual WebLocatorInfo GetLocatorInfo(WebLocatorInfo locatorInfo = null)
        {
            locatorInfo = locatorInfo ?? new WebLocatorInfo();

            locatorInfo.IsRelative = IsRelative;
            locatorInfo.LocatorType = LocatorType;
            locatorInfo.LocatorValue = LocatorValue;

            return locatorInfo;
        }
    }
}
