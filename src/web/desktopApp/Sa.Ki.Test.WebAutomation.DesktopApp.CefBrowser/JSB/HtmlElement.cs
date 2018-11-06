using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.JSB
{
    public class HtmlElement
    {
        public string TagName { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlAttribute> Attributes { get; set; }
        public List<HtmlElement> Children { get; set; }
    }
}
