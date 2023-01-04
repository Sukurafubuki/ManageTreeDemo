using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageTreeDemo.Common;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ManageTreeDemo.UserControls.Views;
using ManageTreeDemo.Model;

namespace ManageTreeDemo.UserControls.ViewModel
{
    public class MyTabcontrolVM:NotifyPropertyBase
    {
        /// <summary>
        /// 模型窗体视图实例
        /// </summary>
        public MyTabcontrol MyTabcontrol { get; set; }
        /// <summary>
        /// 选项卡集合
        /// </summary>
        public ObservableCollection<MyTabItemWithClose> TabItems { get; set; }
        /// <summary>
        /// 已打开的节点集合
        /// </summary>
        public List<Node> OpenedNodes = new List<Node>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public MyTabcontrolVM(MyTabcontrol _myTabcontrol)
        {
            TabItems = new ObservableCollection<MyTabItemWithClose>();
            MyTabcontrol = _myTabcontrol;
        }

        #region 初始化测试数据
        /// <summary>
        /// 初始化测试数据
        /// </summary>
        private void testinit()
        {
            //MyTabItemWithClose tab1 = new MyTabItemWithClose();
            //tab1.Header = "test1";
            //TabItems.Add(tab1);
            //MyTabItemWithClose tab2 = new MyTabItemWithClose();
            //tab2.Header = "test2";
            //TabItems.Add(tab2);
        }
        #endregion

        #region 打开节点
        /// <summary>
        /// 打开节点方法
        /// </summary>
        /// <param name="_node">点击节点</param>
        public void OpenNode(Node _node)
        {
            if (OpenedNodes.Exists(x=>x.NodeName==_node.NodeName))
            {
                //tabcontrol中item与openednodes中node增删动作一致，索引一致
                MyTabcontrol.TabControl1.SelectedIndex = OpenedNodes.IndexOf(OpenedNodes.Find(x => x.NodeName == _node.NodeName));
            }
            else
            {
                MyTabItemWithClose item = new MyTabItemWithClose(_node,this);
                item.Content = _node.NodeName=="canvas"? new BoardCaseCanvas(): new NodeDetails(_node);
                //绑定事件触发委托
                item._itemclose += this.itemclose;
                //MyTabcontrol.TabControl1.Items.Add(item);
                TabItems.Add(item);
                OpenedNodes.Add(_node);
                MyTabcontrol.TabControl1.SelectedItem = item;
            }
        }
        /// <summary>
        /// 同步已打开节点集合
        /// </summary>
        /// <param name="_node"></param>
        private void itemclose(Node _node)
        {
            OpenedNodes.Remove(_node);
        }
        #endregion
    }
}
