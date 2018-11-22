namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using Sa.Ki.Test.SakiTree;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class GoToReferencedCommand : WebElementCommand
    {
        public GoToReferencedCommand(WebElementsTreeUserControl webElementsTreeUserControl)
            : base(webElementsTreeUserControl)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return WebElementsViewModelsHelper.IsAnyParentReference(Selected);
        }

        protected override void ExecuteCommand()
        {
            var refParent = GetReferencedParent(Selected) as WebElementWithReferenceViewModel;
            if (refParent == null) return;

            var refPath = refParent.GetTreePath();
            var elPath = Selected.GetTreePath();

            var relativePath = elPath.Replace($"{refPath} > ", "");
            var sourceParts = refParent.ReferenceBreadString.Split(new[] { " > " }, StringSplitOptions.RemoveEmptyEntries);
            var sourcePath = string.Join(" > ", sourceParts.Take(sourceParts.Length - 1));

            var realPath = $"{sourcePath} > {relativePath}";

            var newSelected = (WebElementInfoViewModel)_webElementsTreeUserControl.WebElements.FindNodeByTreePath(realPath);
            _webElementsTreeUserControl.SelectedWebElement = newSelected;
        }

        private WebElementWithReferenceViewModel GetReferencedParent(WebElementInfoViewModel model)
        {
            var parent = model?.Parent;
            while (parent != null)
            {
                if (parent is WebElementWithReferenceViewModel r) return r;
                parent = parent.Parent;
            }
            return null;
        }
    }
}
