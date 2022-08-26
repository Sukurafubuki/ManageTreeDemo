using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageTreeDemo.Common;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;

namespace ManageTreeDemo.Model
{
    public class StringToImageSourceConverter : IValueConverter
    {
        #region Converter

        /// <summary>
        /// 路径字符串转换为路径（界面图标路径绑定字符用）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;
            if (!string.IsNullOrEmpty(path))
            {
                return new BitmapImage(new Uri(path, UriKind.Absolute));
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
    /// <summary>
    /// Node实体类
    /// </summary>
    public class Node : NotifyPropertyBase
    {
        #region 构造函数
        public Node()
        {
            //this.NodeID = Guid.NewGuid().ToString();
            this.ChildNodes = new ObservableCollection<Node>();
            //拼接图标路径，param1为debug下的exe所在路径
            //this.NodeIconPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\Model\Icons\node_icon.png");
            this.NodeIconPath = @"pack://application:,,,/Model/Icons/node_icon.png";
            //this.NodeType = NodeType.testNode;
        }

        public Node(string name)
        {
            //this.NodeID = Guid.NewGuid().ToString();
            this.NodeName = name;
            this.ChildNodes = new ObservableCollection<Node>();
            //this.NodeIconPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\Model\Icons\node_icon.png");
            this.NodeIconPath = @"pack://application:,,,/Model/Icons/node_icon.png";
        }
        #endregion

        #region 节点信息
        #region 节点ID
        /// <summary>
        /// 节点ID
        /// </summary>
        private string _nodeId ;
        public string NodeID
        {
            get
            {
                return _nodeId;
            }
            set
            {
                _nodeId = value;
            }
        }
        #endregion

        #region 节点图标
        /// <summary>
        /// 节点图标
        /// </summary>
        public string NodeIconPath { get; set; }
        #endregion

        #region 节点名称
        /// <summary>
        ///  节点名称
        /// </summary>
        public string NodeName { get; set; }
        #endregion

        #region 节点内容
        /// <summary>
        /// 节点携带的内容
        /// </summary>
        public virtual string NodeContent { get; set; }
        #endregion

        #region 节点类型
        private NodeType _nodetype;
        /// <summary>
        /// 节点类型（区分文件夹，变量，设备等类型）
        /// </summary>
        public NodeType NodeType
        {
            get
            {
                return _nodetype;
            }
            set
            {
                _nodetype = value;
            }
        }
        #endregion

        #endregion

        #region 树结构
        #region 父节点
        /// <summary>
        /// 节点父节点
        /// </summary>
        public Node ParentNode { get; set; }
        #endregion

        #region 子节点
        /// <summary>
        /// 节点子节点
        /// </summary>
        public ObservableCollection<Node> ChildNodes { get; set; }
        #endregion

        #region 节点站点信息
        private string _site;
        /// <summary>
        /// 节点站点信息
        /// </summary>
        public string Site
        {
            get
            {
                return _site;
            }
            set
            {
                _site = value;
            }
        }
        #endregion
        #endregion

        #region 绑定父子节点（测试填充树）
        /// <summary>
        /// 绑定父子节点
        /// </summary>
        /// <param name="children">子节点</param>
        public void CreateTreeWithChildre(Node children)
        {
            this.ChildNodes.Add(children);
            children.ParentNode = this;
        }
        #endregion
    }

    //public class Node_xmlhelper
    //{
    //    public static
    //}
}
