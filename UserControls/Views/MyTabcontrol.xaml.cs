using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ManageTreeDemo.UserControls.ViewModel;
using ManageTreeDemo.Model;

namespace ManageTreeDemo.UserControls.Views
{
    /// <summary>
    /// MyTabcontrol.xaml 的交互逻辑
    /// </summary>
    public partial class MyTabcontrol : UserControl
    {
        /// <summary>
        /// 已打开的节点集合
        /// </summary>
        public List<Node> OpenedNodes = new List<Node>();

        public MyTabcontrol()
        {
            InitializeComponent();
        }

        /// <summary>
        /// tabcontrol响应节点点击事件
        /// </summary>
        /// <param name="_node"></param>
        public void Nodeload(Node _node)
        {
            if (OpenedNodes.Contains(_node))
            {
                //tabcontrol中item与openednodes中node增删动作一致，索引一致
                TabControl1.SelectedIndex = OpenedNodes.IndexOf(_node);
            }
            else
            {
                MyTabItemWithClose item = new MyTabItemWithClose(_node);
                item.Content = new NodeDetails(_node);
                //绑定事件触发委托
                item._itemclose += this.itemclose;
                TabControl1.Items.Add(item);
                OpenedNodes.Add(_node);
                TabControl1.SelectedItem = item;
            }
        }

        private void itemclose(Node _node)
        {
            OpenedNodes.Remove(_node);
        }
    }
}
