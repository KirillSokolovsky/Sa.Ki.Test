namespace Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.Models
{
    using CefSharp;
    using ReactiveUI;
    using Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.JSB;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BrowserFrame : ReactiveObject
    {
        public BrowserFrameSyncBoundObject SyncObject { get; set; }
        public BrowserFrame Parent { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _loadedUrl;
        public string LoadedUrl
        {
            get => _loadedUrl;
            set => this.RaiseAndSetIfChanged(ref _loadedUrl, value);
        }

        private long _identifier;
        public long Identifier
        {
            get => _identifier;
            set => this.RaiseAndSetIfChanged(ref _identifier, value);
        }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            set => this.RaiseAndSetIfChanged(ref _isLoaded, value);
        }

        private ObservableCollection<BrowserFrame> _frames;
        public ObservableCollection<BrowserFrame> Frames
        {
            get => _frames;
            set => this.RaiseAndSetIfChanged(ref _frames, value);
        }

        public IEnumerable<BrowserFrame> IterateSelfAndChildren()
        {
            yield return this;
            if(Frames != null)
            {
                foreach (var frame in Frames)
                {
                    foreach (var f in frame.IterateSelfAndChildren())
                    {
                        yield return f;
                    }
                }
            }
        }
    }
}
