using System;
using System.Collections.Generic;
using System.Text;

namespace Sa.Ki.Test.SakiTree
{
    public interface ISakiTreeNode
    {
        string Name { get; }
        string Title { get; }
        string Description { get; }
        string NodeType { get; }

        IList<ISakiTreeNode> Children { get; }
        ISakiTreeNode Parent { get; }
    }
}
