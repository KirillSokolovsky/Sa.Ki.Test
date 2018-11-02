namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls
{
    using CefSharp;
    using Sa.Ki.Test.WebAutomation.DesktopApp.JSB;
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
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

    public partial class SaKiChromeBrowserUserControl : UserControl
    {
        public WebElementInfoViewModel WebElement
        {
            get { return (WebElementInfoViewModel)GetValue(WebElementProperty); }
            set { SetValue(WebElementProperty, value); }
        }
        public static readonly DependencyProperty WebElementProperty =
            DependencyProperty.Register("WebElement", typeof(WebElementInfoViewModel), typeof(SaKiChromeBrowserUserControl), new PropertyMetadata(null));

        public string CurrentUrl
        {
            get { return (string)GetValue(CurrentUrlProperty); }
            set { SetValue(CurrentUrlProperty, value); }
        }
        public static readonly DependencyProperty CurrentUrlProperty =
            DependencyProperty.Register("CurrentUrl", typeof(string), typeof(SaKiChromeBrowserUserControl), new PropertyMetadata(null));

        public ObservableCollection<BrowserFrame> Frames
        {
            get { return (ObservableCollection<BrowserFrame>)GetValue(FramesProperty); }
            set { SetValue(FramesProperty, value); }
        }
        public static readonly DependencyProperty FramesProperty =
            DependencyProperty.Register("Frames", typeof(ObservableCollection<BrowserFrame>), typeof(SaKiChromeBrowserUserControl), new PropertyMetadata(null));



        public SaKiChromeBrowserUserControl()
        {
            InitializeComponent();

            CurrentUrl = "https://habr.com/all/";
            LayoutGrid.DataContext = this;

            CBrowser.FrameLoadEnd += CBrowser_FrameLoadEnd;
            CBrowser.FrameLoadStart += CBrowser_FrameLoadStart;

            _abo = new AsyncBoundObject();
            CBrowser.JavascriptObjectRepository.Register("boundAsync", _abo, true);
        }

        private Dictionary<long, BrowserFrame> _framesDict = new Dictionary<long, BrowserFrame>();
        private void CBrowser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            Trace.WriteLine($"{e.Frame.Identifier} >>> " + GetFrameName(e.Frame));
            Dispatcher.Invoke(() =>
            {
                var frame = e.Frame;

                if (frame.IsMain)
                {
                    if (Frames == null)
                        Frames = new ObservableCollection<BrowserFrame>();
                    else
                        Frames.Clear();
                    var rframe = new BrowserFrame
                    {
                        Frames = new ObservableCollection<BrowserFrame>(),
                        Identifier = frame.Identifier,
                        Name = GetFrameName(frame)
                    };
                    _framesDict.Clear();
                    _framesDict.Add(frame.Identifier, rframe);
                    Frames.Add(rframe);
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
                                Name = GetFrameName(f)
                            };
                            _framesDict.Add(f.Identifier, fr);

                            var pfr = f.Parent == null ? Frames.First() : _framesDict[f.Parent.Identifier];

                            if (pfr.Frames == null)
                                pfr.Frames = new ObservableCollection<BrowserFrame>();
                            pfr.Frames.Add(fr);

                            Trace.WriteLine($"ID: {fr.Identifier}");
                        }
                    }
                }
            });
        }

        private AsyncBoundObject _abo;

        private void CBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {

            //var js = File.ReadAllText("JSB\\main.js");
            //CBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(js);
        }




        private void GetFrameNameAndKey(IFrame frame, out string frameName, out string frameKey)
        {
            frameName = GetFrameName(frame);

            var sb = new StringBuilder();

            frame = frame.Parent;
            while (frame != null)
            {
                var key = $"{frame.Identifier}|{GetFrameName(frame)}";

                if (sb.Length == 0)
                    sb.Append(key);
                else
                    sb.Insert(0, $"{key} >> ");

                frame = frame.Parent;
            }

            frameKey = sb.ToString();
        }

        private string GetFrameName(IFrame frame)
        {
            var name = frame.IsMain ? "Root" : frame.Name;

            if (name.Length > 50)
                name = name.Substring(0, 50);

            return name;
        }

        public class AsyncBoundObject
        {
            //We expect an exception here, so tell VS to ignore
            [DebuggerHidden]
            public void Error()
            {
                throw new Exception("This is an exception coming from C#");
            }

            //We expect an exception here, so tell VS to ignore
            [DebuggerHidden]
            public int Div(int divident, int divisor)
            {
                return divident / divisor;
            }
        }
    }
}
