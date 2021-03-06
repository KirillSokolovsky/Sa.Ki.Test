﻿namespace Sa.Ki.Test.SakiTree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class SakiTreeHelper
    {
        public const string TreePathStringDelimeter = " > ";

        public static bool IsDescendantdFor(this ISakiTreeNode node, ISakiTreeCombinedNode combinedNode)
        {
            if (combinedNode == null) return false;
            if (node == null) return false;
            if (node.Parent == null) return false;
            if (node.Parent == combinedNode) return true;

            return combinedNode.Children
                ?.Any(c => node.IsDescendantdFor(c as ISakiTreeCombinedNode)) 
                ?? false;
        }

        public static bool HasAscendant(this ISakiTreeNode node, ISakiTreeCombinedNode combinedNode)
        {
            return !node.IsDescendantdFor(combinedNode);
        }

        public static string GetTreePath(this ISakiTreeNode sakiTreeNode, string nodesHierarchyDelimeter = TreePathStringDelimeter)
        {
            var node = sakiTreeNode;
            var sb = new StringBuilder(node.Name);
            node = node.Parent;

            while (node != null)
            {
                sb.Insert(0, $"{node.Name}{nodesHierarchyDelimeter}");
                node = node.Parent;
            }

            return sb.ToString();
        }


        public static ISakiTreeNode FindNodeByTreePath(this ISakiTreeCombinedNode combinedNode, IEnumerable<string> pathParts)
        {
            if (combinedNode.Children == null)
                return null;

            var name = pathParts.First();

            var node = combinedNode.Children.FirstOrDefault(n => n.Name == name);
            if (node == null) return null;
            if (pathParts.Count() == 1) return node;

            if (node is ISakiTreeCombinedNode cn)
                return cn.FindNodeByTreePath(pathParts.Skip(1));

            return null;
        }
        public static ISakiTreeNode FindNodeByTreePath(this ISakiTreeCombinedNode combinedNode, string childTreePath, string nodesHierarchyDelimeter = TreePathStringDelimeter)
        {
            var pathParts = childTreePath.Split(new[] { nodesHierarchyDelimeter }, StringSplitOptions.RemoveEmptyEntries);

            if (pathParts.Length == 0) return null;

            return combinedNode.FindNodeByTreePath(pathParts);
        }
        public static ISakiTreeNode FindNodeByTreePath(this IEnumerable<ISakiTreeCombinedNode> combinedNodes, string childTreePath, string nodesHierarchyDelimeter = TreePathStringDelimeter)
        {
            var pathParts = childTreePath.Split(new[] { nodesHierarchyDelimeter }, StringSplitOptions.RemoveEmptyEntries);

            if (pathParts.Length == 0) return null;

            var cn = combinedNodes?.FirstOrDefault(n => n.Name == pathParts[0]);
            ISakiTreeNode result = cn;

            if (result != null && pathParts.Length > 1)
            {
                result = cn.FindNodeByTreePath(pathParts.Skip(1));
            }

            return result;
        }
    }
}
