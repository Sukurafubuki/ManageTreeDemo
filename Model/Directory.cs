using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ManageTreeDemo.Model
{
    /// <summary>
    /// 文件夹实体类
    /// </summary>
    public class Directory:Node
    {
        #region 构造函数
        public Directory()
        {
            this.NodeID = Guid.NewGuid().ToString();
            this.ChildNodes = new ObservableCollection<Node>();
            //拼接图标路径，param1为debug下的exe所在路径
            this.NodeIconPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\Model\Icons\directory_icon.png");
            //this.NodeType = NodeType.testNode;
        }

        public Directory(string name):base(name)
        {
            this.NodeID = Guid.NewGuid().ToString();
            this.NodeName = name;
            this.ChildNodes = new ObservableCollection<Node>();
            //拼接图标路径，param1为debug下的exe所在路径
            this.NodeIconPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\Model\Icons\dir_icon.png");
        }
        #endregion

        #region 节点内容
        /// <summary>
        /// 文件夹物理路径
        /// </summary>
        public override string NodeContent { get; set; }
        #endregion
    }
}
