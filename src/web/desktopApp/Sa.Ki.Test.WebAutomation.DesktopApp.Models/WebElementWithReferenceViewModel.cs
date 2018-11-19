namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;
    using Sa.Ki.Test.SakiTree;

    public class WebElementWithReferenceViewModel : WebElementInfoViewModel, IWebElementWithReferenceInfo
    {
        public WebElementWithReferenceViewModel(FrameWebElementInfo frameWebElement)
            : base(frameWebElement)
        {
            ReferenceBreadString = frameWebElement.TreePathToInnerElement;
        }
        public WebElementWithReferenceViewModel(WebElementReference webElementReference)
            : base(webElementReference)
        {
            ReferenceBreadString = webElementReference.TreePathToReferencedElement;
        }

        public WebElementWithReferenceViewModel(string elementType)
            : base(null)
        {
            ElementType = elementType;
        }

        public WebElementWithReferenceViewModel()
        {

        }

        private string _referenceBreadString;
        public string ReferenceBreadString
        {
            get => _referenceBreadString;
            set => this.RaiseAndSetIfChanged(ref _referenceBreadString, value);
        }

        public WebElementInfoViewModel _referencedWebElement;
        public WebElementInfoViewModel ReferencedWebElement
        {
            get => _referencedWebElement;
            set => this.RaiseAndSetIfChanged(ref _referencedWebElement, value);
        }

        public IWebElementInfo ReferencedElement => ReferencedWebElement;
    }
}
