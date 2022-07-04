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
using ManageTreeDemo.ViewModel;
using Microsoft.Win32;
using ManageTreeDemo.Helpers;
using ManageTreeDemo.Model;
using System.Xml;
using ManageTreeDemo.Windows;

namespace ManageTreeDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MainTreeViewVM _maintreeVM;

        public static string fullpath = null;
        public MainWindow()
        {
            _maintreeVM = new MainTreeViewVM(this);
            InitializeComponent();
            this.DataContext = _maintreeVM;
        }

        /// <summary>
        /// 菜单—打开工程点击事件
        /// 文件工程名与根节点标签名一致
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenProgram_Click(object sender, RoutedEventArgs e)
        {
            _maintreeVM.OpenProgram_Click();
        }

        /// <summary>
        /// 菜单-新建工程点击事件
        /// 文件工程名与根节点标签名一致
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProgram_Click(object sender, RoutedEventArgs e)
        {
            _maintreeVM.NewProgram_Click();
        }

        private void RefTest_Click(object sender, RoutedEventArgs e)
        {
            //_maintreeVM.RefTest();//添加节点测试
            _maintreeVM.RefTree();
        }


        private void MainTreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject source = e.OriginalSource as DependencyObject;
            while (source != null && source.GetType() != typeof(TreeViewItem))
                source = System.Windows.Media.VisualTreeHelper.GetParent(source);
            if (source != null)
            {
                TreeViewItem item = source as TreeViewItem;
                item.Focus();
                e.Handled = true;
                _maintreeVM._selectItem = item;
            }
        }

        private void Renametxb_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox texbox = sender as TextBox;
            texbox.Visibility = Visibility.Collapsed;
            XmlHelper.Update(MainTreeViewVM.fullpath, (_maintreeVM._selectItem.Header as Node).Site, "Name", texbox.Text);
            _maintreeVM.RefTree();
        }

        //private Node getxmlNodes(string xmlpath,string xmlsite)
        //{
        //    Node node = new Node();
        //    XmlDocument doc = new XmlDocument();
        //    XmlReaderSettings settings = new XmlReaderSettings();
        //    settings.IgnoreComments = true;//忽略xml中注释
        //    XmlReader reader = XmlReader.Create(xmlpath, settings);
        //    doc.Load(reader);
        //    XmlNode xmlnode = doc.SelectSingleNode(xmlsite);
        //    XmlElement xmlElement = (XmlElement)xmlnode;
        //    node.NodeName = xmlElement.GetAttribute("Name").ToString();
        //    //if (xmlsite == System.IO.Path.GetFileNameWithoutExtension(fullpath))
        //    //    node.Site = xmlsite;
        //    //else
        //    //    node.Site = xmlsite + "/" + node.NodeName;
        //    node.Site = xmlsite;
        //    foreach (XmlNode childnode in xmlnode)
        //    {
        //        XmlElement childnodeElement = (XmlElement)childnode;
        //        //node.ChildNodes.Add(getxmlNodes(xmlpath, node.Site+"/" +childnodeElement.GetAttribute("Name" ).ToString()));
        //        node.ChildNodes.Add(getxmlNodes(xmlpath, node.Site + "/" + childnode.Name.ToString()));
        //    }
        //    return node;
        //}
    }
}
