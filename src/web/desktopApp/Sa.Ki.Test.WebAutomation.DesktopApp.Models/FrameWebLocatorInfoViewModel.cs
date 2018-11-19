namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FrameWebLocatorInfoViewModel : WebLocatorInfoViewModel, IFrameWebLocatorInfo
    {
        public FrameWebLocatorInfoViewModel(WebLocatorInfo locatorInfo = null)
            : base(locatorInfo ?? new FrameWebLocatorInfo())
        {
        }

        private FrameLocatorType _frameLocatorType;
        public FrameLocatorType FrameLocatorType
        {
            get => _frameLocatorType;
            set => this.RaiseAndSetIfChanged(ref _frameLocatorType, value);
        }

        private static List<FrameLocatorType> _frameLocatorTypes;

        public static List<FrameLocatorType> FrameLocatorTypes
        {
            get
            {
                return _frameLocatorTypes ??
                (
                    _frameLocatorTypes = Enum.GetValues(typeof(FrameLocatorType))
                        .Cast<FrameLocatorType>()
                        .ToList()
                );
            }
        }

        public override WebLocatorInfo GetLocatorInfo(WebLocatorInfo locatorInfo = null)
        {
            var fwli = locatorInfo as FrameWebLocatorInfo ?? new FrameWebLocatorInfo();

            fwli.FrameLocatorType = FrameLocatorType;

            return base.GetLocatorInfo(fwli);
        }
    }
}
