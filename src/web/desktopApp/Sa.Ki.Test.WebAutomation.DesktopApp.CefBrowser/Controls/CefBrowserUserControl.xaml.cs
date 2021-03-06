﻿namespace Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser.Controls
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

            CurrentUrl = "file:///C:/Dev/JSTest/test.html";

            CBrowser.FrameLoadStart += CBrowser_FrameLoadStart;
            CBrowser.FrameLoadEnd += CBrowser_FrameLoadEnd;

            CBrowser.JavascriptObjectRepository.ResolveObject += JavascriptObjectRepository_ResolveObject;
        }

        private void JavascriptObjectRepository_ResolveObject(object sender, CefSharp.Event.JavascriptBindingEventArgs e)
        {
            //Trace.WriteLine($"=== Resolve request for: {e.ObjectName}");

            var objName = e.ObjectName;

            var ps = objName.Split('_');
            if (ps.Length < 2) return;

            var identifier = long.Parse(ps[1]);

            if (CBrowser.JavascriptObjectRepository.IsBound(objName))
            {
                CBrowser.JavascriptObjectRepository.UnRegister(objName);
                //Trace.WriteLine($"=== Unregister: {objName}");
            }

            //Trace.WriteLine($"=== Register: {objName}");
            var fr = _framesDict[identifier];
            var syncObj = fr.SyncObject;
            CBrowser.JavascriptObjectRepository.Register(syncObj.BindName, syncObj, true);
        }

        private void CBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var frame = e.Frame;
            var fr = _framesDict[frame.Identifier];

            //if (fr.IsLoaded)
            //Trace.WriteLine($"=== Reloaded: {objName}");
            fr.IsLoaded = true;

            //Trace.WriteLine($"=== Inject: {objName}");

            var jsLines = File.ReadAllLines(@"C:\Dev\SaKi\Sa.Ki.Test\src\web\desktopApp\Sa.Ki.Test.WebAutomation.DesktopApp.CefBrowser\JS\frameSync.js");
            jsLines[0] = $"var frameSyncObjectName = \"{fr.SyncObject.BindName}\";";
            var js = string.Join(Environment.NewLine, jsLines);
            frame.ExecuteJavaScriptAsync(js);
        }

        private Dictionary<long, BrowserFrame> _framesDict = new Dictionary<long, BrowserFrame>();
        private void CBrowser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var frame = e.Frame;

                if (frame.IsMain)
                {
                    CBrowser.JavascriptObjectRepository.UnRegisterAll();

                    var guid = Guid.NewGuid();
                    //Trace.WriteLine($"ROOT UPDATED: {guid}");
                    RootFrame = new BrowserFrame
                    {
                        Frames = new ObservableCollection<BrowserFrame>(),
                        Identifier = frame.Identifier,
                        Name = $"{GetFrameName(frame)}",
                        LoadedUrl = frame.Url,
                        SyncObject = new BrowserFrameSyncBoundObject(frame.Identifier)
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
                                LoadedUrl = frame.Url,
                                SyncObject = new BrowserFrameSyncBoundObject(frame.Identifier)
                            };
                            _framesDict.Add(f.Identifier, fr);

                            var pfr = f.Parent == null ? RootFrame : _framesDict[f.Parent.Identifier];

                            if (pfr.Frames == null)
                                pfr.Frames = new ObservableCollection<BrowserFrame>();
                            pfr.Frames.Add(fr);
                            fr.Parent = pfr;

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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var br = CBrowser.GetBrowser();
            foreach (var f in RootFrame.IterateSelfAndChildren())
            {
                var fr = br.GetFrame(f.Identifier);
                f.SyncObject.ClearHighlight(fr);
                if (!string.IsNullOrEmpty(MagicTextBox.Text))
                    f.SyncObject.Highlight(MagicTextBox.Text, fr);
            }
        }
    }
}
