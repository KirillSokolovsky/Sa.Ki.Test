namespace Sa.Ki.Test.WebAutomation.DesktopApp.Controls.WebElementInfoUserControlCommon
{
    using Sa.Ki.Test.WebAutomation.DesktopApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

    public class WebElementInfoDetailsTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is WebElementInfoViewModel model)
            {
                switch (model.ElementType)
                {
                    case WebElementTypes.Directory:
                        return element.FindResource("BaseInfo") as DataTemplate;
                    case WebElementTypes.Page:
                        return element.FindResource("PageInfo") as DataTemplate;
                    case WebElementTypes.Frame:
                    case WebElementTypes.Reference:
                        return element.FindResource("ElementWithReferenceInfo") as DataTemplate;
                    default:
                        return element.FindResource("ElementInfo") as DataTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
