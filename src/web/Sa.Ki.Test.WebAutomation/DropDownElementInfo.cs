namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DropDownElementInfo : CombinedWebElementInfo
    {
        public DropDownElementInfo()
        {
            ElementType = WebElementTypes.DropDown;
        }

        public WebElementInfo GetInputElement() => this[Keys.Input];
        public WebElementInfo GetOptionElement() => this[Keys.Option];

        public override WebElementInfo GetCopyWithoutParent(WebElementInfo webElementInfo = null)
        {
            var element = webElementInfo as DropDownElementInfo
                ?? new DropDownElementInfo();

            return base.GetCopyWithoutParent(element);
        }

        public static class Keys
        {
            public const string Input = "Input";
            public const string Option = "Option";
        }
    }
}
