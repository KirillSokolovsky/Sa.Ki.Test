namespace Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.JSB
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BrowserFrameSyncBoundObject
    {
        public long FrameIdentifier { get; set; }
        public string BindName { get; set; }

        public BrowserFrameSyncBoundObject(long frameIdentifier)
        {
            FrameIdentifier = frameIdentifier;
            BindName = $"FrameSyncObject_{FrameIdentifier}";
        }

        public string Test()
        {
            Trace.WriteLine($"=== Called: {BindName}");
            return BindName;
        }
    }
}
