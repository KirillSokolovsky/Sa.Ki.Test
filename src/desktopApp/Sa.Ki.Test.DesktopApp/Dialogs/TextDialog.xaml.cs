using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Sa.Ki.Test.DesktopApp.Dialogs
{
    public partial class TextDialog : MetroWindow
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextDialog), new PropertyMetadata(null));

        public bool IsMultiline
        {
            get { return (bool)GetValue(IsMultilineProperty); }
            set { SetValue(IsMultilineProperty, value); }
        }
        public static readonly DependencyProperty IsMultilineProperty =
            DependencyProperty.Register("IsMultiline", typeof(bool), typeof(TextDialog), new PropertyMetadata(false));

        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }
        public static readonly DependencyProperty IsEditModeProperty =
            DependencyProperty.Register("IsEditMode", typeof(bool), typeof(TextDialog), new PropertyMetadata(false));
               
        public int MinLines
        {
            get { return (int)GetValue(MinLinesProperty); }
            set { SetValue(MinLinesProperty, value); }
        }
        public static readonly DependencyProperty MinLinesProperty =
            DependencyProperty.Register("MinLines", typeof(int), typeof(TextDialog), new PropertyMetadata(1));

        public string DialogTitle
        {
            get { return (string)GetValue(DialogTitleProperty); }
            set { SetValue(DialogTitleProperty, value); }
        }
        public static readonly DependencyProperty DialogTitleProperty =
            DependencyProperty.Register("DialogTitle", typeof(string), typeof(TextDialog), new PropertyMetadata(null));

        public Func<string, bool> Validate { get; set; }

        private readonly string _sourceText;

        public TextDialog(string itemName, string text = null)
        {
            _sourceText = text;
            IsEditMode = text != null;
            DialogTitle = IsEditMode ? $"Edit {itemName}" : $"Create {itemName}";
            Text = text;

            InitializeComponent();

            DataContext = this;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var state = Validate?.Invoke(Text) ?? true;

            if (state && IsEditMode && _sourceText == Text)
                state = false;

            AcceptButton.IsEnabled = state;
        }
    }
}
