namespace Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.JSB
{
    using CefSharp;
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

        public void Highlight(string xpath, IFrame frame)
        {
            frame.ExecuteJavaScriptAsync($"highlightByXPath(\"{xpath}\")");
        }
        public void ClearHighlight(IFrame frame)
        {
            frame.ExecuteJavaScriptAsync($"clearHighlight()");
        }
    }
}
