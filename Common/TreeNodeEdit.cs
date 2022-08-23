using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageTreeDemo.Model;
using ManageTreeDemo.Helpers;
using ManageTreeDemo.ViewModel;

namespace ManageTreeDemo.Common
{
    /// <summary>
    /// 节点编辑类
    /// </summary>
    public static class TreeNodeEdit
    {
        /// <summary>
        /// 复制节点
        /// </summary>
        /// <param name="node"></param>
        public static bool NodeCopy(Node node)
        {
            //复制选择节点对象到剪切板（mainwindow静态变量）
            MainWindow.Clipper.Node = node;
            return true;
        }

        /// <summary>
        /// 剪切节点
        /// </summary>
        /// <param name="node"></param>
        public static bool NodeCut(Node node)
        {
            MainWindow.Clipper.Node = node;
            MainWindow.Clipper.IsCut = true;
            return true;
        }

        /// <summary>
        /// 粘贴节点
        /// </summary>
        /// <param name="targetNode">粘贴目标节点</param>
        /// <returns>粘贴结果</returns>
        public static bool NodePaste(Node targetNode)
        {
            try
            {
                XmlHelper.InsertByRecursion<Node>(MainTreeViewVM.fullpath, targetNode.Site, MainWindow.Clipper.Node);
                if (MainWindow.Clipper.IsCut)
                {
                    XmlHelper.Delete(MainTreeViewVM.fullpath, MainWindow.Clipper.Node.Site, "");
                    MainWindow.Clipper.IsCut = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
