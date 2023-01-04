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

namespace ManageTreeDemo.UserControls.Canvas
{
    /// <summary>
    /// 可拖动控件基础类
    /// </summary>
    public partial class BaseControl : UserControl
    {
        #region 属性字段

        private Size size;
        /// <summary>
        /// 尺寸
        /// </summary>
        public Size Size{ get; set; }


        private List<Point> alignPoints = new List<Point>();
        /// <summary>
        /// 可对齐点集
        /// </summary>
        public List<Point> AlignPoints { get; set; }
        #endregion



        /// <summary>
        ///构造函数
        /// </summary>
        public BaseControl()
        {
            InitializeComponent();
        }


    }
}
