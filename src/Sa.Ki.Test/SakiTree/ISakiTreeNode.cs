﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sa.Ki.Test.SakiTree
{
    public interface ISakiTreeNode
    {
        string Name { get; }
        ISakiTreeCombinedNode Parent { get; }
    }
}
