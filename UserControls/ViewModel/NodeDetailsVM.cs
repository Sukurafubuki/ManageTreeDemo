using ManageTreeDemo.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ManageTreeDemo.Model;
using System.Reflection;

namespace ManageTreeDemo.UserControls.ViewModel
{
    /// <summary>
    /// 节点详情VM
    /// </summary>
    public class NodeDetailsVM: NotifyPropertyBase
    {
        /// <summary>
        /// 节点详情
        /// </summary>
        public ObservableCollection<TextBlock> TextBlocks { get; set; }

        #region construction
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_node"></param>
        public NodeDetailsVM(Node _node)
        {
            TextBlocks = new ObservableCollection<TextBlock>();
            PropertyInfo[] properties = typeof(Node).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if(prop.GetValue(_node)!=null)
                {
                    TextBlock txtblk = new TextBlock();
                    txtblk.Text += prop.Name;
                    txtblk.Text += " : " + prop.GetValue(_node);
                    txtblk.FontSize = 20;
                    txtblk.Margin = new System.Windows.Thickness(20, 20, 20, 20);
                    TextBlocks.Add(txtblk);
                }
            }
        }
        #endregion
    }
}
