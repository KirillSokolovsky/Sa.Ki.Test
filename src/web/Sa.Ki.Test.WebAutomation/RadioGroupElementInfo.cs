namespace Sa.Ki.Test.WebAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RadioGroupElementInfo : CombinedWebElementInfo
    {
        public RadioGroupElementInfo()
        {
            ElementType = WebElementTypes.RadioGroup;
        }

        public WebElementInfo GetOptionElement() => this[Keys.Option];

        public override WebElementInfo GetCopyWithoutParent(WebElementInfo webElementInfo = null)
        {
            var element = webElementInfo as RadioGroupElementInfo
                ?? new RadioGroupElementInfo();

            return base.GetCopyWithoutParent(element);
        }

        public static class Keys
        {
            public const string Option = "Option";
        }
    }
}
