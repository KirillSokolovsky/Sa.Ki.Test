namespace Sa.Ki.Test.SakiTree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class SakiTreeNodeProvider<T> : ISakiTreeNodeProvider
    {
        public SakiTreeNodeProvider()
        {

        }

        public ISakiTreeNodeCommand GetCommandForTreeNode(ISakiTreeNode sakiTreeNode)
        {
            throw new NotImplementedException();
        }

        public abstract ISakiTreeNode GetNode();
    }
}
