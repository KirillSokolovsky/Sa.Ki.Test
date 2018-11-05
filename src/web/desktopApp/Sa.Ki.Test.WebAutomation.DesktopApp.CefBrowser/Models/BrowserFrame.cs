namespace Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.Models
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BrowserFrame : ReactiveObject
    {
        public string Key { get; set; }

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
    }
}
