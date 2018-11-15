using Sa.Ki.Test.DesktopApp.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace Sa.Ki.Test.DesktopApp.Controls
{
    public partial class ListOfStringsUserControl : UserControl
    {
        public ObservableCollection<string> StringItems
        {
            get { return (ObservableCollection<string>)GetValue(StringItemsProperty); }
            set { SetValue(StringItemsProperty, value); }
        }
        public static readonly DependencyProperty StringItemsProperty =
            DependencyProperty.Register("StringItems", typeof(ObservableCollection<string>), typeof(ListOfStringsUserControl), new PropertyMetadata(new ObservableCollection<string>()));

        public int TextInputMinWidth
        {
            get { return (int)GetValue(TextInputMinWidthProperty); }
            set { SetValue(TextInputMinWidthProperty, value); }
        }
        public static readonly DependencyProperty TextInputMinWidthProperty =
            DependencyProperty.Register("TextInputMinWidth", typeof(int), typeof(ListOfStringsUserControl), new PropertyMetadata(50));

        public string NewStringItem
        {
            get { return (string)GetValue(NewStringItemProperty); }
            set { SetValue(NewStringItemProperty, value); }
        }
        public static readonly DependencyProperty NewStringItemProperty =
            DependencyProperty.Register("NewStringItem", typeof(string), typeof(ListOfStringsUserControl), new PropertyMetadata(null));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ListOfStringsUserControl), new PropertyMetadata(false));

        public string SelectedStringItem
        {
            get { return (string)GetValue(SelectedStringItemProperty); }
            set { SetValue(SelectedStringItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedStringItemProperty =
            DependencyProperty.Register("SelectedStringItem", typeof(string), typeof(ListOfStringsUserControl), new PropertyMetadata(null));

        public bool AskConfirmationOnDelete
        {
            get { return (bool)GetValue(AskConfirmationOnDeleteProperty); }
            set { SetValue(AskConfirmationOnDeleteProperty, value); }
        }
        public static readonly DependencyProperty AskConfirmationOnDeleteProperty =
            DependencyProperty.Register("AskConfirmationOnDelete", typeof(bool), typeof(ListOfStringsUserControl), new PropertyMetadata(false));

        public ObservableCollection<string> AvailableStringItems
        {
            get { return (ObservableCollection<string>)GetValue(AvailableStringItemsProperty); }
            set { SetValue(AvailableStringItemsProperty, value); }
        }
        public static readonly DependencyProperty AvailableStringItemsProperty =
            DependencyProperty.Register("AvailableStringItems", typeof(ObservableCollection<string>), typeof(ListOfStringsUserControl), new PropertyMetadata(null));

        public bool IsCustomAvailableStringItemsAllowed
        {
            get { return (bool)GetValue(IsCustomAvailableStringItemsPropertyAllowed); }
            set { SetValue(IsCustomAvailableStringItemsPropertyAllowed, value); }
        }
        public static readonly DependencyProperty IsCustomAvailableStringItemsPropertyAllowed =
            DependencyProperty.Register("IsCustomAvailableStringItemsAllowed", typeof(bool), typeof(ListOfStringsUserControl), new PropertyMetadata(false));

        public bool IsAvailableItemsUnpined
        {
            get { return (bool)GetValue(IsAvailableItemsUnpinedProperty); }
            set { SetValue(IsAvailableItemsUnpinedProperty, value); }
        }
        public static readonly DependencyProperty IsAvailableItemsUnpinedProperty =
            DependencyProperty.Register("IsAvailableItemsUnpined", typeof(bool), typeof(ListOfStringsUserControl), new PropertyMetadata(true));

        public bool IsEnabledToAddRemovedItemsToAvailable
        {
            get { return (bool)GetValue(IsEnabledToAddRemovedItemsToAvailableProperty); }
            set { SetValue(IsEnabledToAddRemovedItemsToAvailableProperty, value); }
        }
        public static readonly DependencyProperty IsEnabledToAddRemovedItemsToAvailableProperty =
            DependencyProperty.Register("IsEnabledToAddRemovedItemsToAvailable", typeof(bool), typeof(ListOfStringsUserControl), new PropertyMetadata(true));

        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }
        public static readonly DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string), typeof(ListOfStringsUserControl), new PropertyMetadata("string item"));



        public ListOfStringsUserControl()
        {
            InitializeComponent();

            LayoutGrid.DataContext = this;
        }

        private bool _isUnpinningInProcess = false;
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (IsAvailableItemsUnpined && e.Property.Name == nameof(AvailableStringItems))
            {
                if (!_isUnpinningInProcess)
                {
                    _isUnpinningInProcess = true;
                    AvailableStringItems = new ObservableCollection<string>((e.NewValue as ObservableCollection<string>)?.ToList());
                }
                else
                    _isUnpinningInProcess = false;
            }
        }

        private void AvailableStringItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Trace.WriteLine($"{e.Action}");
        }

        private void DeleteStringItemMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedStringItem == null) return;

            if (AskConfirmationOnDelete)
            {
                var result = MessageBox.Show($"Confirm removal for item: {SelectedStringItem}",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.No);

                if (result != MessageBoxResult.Yes) return;
            }

            if (IsEnabledToAddRemovedItemsToAvailable
                && !(AvailableStringItems?.Contains(SelectedStringItem) ?? false))
                AvailableStringItems?.Add(SelectedStringItem);

            StringItems.RemoveAt(StringItemsListBox.SelectedIndex);
        }

        private void AddNewStringItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewStringItem)) return;

            if (StringItems?.Contains(NewStringItem) ?? false)
            {
                MessageBox.Show($"Item: {NewStringItem} already exists",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (StringItems == null)
                StringItems = new ObservableCollection<string>();
            StringItems.Add(NewStringItem);

            if (AvailableStringItems?.Contains(NewStringItem) ?? false)
                AvailableStringItems?.Remove(NewStringItem);

            NewStringItem = null;
        }

        private void EditStringItemMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedStringItem == null) return;

            var index = StringItemsListBox.SelectedIndex;

            var editDialog = new TextDialog(ItemName, SelectedStringItem);

            editDialog.Validate = str => !string.IsNullOrEmpty(str) && !StringItems.Contains(str);

            if (editDialog.ShowDialog() != true) return;

            StringItems[index] = editDialog.Text;
        }


    }
}
