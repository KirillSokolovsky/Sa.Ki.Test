namespace Sa.Ki.Test.SakiTree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SakiTreeNode<T> : ISakiTreeNode
        where T : ISakiDescriptor
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string NodeType { get; set; }

        public IList<ISakiTreeNode> Children { get; set; }
        public ISakiTreeNode Parent { get; set; }

        public T Data { get; set; }
    }
}
