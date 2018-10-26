namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
                && !CutOrCopied.IsDescendantdOf(Selected as CombinedWebElementInfoViewModel)
                && (Selected as CombinedWebElementInfoViewModel).Elements
                    ?.FirstOrDefault(e => e.Name == CutOrCopied.Name) == null;

            return result;
        }

        protected override void ExecuteCommand(WebElementInfoViewModel elementModel)
        {
            if (!CanExecute(elementModel)) return;

            var combined = Selected as CombinedWebElementInfoViewModel;

            if (combined.Elements == null) combined.Elements = new ObservableCollection<WebElementInfoViewModel>();

            WebElementInfoViewModel model = Cut;

            if (Copied != null)
            {
                var info = WebElementsViewModelsFactory.CreateInfoFromModel(Copied);
                model = WebElementsViewModelsFactory.CreateModelFromInfo(info);
            }

            model.Parent = combined;
            combined.Elements.Add(model);
        }
    }
}
