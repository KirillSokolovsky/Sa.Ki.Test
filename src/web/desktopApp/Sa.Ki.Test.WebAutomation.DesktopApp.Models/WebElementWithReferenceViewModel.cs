namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;
    using Sa.Ki.Test.SakiTree;

    public class WebElementWithReferenceViewModel : WebElementInfoViewModel
    {
        public WebElementWithReferenceViewModel(FrameWebElementInfo frameWebElement)
            : base(frameWebElement)
        {
            ReferenceBreadString = frameWebElement.Path;
        }
        public WebElementWithReferenceViewModel(WebElementReference webElementReference)
            : base(webElementReference)
        {
            ReferenceBreadString = webElementReference.Path;
        }

        public WebElementWithReferenceViewModel(string elementType)
            : base(null)
        {
            ElementType = elementType;
        }

        private string _referenceBreadString;
        public string ReferenceBreadString
        {
            get => _referenceBreadString;
            set => this.RaiseAndSetIfChanged(ref _referenceBreadString, value);
        }
    }
}
