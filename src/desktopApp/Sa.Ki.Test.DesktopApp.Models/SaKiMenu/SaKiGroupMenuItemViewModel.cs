namespace Sa.Ki.Test.DesktopApp.Models.SaKiMenu
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class SaKiGroupMenuItemViewModel : SaKiMenuItemViewModel
    {
        private ObservableCollection<SaKiMenuItemViewModel> _items;
        public ObservableCollection<SaKiMenuItemViewModel> Items
        {
            get => _items;
            set => this.RaiseAndSetIfChanged(ref _items, value);
        }

        public ICommand Command => null;
    }
}
