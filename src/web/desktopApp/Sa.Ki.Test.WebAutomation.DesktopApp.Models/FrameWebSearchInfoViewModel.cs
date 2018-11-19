using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    public class FrameWebSearchInfoViewModel : WebSearchInfoModel
    {
        public FrameWebSearchInfoViewModel(FrameWebSearchInfo frameWebSearchInfo = null)
            : base(frameWebSearchInfo)
        {
            if (frameWebSearchInfo != null)
                FrameLocatorType = frameWebSearchInfo.FrameLocatorType;
        }


        private FrameLocatorType _frameLocatorType;
        public FrameLocatorType FrameLocatorType
        {
            get => _frameLocatorType;
            set => this.RaiseAndSetIfChanged(ref _frameLocatorType, value);
        }
    }
}
