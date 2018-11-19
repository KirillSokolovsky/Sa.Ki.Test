namespace Sa.Ki.Test.WebAutomation
{
    using Sa.Ki.Test.SakiTree;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WebElementInfo : ISakiTreeNode, IWebElementInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ElementType { get; set; }
        public string InnerKey { get; set; }
        public bool IsKey { get; set; }


        public WebLocatorInfo Locator { get; set; }
        public List<string> Tags { get; set; }

        public CombinedWebElementInfo Parent { get; set; }

        ISakiTreeCombinedNode ISakiTreeNode.Parent => Parent;

        IWebLocatorInfo IWebElementInfo.Locator => Locator;

        ICombinedWebElementInfo IWebElementInfo.Parent => Parent;

        public WebElementInfo()
        {
            ElementType = WebElementTypes.Element;
        }


        private WebSearchInfo _webSearch;
        public WebSearchInfo GetWebSearch(bool reBuild = false)
        {
            if (_webSearch == null || reBuild)
                _webSearch = WebElementsHelper.BuildWebSearch(this);
            return _webSearch;
        }


        public virtual WebElementInfo GetCopyWithoutParent(WebElementInfo webElementInfo = null)
        {
            if (webElementInfo == null)
                webElementInfo = new WebElementInfo();

            webElementInfo.Name = Name;
            webElementInfo.Description = Description;
            webElementInfo.ElementType = ElementType;
            webElementInfo.InnerKey = InnerKey;
            webElementInfo.IsKey = IsKey;
            webElementInfo.Locator = Locator?.GetCopy();
            webElementInfo.Tags = Tags?.ToList();

            return webElementInfo;
        }


        private StringBuilder _sb;
        public override string ToString()
        {
            if (_sb == null)
            {
                _sb = new StringBuilder();
                _sb.Append(ElementType);
                _sb.Append(" ");
                _sb.Append(Name);

                if (Parent != null)
                {
                    _sb.Append(" on ");
                    _sb.Append(Parent.ElementType);
                    _sb.Append(" ");
                    _sb.Append(Parent.Name);
                }
            }

            return _sb.ToString();
        }
    }
}
