using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    public class WebPageInfoViewModel : CombinedWebElementInfoViewModel
    {
        public WebPageInfoViewModel(WebPageInfo webPageInfo = null)
            : base(webPageInfo ?? new WebPageInfo())
        {
            Locator = null;

            if (webPageInfo != null)
            {
                UrlRegexString = webPageInfo.UrlRegexString;
                DefaultUrl = webPageInfo.DefaultUrl;
            }
        }

        private string _defaultUrl;
        public string DefaultUrl
        {
            get => _defaultUrl;
            set => this.RaiseAndSetIfChanged(ref _defaultUrl, value);
        }

        private string _urlRegexString;
        public string UrlRegexString
        {
            get => _urlRegexString;
            set => this.RaiseAndSetIfChanged(ref _urlRegexString, value);
        }
    }
}
