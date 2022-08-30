using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageTreeDemo.Common;
using System.Collections.ObjectModel;
using ManageTreeDemo.Model;
using Microsoft.Win32;
using ManageTreeDemo.Helpers;
using System.Windows;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Input;
using ManageTreeDemo.Windows;
using System.Windows.Media;
using ManageTreeDemo.UserControls;
using ManageTreeDemo.UserControls.Views;

namespace ManageTreeDemo.ViewModel
{
    public class MainTreeViewVM : NotifyPropertyBase
    {
        #region commad及执行事件
        /// <summary>
        /// 命令要执行的方法
        /// </summary>
        void AddExecute(object sender)
        {
            if (_selectItem != null)
            {
                //MessageBox.Show(_selectItem.NodeName);
                //XmlHelper.Insert(fullpath, _selectItem.Site, "Name", "test");
                AddNodeWindow _addnodewindow = new AddNodeWindow(fullpath, _selectItem.Header as Node);
                _addnodewindow.Owner = mainwindow;
                _addnodewindow.ShowDialog();
                RefTree();
            }
        }
        /// <summary>
        /// 删除命令触发事件
        /// </summary>
        /// <param name="sender"></param>
        void DeleteExecute(object sender)
        {
            if (_selectItem != null)
            {
                //MessageBox.Show(_selectItem.NodeName);
                XmlHelper.Delete(fullpath, (_selectItem.Header as Node).Site, "");
                RefTree();
            }
        }
        /// <summary>
        /// 重命名命令触发事件
        /// </summary>
        /// <param name="sender"></param>
        void ReNameExecute(object sender)
        {
            if (_selectItem != null)
            {
                //MessageBox.Show("ReNameExecute");
                selecttxb = FindVisualChild<TextBox>(_selectItem as DependencyObject);
                selecttxb.Visibility = Visibility.Visible;
                selecttxb.SelectAll();
                selecttxb.Focus();
            }
        }
        /// <summary>
        /// 复制命令触发事件
        /// </summary>
        /// <param name="sender"></param>
        void CopyExecute(object sender)
        {
            if (_selectItem != null)
            {
                //MessageBox.Show("CopyExecute");
                //复制选择节点对象到剪切板（mainwindow静态变量）
                TreeNodeEdit.NodeCopy(_selectItem.Header as Node);
            }
        }
        /// <summary>
        /// 粘贴命令触发事件
        /// </summary>
        /// <param name="sender"></param>
        void PasteExecute(object sender)
        {
            if (_selectItem != null)
            {
                //MessageBox.Show("PasteExecute");
                //粘贴剪切板clipper对象中的节点
                //递归插入
                TreeNodeEdit.NodePaste(_selectItem.Header as Node);
                RefTree();
            }
        }
        /// <summary>
        /// 寻找视觉树中的树节点
        /// </summary>
        /// <typeparam name="childItem"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        /// <summary>
        /// 命令是否可以执行
        /// </summary>
        /// <returns></returns>
        bool CanUpdateNameExecute()
        {
            return true;
        }
        /// <summary>
        /// 创建新命令
        /// </summary>
        public ICommand _addAction
        {
            get
            {
                return new RelayCommand<object>(AddExecute, CanUpdateNameExecute);
            }
        }
        public ICommand _delAction
        {
            get
            {
                return new RelayCommand<object>(DeleteExecute, CanUpdateNameExecute);
            }
        }
        public ICommand _rNameAction
        {
            get
            {
                return new RelayCommand<object>(ReNameExecute, CanUpdateNameExecute);
            }
        }
        public ICommand _copyAction
        {
            get
            {
                return new RelayCommand<object>(CopyExecute, CanUpdateNameExecute);
            }
        }
        public ICommand _pasteAction
        {
            get
            {
                return new RelayCommand<object>(PasteExecute, CanUpdateNameExecute);
            }
        }
        #endregion

        #region 属性
        #region 视图树数据实体
        /// <summary>
        /// 视图树数据实体
        /// </summary>
        public ObservableCollection<Node> MainTrees
        {
            get;
            set;
        }
        public ObservableCollection<TreeViewItem> treeViewItems;
        /// <summary>
        /// 主窗体实例，弹出窗体父窗体对象
        /// </summary>
        public MainWindow mainwindow;
        /// <summary>
        /// 选择节点的textbox
        /// </summary>
        public TextBox selecttxb;

        #region 树菜单数据实体
        /// <summary>
        /// 视图树数据实体
        /// </summary>
        public ObservableCollection<MenuItem> MenuItems
        {
            get;
            set;
        }
        #endregion

        #endregion
        /// <summary>
        /// xml路径
        /// </summary>
        public static string fullpath = null;
        /// <summary>
        /// 右键选择树节点
        /// </summary>
        public TreeViewItem _selectItem
        {
            get;
            set;
        }

        private MyTabcontrol _nodeTabs = new MyTabcontrol();
        /// <summary>
        /// 节点详情，绑定用户控件
        /// </summary>
        public MyTabcontrol NodeTabs { get { return _nodeTabs; } set { _nodeTabs = value;OnPropertyChanged("_nodeTabs"); } }
        #endregion

        #region 构造函数
        /// <summary>
        /// TreeviewVM构造函数
        /// </summary>
        public MainTreeViewVM(MainWindow _mainWindow)
        {
            mainwindow = _mainWindow;
            MainTrees = new ObservableCollection<Node>();
            MenuItems = new ObservableCollection<MenuItem>();
            CreateMenu();
        }

        /// <summary>
        /// 生成右键菜单
        /// </summary>
        /// <returns></returns>
        public void CreateMenu()
        {
            MenuItem _myItem = new MenuItem();
            MenuItem AddItem = new MenuItem();
            AddItem.Header = "添加";
            MenuItems.Add(AddItem);
            ///添加
            MenuItem _addItem = new MenuItem();
            _addItem.Header = "添加1";
            _addItem.Command = _addAction;
            AddItem.Items.Add(_addItem);
            ///删除
            MenuItem _delItem = new MenuItem();
            _delItem.Header = "删除";
            _delItem.Command = _delAction;
            MenuItems.Add(_delItem);
            ///重命名
            ///
            MenuItem _rNameItem = new MenuItem();
            _rNameItem.Header = "重命名";
            _rNameItem.Command = _rNameAction;
            MenuItems.Add(_rNameItem);

            ///复制
            ///
            MenuItem _copyItem = new MenuItem();
            _copyItem.Header = "复制";
            _copyItem.Command = _copyAction;
            MenuItems.Add(_copyItem);
            ///粘贴
            ///
            /////////////////////////////////////////////粘贴item.灰度=剪切板==null? 可选:不可选
            MenuItem _pasteItem = new MenuItem();
            _pasteItem.Header = "粘贴";
            _pasteItem.Command = _pasteAction;
            MenuItems.Add(_pasteItem);
        }
        #endregion

        #region 填充数据（测试用）
        public Node TestCreateTree()
        {
            Node _myT = new Node("中国");
            #region 河南
            Node _myBJ = new Node("河南");
            _myT.CreateTreeWithChildre(_myBJ);
            Node _HD = new Node("郑州");


            Node _CY = new Node("鹤壁");
            Node _FT = new Node("新乡");
            Node _DC = new Node("洛阳");

            _myBJ.CreateTreeWithChildre(_HD);
            _HD.CreateTreeWithChildre(new Node("某某1"));
            _HD.CreateTreeWithChildre(new Node("某某2"));
            _myBJ.CreateTreeWithChildre(_CY);
            _myBJ.CreateTreeWithChildre(_FT);
            _myBJ.CreateTreeWithChildre(_DC);

            #endregion

            #region 陕西
            Node _myHB = new Node("陕西");
            _myT.CreateTreeWithChildre(_myHB);
            Node _mySJZ = new Node("西安");
            Node _mySD = new Node("渭南");

            Directory _myTS = new Directory("汉中");

            _myHB.CreateTreeWithChildre(_mySJZ);
            _myHB.CreateTreeWithChildre(_mySD);
            _myHB.CreateTreeWithChildre(_myTS);
            #endregion

            return _myT;
        }
        #endregion

        #region 下拉菜单刷新事件
        /// <summary>
        /// 刷新树
        /// </summary>
        public void RefTree()
        {
            //MainTrees[0].CreateTreeWithChildre(new Node("testtesttest"));界面更新，VM绑定测试
            if (string.IsNullOrEmpty(fullpath))
            { }
            else
            {
                Node _mainTree = XmlHelper.GetXmlTreeByRecursion<Node>(fullpath, System.IO.Path.GetFileNameWithoutExtension(fullpath), false);
                MainTrees.Clear();
                MainTrees.Add(_mainTree);
            }
        }
        #endregion

        #region 菜单—打开工程点击事件
        /// <summary>
        /// 菜单—打开工程点击事件
        /// 文件工程名与根节点标签名一致
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenProgram_Click()
        {
            try
            {
                OpenFileDialog opendlg = new OpenFileDialog();
                opendlg.Filter = "XML(.xml)|*.xml";
                //open.Filter="word Files(*.doc)|*.doc|All Files(*.*)|*.*";
                opendlg.RestoreDirectory = true;
                while (opendlg.ShowDialog() == true)
                {
                    fullpath = opendlg.FileName;
                    if (string.IsNullOrEmpty(fullpath))
                        throw new Exception("打开失败");
                    //Node MainTree = getxmlNodes(fullpath, System.IO.Path.GetFileNameWithoutExtension(fullpath));
                    Node _mainTree = XmlHelper.GetXmlTreeByRecursion<Node>(fullpath, System.IO.Path.GetFileNameWithoutExtension(fullpath), false);
                    MainTrees.Clear();
                    MainTrees.Add(_mainTree);
                    return ;
                }
                MessageBox.Show("未选择有效文件");
            }
            catch (Exception ex)
            {
                //throw new Exception("", ex);
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 菜单-新建工程点击事件
        /// <summary>
        /// 菜单-新建工程点击事件
        /// 文件工程名与根节点标签名一致
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewProgram_Click()
        {
            try
            {
                SaveFileDialog savedlg = new SaveFileDialog();
                savedlg.Filter = "XML(.xml)|*.xml";//"XML(.xml)|*.xml|All Files(*.*)|*.*"
                savedlg.FileName = "MyProgram";//默认名
                //savedlg.DefaultExt = "xml";//默认拓展名
                savedlg.RestoreDirectory = true;
                while (savedlg.ShowDialog() == true)
                {
                    fullpath = savedlg.FileName;//获得文件路径
                    System.IO.FileStream fs = (System.IO.FileStream)savedlg.OpenFile();//输出文件，fs可以用于其他要写入的操作
                    FileHelper.CreateFile(fullpath);
                    XmlDocument xmlDocument = new XmlDocument();
                    XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "");
                    xmlDocument.AppendChild(xmlDeclaration);
                    XmlElement program = xmlDocument.CreateElement(System.IO.Path.GetFileNameWithoutExtension(fullpath));//元素名
                    xmlDocument.AppendChild(program);
                    program.SetAttribute("Name", System.IO.Path.GetFileNameWithoutExtension(fullpath));
                    program.SetAttribute("NodeType", NodeType.Normal.ToString());
                    ///************************//
                    xmlDocument.Save(fs);
                    fs.Close();
                    //if (string.IsNullOrEmpty(fullpath))
                    //    throw new Exception("打开失败");
                    //Node MainTree = getxmlNodes(fullpath, System.IO.Path.GetFileNameWithoutExtension(fullpath));
                    Node _mainTree = XmlHelper.GetXmlTreeByRecursion<Node>(fullpath, System.IO.Path.GetFileNameWithoutExtension(fullpath), false);
                    MainTrees.Clear();
                    MainTrees.Add(_mainTree);
                    return;
                }
                MessageBox.Show("未创建有效文件");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


        #region 双击节点事件
        public void NoedDouble_Click(Node _node)
        {
            //_nodeDetail = new NodeDetails(_node);
            NodeTabs.MyTabcontrolVM.OpenNode(_node);
        }
        #endregion
    }
}
