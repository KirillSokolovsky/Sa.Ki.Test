namespace Sa.Ki.Test.DesktopApp.Models.SaKiMenu
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class SaKiCommandMenuItemViewModel : SaKiMenuItemViewModel
    {
        private ICommand _command;
        public ICommand Command
        {
            get => _command;
            set => this.RaiseAndSetIfChanged(ref _command, value);
        }
    }
}
