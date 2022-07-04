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
            XmlHelper.Insert(fullpath, Datas.ParentNodeSite, "Name", NodeNametxb.Text);
            this.Close();
        }

        private void canclebtn_Click(object sender, RoutedEventArgs e)
        {
            ///testtesttesttesttesttest
            this.Close();
        }
    }
}
