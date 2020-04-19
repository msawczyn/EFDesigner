using System.Collections.Generic;
using System.Windows.Forms;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class TreeViewExtensions
   {
      public static List<TreeNode> GetAllNodes(this TreeView _self)
      {
         List<TreeNode> result = new List<TreeNode>();

         foreach (TreeNode child in _self.Nodes)
            result.AddRange(child.GetAllNodes());

         return result;
      }

      public static List<TreeNode> GetAllNodes(this TreeNode _self)
      {
         List<TreeNode> result = new List<TreeNode>();
         result.Add(_self);

         foreach (TreeNode child in _self.Nodes)
            result.AddRange(child.GetAllNodes());

         return result;
      }
   }
}