namespace Sa.Ki.Test.WebAutomation
{
    using Sa.Ki.Test.SakiTree;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CombinedWebElementInfo : WebElementInfo, ISakiTreeCombinedNode
    {
        public CombinedWebElementInfo()
        {
            ElementType = WebElementTypes.Control;
        }

        public List<WebElementInfo> Elements { get; set; }

        public IEnumerable<ISakiTreeNode> Children => Elements;

        public override WebElementInfo GetCopyWithoutParent(WebElementInfo webElementInfo = null)
        {
            var element = webElementInfo as CombinedWebElementInfo
                ?? new CombinedWebElementInfo();

            element.Elements = Elements?.Select(e => e.GetCopyWithoutParent())
                .ToList();
            element.Elements.ForEach(e => e.Parent = element);

            return base.GetCopyWithoutParent(element);
        }

        public List<CombinedWebElementInfo> GetChildContexts()
        {
            var childrenContexts = new List<CombinedWebElementInfo>();
            foreach (var child in Elements)
            {
                if (child is CombinedWebElementInfo combined)
                    childrenContexts.Add(combined);
            }
            return childrenContexts;
        }

        public WebElementInfo this[string innerKey]
        {
            get
            {
                return Elements?.FirstOrDefault(e => e.InnerKey == innerKey);
            }
        }
    }
}
