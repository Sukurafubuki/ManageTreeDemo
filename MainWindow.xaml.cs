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
        Point _lastMouseDown;
        Node moveTreeItem;

        /// <summary>
        /// 剪切板
        /// </summary>
        public static Node Clipper { get; set; }

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

        private void MainTreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _lastMouseDown = e.GetPosition(MainTreeView);
        }

        private void MainTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            TreeView _treeview = sender as TreeView;
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(MainTreeView);
                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 2.0) || (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 2.0))
                    {
                        moveTreeItem = (Node)_treeview.SelectedItem;
                        if (moveTreeItem != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(MainTreeView, MainTreeView.SelectedValue, DragDropEffects.Move);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void MainTreeView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (moveTreeItem == null)
            {
                return;
            }
            var ele = e.OriginalSource as FrameworkElement;
            if (ele != null)
            {
                var targetTreeItem = (Node)ele.DataContext;
                if (targetTreeItem == null)
                {
                    
                }
            }
        }


        private void MainTreeView_CheckDropTarget(object sender, DragEventArgs e)
        {
            if (e.Source as TreeViewItem != null)
            {
            }
            e.Handled = true;
        }


        private bool IsValidDropTarget(object id)
        {
            bool res = false;
            if (id != null)
            {
                //拖拽条件
            }
            return res;
        }

        private void MainTreeView_Drop(object sender, DragEventArgs e)
        {
            XmlHelper.Delete(MainTreeViewVM.fullpath, moveTreeItem.Site, "");
            //TreeViewItem treeViewItemParent = new TreeViewItem();
            //if (e.Source as TreeViewItem != null)
            //{
            //    treeViewItemParent = e.Source as TreeViewItem;//().Parent as TreeViewItem
            //    if (MainTreeView.SelectedItem as TreeViewItem == (e.Source as TreeViewItem))
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    treeViewItemParent = e.Source as TreeViewItem;
            //    if (MainTreeView.SelectedItem as TreeViewItem == e.Source as TreeViewItem)
            //    {
            //        return;
            //    }
            //}
            ////进行增加删除功能
            //TreeViewItem itemRemoved = MainTreeView.SelectedItem as TreeViewItem;
            //CusRequireInfo cusRequireInfo = BLLCusRequire.GetModel(Convert.ToInt32(itemRemoved.Tag));
            //if (cusRequireInfo.ParentId == 0)
            //{
            //    MainTreeView.Items.Remove(itemRemoved);
            //}
            //else
            //{
            //    (itemRemoved.Parent as TreeViewItem).Items.Remove(itemRemoved);
            //}
            //(treeViewItemParent).Items.Add(itemRemoved);
            //cusRequireInfo.ParentId = Convert.ToInt32(treeViewItemParent.Tag);
            //BLLCusRequire.Update(cusRequireInfo);
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
