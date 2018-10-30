using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    public class WebContextInfoViewModel : CombinedWebElementInfoViewModel
    {
        public WebContextInfoViewModel(WebContextInfo webContextInfo = null)
            : base(webContextInfo ?? new WebContextInfo())
        {

        }
    }
}
