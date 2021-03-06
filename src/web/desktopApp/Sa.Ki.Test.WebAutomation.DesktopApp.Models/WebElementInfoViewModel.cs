﻿namespace Sa.Ki.Test.WebAutomation.DesktopApp.Models
{
    using ReactiveUI;
    using Sa.Ki.Test.DesktopApp.Models.SaKiMenu;
    using Sa.Ki.Test.SakiTree;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class WebElementInfoViewModel : ReactiveObject, ISakiTreeNode, IWebElementInfo
    {
        protected WebElementInfo _sourceWebElement { get; set; }

        public WebElementInfoViewModel(WebElementInfo webElementInfo = null)
        {
            _sourceWebElement = webElementInfo ?? new WebElementInfo();

            Name = _sourceWebElement.Name;
            Description = _sourceWebElement.Description;
            ElementType = _sourceWebElement.ElementType;
            InnerKey = _sourceWebElement.InnerKey;
            IsKey = _sourceWebElement.IsKey;

            if (_sourceWebElement.Tags != null)
                Tags = new ObservableCollection<string>(_sourceWebElement.Tags);

            if(webElementInfo == null
                || !(webElementInfo.ElementType == WebElementTypes.Reference
                        && (webElementInfo as WebElementReference).Locator == null))
            Locator = WebElementsViewModelsHelper.CreateLocatorModel(_sourceWebElement.Locator);
        }

        private CombinedWebElementInfoViewModel _parent;
        public CombinedWebElementInfoViewModel Parent
        {
            get => _parent;
            set => this.RaiseAndSetIfChanged(ref _parent, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        private string _elementType;
        public string ElementType
        {
            get => _elementType;
            set => this.RaiseAndSetIfChanged(ref _elementType, value);
        }

        private string _innerKey;
        public string InnerKey
        {
            get => _innerKey;
            set => this.RaiseAndSetIfChanged(ref _innerKey, value);
        }

        private bool _isKey;
        public bool IsKey
        {
            get => _isKey;
            set => this.RaiseAndSetIfChanged(ref _isKey, value);
        }

        private ObservableCollection<string> _tags;
        public ObservableCollection<string> Tags
        {
            get => _tags;
            set => this.RaiseAndSetIfChanged(ref _tags, value);
        }

        private WebLocatorInfoViewModel _locator;
        public WebLocatorInfoViewModel Locator
        {
            get => _locator;
            set => this.RaiseAndSetIfChanged(ref _locator, value);
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set => this.RaiseAndSetIfChanged(ref _isVisible, value);
        }

        ISakiTreeCombinedNode ISakiTreeNode.Parent => Parent;

        IWebLocatorInfo IWebElementInfo.Locator => Locator;

        ICombinedWebElementInfo IWebElementInfo.Parent => Parent;

        public override string ToString()
        {
            return $"{ElementType} | {Name}";
        }

        public WebSearchInfo GetWebSearch(bool reBuild = false)
        {
            return WebElementsHelper.BuildWebSearch(this);
        }
    }
}
