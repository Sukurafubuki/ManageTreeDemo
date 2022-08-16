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
using ManageTreeDemo.Common;

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
        bool IsDrop = false;
        public static clipper Clipper = new clipper(false);

        /// <summary>
        /// 剪切板
        /// </summary>
        public struct  clipper
        {
            #region 属性字段Property
            /// <summary>
            /// 剪切板数据
            /// </summary>
            public Node Node { get; set; }
            /// <summary>
            /// 是否剪切:true粘贴时删除源数据，false不删除
            /// </summary>
            public bool IsCut { get; set; }
            #endregion
            #region 构造方法Constructors
            /// <summary>
            /// 全赋值构造方法
            /// </summary>
            /// <param name="_node"></param>
            /// <param name="_iscut"></param>
            public clipper(Node _node, bool _iscut)
            {
                Node = _node;
                IsCut = _iscut;
            }
            /// <summary>
            /// 初始化构造方法
            /// </summary>
            /// <param name="_iscut"></param>
            public clipper(bool _iscut)
            {
                Node = null;
                IsCut = _iscut;
            }
            #endregion
        }
        
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
            _maintreeVM._selectItem = MainTreeView.SelectedItem as TreeViewItem;
            var ele = e.OriginalSource as FrameworkElement;
            if (ele != null)
            {
                //判断触发控件
                if (ele.GetType() == typeof(System.Windows.Controls.TextBlock) || ele.GetType() == typeof(System.Windows.Controls.Image))
                {
                    _lastMouseDown = e.GetPosition(MainTreeView);
                    moveTreeItem = (Node)ele.DataContext;
                }
            }
        }

        private void MainTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(MainTreeView);
                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 2.0) || (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 2.0))
                    {
                        if (!IsDrop)
                        {
                            if (moveTreeItem != null)
                            {
                                IsDrop = true;
                                this.MainTreeView.Cursor = Cursors.Hand;
                                //DragDropEffects finalDropEffect = DragDrop.DoDragDrop(MainTreeView, MainTreeView.SelectedValue, DragDropEffects.Move);
                            }
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
                //判断触发控件
                if (ele.GetType() == typeof(System.Windows.Controls.TextBlock) || ele.GetType() == typeof(System.Windows.Controls.Image))
                {
                    var targetTreeItem = (Node)ele.DataContext;
                    if (targetTreeItem == null)
                    {
                        //还原拖拽信号量
                        ClearDrops();
                        return;
                    }
                    if (targetTreeItem.NodeName != moveTreeItem.NodeName)
                    {
                        //移动节点
                        //moveTreeItem.ParentNode.ChildNodes.Remove(moveTreeItem);
                        //targetTreeItem.ChildNodes.Add(moveTreeItem);
                        //moveTreeItem.ParentNode = targetTreeItem;
                        #region 移动节点
                        //删除源节点
                        XmlHelper.Delete(MainTreeViewVM.fullpath,moveTreeItem.Site,"");
                        moveTreeItem.ParentNode = targetTreeItem;
                        XmlHelper.InsertByRecursion(MainTreeViewVM.fullpath, targetTreeItem.Site, moveTreeItem);
                        _maintreeVM.RefTree();
                        #endregion
                    }
                }
            }
            ClearDrops();
        }

        /// <summary>
        /// 还原拖拽信号量
        /// </summary>
        private void ClearDrops()
        {
            moveTreeItem = null;
            IsDrop = false;
            this.MainTreeView.Cursor = Cursors.Arrow;
        }

        private void MainTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as TreeView).SelectedItem != null)
            {
                _maintreeVM.NoedDouble_Click((sender as TreeView).SelectedItem as Node);
            }
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
