﻿namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementsTreeUserControlCommands
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class DeleteWebElementCommand : WebElementCommand
    {
        public DeleteWebElementCommand(WebElementsTreeUserControl webElementsTreeUserControl)
            : base(webElementsTreeUserControl)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Selected != null;
        }

        protected override void ExecuteCommand(WebElementInfoViewModel elementModel)
        {
            var toRemove = Selected;
            if (Selected.Parent == null)
            {
                if (toRemove is WebContextInfoViewModel wc)
                    _webElementsTreeUserControl.WebContexts.Remove(wc);
            }
            else
            {
                toRemove.Parent.Elements?.Remove(toRemove);
                toRemove.Parent = null;
            }
        }
    }
}
