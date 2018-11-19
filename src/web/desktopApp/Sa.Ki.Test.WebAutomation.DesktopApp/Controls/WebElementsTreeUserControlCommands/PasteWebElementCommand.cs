namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using Sa.Ki.Test.SakiTree;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class PasteWebElementCommand : WebElementCommand
    {
        public PasteWebElementCommand(WebElementsTreeUserControl webElementsTreeUserControl)
            : base(webElementsTreeUserControl)
        {
        }

        public override bool CanExecute(object parameter)
        {
            var result = CutOrCopied != null
                && Selected != CutOrCopied
                && WebElementCommandsHelper.CanElementHasCustomChildren(Selected)
                && !(Cut != null && Selected.IsDescendantdFor(Cut as CombinedWebElementInfoViewModel))
                && ((Selected as CombinedWebElementInfoViewModel)?.Elements
                    ?? _webElementsTreeUserControl.WebElements.Cast<WebElementInfoViewModel>())
                    ?.FirstOrDefault(e => e.Name == CutOrCopied.Name) == null;

            return result;
        }

        protected override void ExecuteCommand()
        {
            if (!CanExecute(null)) return;

            WebElementInfoViewModel model = Cut;

            var combined = Selected as CombinedWebElementInfoViewModel;

            if (combined == null && Selected != null)
            {
                MessageBox.Show("Magic!!! Try to paste to not combined element!");
                return;
            }

            if (combined != null)
            {
                if (combined.Elements == null)
                    combined.Elements = new ObservableCollection<WebElementInfoViewModel>();
            }

            if (Copied != null)
            {
                var info = WebElementsViewModelsHelper.CreateInfoFromModel(Copied);
                model = WebElementsViewModelsHelper.CreateModelFromInfo(info);
            }
            else
            {
                if (model.Parent != null)
                    model.Parent.Elements.Remove(model);
                else
                    _webElementsTreeUserControl.WebElements.Remove(model as CombinedWebElementInfoViewModel);
            }

            model.Parent = combined;

            if (combined != null)
                combined.Elements.Add(model);
            else
            {
                var cmb = model as CombinedWebElementInfoViewModel;
                if(cmb == null)
                {
                    MessageBox.Show("Magic!!! Try to paste not combined element to Root!");
                    return;
                }
                _webElementsTreeUserControl.WebElements.Add(cmb);
            }

            _webElementsTreeUserControl.SelectedWebElement = model;
        }
    }
}
