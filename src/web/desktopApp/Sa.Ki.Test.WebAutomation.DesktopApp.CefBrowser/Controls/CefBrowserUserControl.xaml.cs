namespace Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.Controls
{
    using CefSharp;
    using Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.JSB;
    using Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.Models;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    public partial class CefBrowserUserControl : UserControl
    {
        public string CurrentUrl
        {
            get { return (string)GetValue(CurrentUrlProperty); }
            set { SetValue(CurrentUrlProperty, value); }
        }
        public static readonly DependencyProperty CurrentUrlProperty =
            DependencyProperty.Register("CurrentUrl", typeof(string), typeof(CefBrowserUserControl), new PropertyMetadata(null));

        public BrowserFrame RootFrame
        {
            get { return (BrowserFrame)GetValue(RootFrameProperty); }
            set { SetValue(RootFrameProperty, value); }
        }
        public static readonly DependencyProperty RootFrameProperty =
            DependencyProperty.Register("RootFrame", typeof(BrowserFrame), typeof(CefBrowserUserControl), new PropertyMetadata(null));

        public CefBrowserUserControl()
        {
            InitializeComponent();
            LayoutGrid.DataContext = this;

            CurrentUrl = "https://yandex.ru";

            CBrowser.FrameLoadStart += CBrowser_FrameLoadStart;
            CBrowser.FrameLoadEnd += CBrowser_FrameLoadEnd;

            CBrowser.JavascriptObjectRepository.ResolveObject += JavascriptObjectRepository_ResolveObject;

            //CBrowser.JavascriptObjectRepository.Register("boundAsync", _abo, true);
        }

        private void JavascriptObjectRepository_ResolveObject(object sender, CefSharp.Event.JavascriptBindingEventArgs e)
        {
            Trace.WriteLine($"=== Resolve request for: {e.ObjectName}");
        }

        private void CBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var frame = e.Frame;
            var fr = _framesDict[frame.Identifier];
            var syncFr = _syncFrameObjsDict[frame.Identifier];

            if (fr.IsLoaded)
            {
                Trace.WriteLine($"=== Reloaded: {syncFr.BindName}");
                return;
            }
            fr.IsLoaded = true;

            Trace.WriteLine($"=== Inject: {syncFr.BindName}");

            var jsLines = File.ReadAllLines(@"C:\Dev\JSTest\frameSync.js");
            jsLines[0] = $"var frameSyncObjectName = \"{syncFr.BindName}\";";
            var js = string.Join(Environment.NewLine, jsLines);
            frame.ExecuteJavaScriptAsync(js);
        }

        private Dictionary<long, BrowserFrame> _framesDict = new Dictionary<long, BrowserFrame>();
        private Dictionary<long, BrowserFrameSyncBoundObject> _syncFrameObjsDict = new Dictionary<long, BrowserFrameSyncBoundObject>();
        private void CBrowser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var frame = e.Frame;

                if(frame.IsMain)
                {
                    CBrowser.JavascriptObjectRepository.UnRegisterAll();
                    _syncFrameObjsDict.Clear();
                    Trace.WriteLine("=== Clear sync objs");
                }

                var syncObj = new BrowserFrameSyncBoundObject(frame.Identifier);
                Trace.WriteLine($"=== Register: {syncObj.BindName}");

                if (CBrowser.JavascriptObjectRepository.IsBound(syncObj.BindName))
                {
                    CBrowser.JavascriptObjectRepository.UnRegister(syncObj.BindName);
                    _syncFrameObjsDict.Remove(syncObj.FrameIdentifier);
                    Trace.WriteLine($"=== RERegister: {syncObj.BindName}");
                }

                CBrowser.JavascriptObjectRepository.Register(syncObj.BindName, syncObj, true);
                _syncFrameObjsDict.Add(syncObj.FrameIdentifier, syncObj);

                if (frame.IsMain)
                {
                    var guid = Guid.NewGuid();
                    //Trace.WriteLine($"ROOT UPDATED: {guid}");
                    RootFrame = new BrowserFrame
                    {
                        Frames = new ObservableCollection<BrowserFrame>(),
                        Identifier = frame.Identifier,
                        Name = $"{GetFrameName(frame)}",
                        LoadedUrl = frame.Url
                    };
                    _framesDict.Clear();
                    _framesDict.Add(frame.Identifier, RootFrame);
                }
                else
                {
                    var stack = new Stack<IFrame>();

                    for (var f = frame; f != null; f = f.Parent)
                        stack.Push(f);

                    while (stack.Count > 0)
                    {
                        var f = stack.Pop();
                        if (!_framesDict.ContainsKey(f.Identifier))
                        {
                            var fr = new BrowserFrame
                            {
                                Identifier = f.Identifier,
                                Name = GetFrameName(f),
                                LoadedUrl = frame.Url
                            };
                            _framesDict.Add(f.Identifier, fr);

                            var pfr = f.Parent == null ? RootFrame : _framesDict[f.Parent.Identifier];

                            if (pfr.Frames == null)
                                pfr.Frames = new ObservableCollection<BrowserFrame>();
                            pfr.Frames.Add(fr);

                            //Trace.WriteLine($"ID: {fr.Identifier}");
                        }
                    }
                }
            });
        }
        private string GetFrameName(IFrame frame)
        {
            var name = frame.IsMain ? "Root" : frame.Name;

            if (name.Length > 50)
                name = name.Substring(0, 50);

            return name;
        }

        private void DeveloperToolsButton_Click(object sender, RoutedEventArgs e)
        {
            CBrowser.ShowDevTools();
        }
    }
}
