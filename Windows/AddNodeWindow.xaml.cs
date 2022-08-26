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
using System.Windows.Shapes;
using ManageTreeDemo.Model;
using ManageTreeDemo.Helpers;
using ManageTreeDemo.Common;

namespace ManageTreeDemo.Windows
{
    /// <summary>
    /// AddNodeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddNodeWindow : Window
    {
        /// <summary>
        /// 添加窗口数据源
        /// </summary>
        public class adddata : NotifyPropertyBase
        {
            /// <summary>
            /// 添加节点位置（父节点路径）
            /// </summary>
            public string ParentNodeSite { get; set; }
        }

        adddata Datas=new adddata();

        string fullpath;

        public AddNodeWindow(string xmlpath, Node parentNode)
        {
            fullpath = xmlpath;
            Datas.ParentNodeSite = parentNode.Site;
            InitializeComponent();
            this.DataContext = Datas;
        }

        /// <summary>
        /// 添加窗口确认事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void confirmbtn_Click(object sender, RoutedEventArgs e)
        {
            if (NodeNametxb.Text[0] >= '0' && NodeNametxb.Text[0] <= '9')
            {
                MessageBox.Show("节点名不能以0-9数字开头");
                return;
            }
            if (NodetypeCmb.SelectedIndex == 0)
            {
                string str = Guid.NewGuid().ToString();
                XmlHelper.Insert(fullpath, Datas.ParentNodeSite, NodeNametxb.Text ,"Name", NodeNametxb.Text);//添加节点
                XmlHelper.Insert(fullpath, Datas.ParentNodeSite + "/" + NodeNametxb.Text , "", "NodeType", NodetypeCmb.Text);//设置属性值
            }
            else
            {
                string str = Guid.NewGuid().ToString();
                FileHelper.CreateDirectory(fullpath.Substring(0, fullpath.LastIndexOf("\\") + 1) + NodeNametxb.Text);
                XmlHelper.Insert(fullpath, Datas.ParentNodeSite, NodeNametxb.Text, "Name", NodeNametxb.Text);//添加节点
                XmlHelper.Insert(fullpath, Datas.ParentNodeSite + "/" + NodeNametxb.Text, "", "NodeType", NodetypeCmb.Text);//设置属性值
            }
            this.Close();
        }

        private void canclebtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
