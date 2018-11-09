namespace Sa.Ki.Test.SakiTree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISakiTreeNodeProvider
    {
        ISakiTreeNode GetNode();
        ISakiTreeNodeCommand GetCommandForTreeNode(ISakiTreeNode sakiTreeNode);
    }
}
