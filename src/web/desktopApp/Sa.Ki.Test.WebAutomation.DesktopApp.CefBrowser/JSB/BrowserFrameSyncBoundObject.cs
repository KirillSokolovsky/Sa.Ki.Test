namespace Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.JSB
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BrowserFrameSyncBoundObject
    {
        public static string CreateName(long identifier)
        {
            return $"FrameSyncObject_{identifier}";
        }

        public long FrameIdentifier { get; set; }
        public string BindName { get; set; }

        public BrowserFrameSyncBoundObject(long frameIdentifier)
        {
            FrameIdentifier = frameIdentifier;
            BindName = CreateName(FrameIdentifier);
        }

        public string Test()
        {
            Trace.WriteLine($"=== Called: {BindName}");
            return BindName;
        }

        public void HandleEvent(string eventType, string targetElementJson)
        {
            Trace.WriteLine($"=== Handled: {eventType}");

            var element = JsonConvert.DeserializeObject<HtmlElement>(targetElementJson);
        }
    }
}
