﻿namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using System;
    using System.Collections.Generic;
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
        }
        public WebElementWithReferenceViewModel(WebElementReference webElementReference)
            : base(webElementReference)
        {
            ReferenceBreadString = webElementReference.TreePathToReferencedElement;
            HasLocator = webElementReference.Locator != null;
            ReferencedWebElement = WebElementsViewModelsHelper.CreateModelFromInfo(webElementReference.ReferencedElement);
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

        private bool _hasLocator;
        public bool HasLocator
        {
            get => _hasLocator;
            set => this.RaiseAndSetIfChanged(ref _hasLocator, value);
        }

        public IWebElementInfo ReferencedElement => ReferencedWebElement;
    }
}
