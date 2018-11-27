namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;
    using Sa.Ki.Test.SakiTree;

    public class WebElementWithReferenceViewModel : CombinedWebElementInfoViewModel, IWebElementWithReferenceInfo
    {
        public WebElementWithReferenceViewModel(FrameWebElementInfo frameWebElement)
            : base(frameWebElement)
        {
            ReferenceBreadString = frameWebElement.TreePathToInnerElement;
            ReferencedWebElement = WebElementsViewModelsHelper.CreateModelFromInfo(frameWebElement.InnerElement);
            ReferencedWebElement.Parent = this;
            Elements = new ObservableCollection<WebElementInfoViewModel>();
            Elements.Add(ReferencedWebElement);
        }
        public WebElementWithReferenceViewModel(WebElementReference webElementReference)
            : base(webElementReference)
        {
            ReferenceBreadString = webElementReference.TreePathToReferencedElement;
            HasLocator = webElementReference.Locator != null;
            ReferencedWebElement = WebElementsViewModelsHelper.CreateModelFromInfo(webElementReference.ReferencedElement);
            ReferencedWebElement.Parent = this;

            if(ReferencedWebElement is CombinedWebElementInfoViewModel cmb)
            {
                if(cmb.Elements != null)
                {
                    Elements = new ObservableCollection<WebElementInfoViewModel>();
                    foreach (var c in cmb.Elements)
                    {
                        Elements.Add(c);
                        c.Parent = this;
                    }
                }
            }
        }

        public WebElementWithReferenceViewModel(string elementType)
            : base(null)
        {
            ElementType = elementType;
            if (ElementType == WebElementTypes.Frame)
                Locator = new FrameWebLocatorInfoViewModel();
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

        private WebLocatorInfoViewModel _savedLocator = null;
        private bool _hasLocator;
        public bool HasLocator
        {
            get => _hasLocator;
            set
            {
                if (ElementType == WebElementTypes.Reference)
                {
                    if (value && Locator == null)
                        Locator = _savedLocator ?? new WebLocatorInfoViewModel();

                    if (!value)
                    {
                        _savedLocator = Locator;
                        Locator = null;
                    }
                }

                this.RaiseAndSetIfChanged(ref _hasLocator, value);
            }

        }

        public IWebElementInfo ReferencedElement => ReferencedWebElement;
    }
}
