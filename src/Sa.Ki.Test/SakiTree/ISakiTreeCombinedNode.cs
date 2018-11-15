using System;
using System.Collections.Generic;
using System.Text;

namespace Sa.Ki.Test.SakiTree
{
    public interface ISakiTreeCombinedNode : ISakiTreeNode
    {
        IEnumerable<ISakiTreeNode> Children { get; }
    }
}
