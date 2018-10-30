namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public abstract class WebElementCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        protected WebElementsTreeUserControl _webElementsTreeUserControl;

        public WebElementInfoViewModel Selected => _webElementsTreeUserControl?.SelectedWebElement;
        public WebElementInfoViewModel Copied => _webElementsTreeUserControl?.CopiedWebElement;
        public WebElementInfoViewModel Cut => _webElementsTreeUserControl?.CutWebElement;
        public WebElementInfoViewModel CutOrCopied => Cut ?? Copied;

        public WebElementCommand(WebElementsTreeUserControl webElementsTreeUserControl)
        {
            _webElementsTreeUserControl = webElementsTreeUserControl;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ExecuteCommand();
        }

        protected abstract void ExecuteCommand();
    }
}
