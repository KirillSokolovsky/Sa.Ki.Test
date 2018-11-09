using System;
using System.Collections.Generic;
using System.Text;

namespace Sa.Ki.Test.SakiTree
{
    public interface ISakiDescriptor
    {
        string Name { get; }
        string Title { get; }
        string Description { get; }
        Type NodeType { get; }
    }
}
