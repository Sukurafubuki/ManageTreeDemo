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
using ManageTreeDemo.UserControls.ViewModel;
using ManageTreeDemo.Model;

namespace ManageTreeDemo.UserControls.Views
{
    /// <summary>
    /// MyTabcontrol.xaml 的交互逻辑
    /// </summary>
    public partial class MyTabcontrol : UserControl
    {
        #region 选项卡拖拽相关
        //初始位置
        Point _lastMouseDown;
        //所移动节点
        MyTabItemWithClose moveTabItem;
        //移动信号量
        bool IsDrop = false;
        #endregion

        private MyTabcontrolVM _myTabcontrolVM;
        public MyTabcontrolVM MyTabcontrolVM
        { 
            get { return _myTabcontrolVM; }
            set { _myTabcontrolVM = value; } 
        }

        public MyTabcontrol()
        {
            InitializeComponent();
            _myTabcontrolVM = new MyTabcontrolVM(this);
            this.DataContext = _myTabcontrolVM;
        }

        #region 选项卡拖拽
        private void TabControl1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ele = e.OriginalSource as FrameworkElement;
            if (ele != null)
            {
                //视觉树获取触发的mytabitemwithclose控件
                DependencyObject source = e.OriginalSource as DependencyObject;
                while (source != null && source.GetType() != typeof(MyTabItemWithClose) && source.GetType() != typeof(MyTabcontrol))
                    source = System.Windows.Media.VisualTreeHelper.GetParent(source);
                //仅当触发控件为合理控件时，做拖动准备
                if (source.GetType() == typeof(MyTabItemWithClose))
                {
                    _lastMouseDown = e.GetPosition(TabControl1);
                    //视觉树获取触发的mytabitemwithclose控件
                    moveTabItem = source as MyTabItemWithClose;
                }
            }
        }

        private void TabControl1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(TabControl1);
                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 2.0) || (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 2.0))
                    {
                        if (!IsDrop)
                        {
                            if (moveTabItem != null)
                            {
                                IsDrop = true;
                                this.TabControl1.Cursor = Cursors.Hand;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void TabControl1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (moveTabItem == null)
            {
                return;
            }
            var ele = e.OriginalSource as FrameworkElement;
            if (ele != null)
            {
                //视觉树获取触发的mytabitemwithclose控件
                DependencyObject source = e.OriginalSource as DependencyObject;
                while (source != null && source.GetType() != typeof(MyTabItemWithClose) && source.GetType() != typeof(MyTabcontrol))
                    source = System.Windows.Media.VisualTreeHelper.GetParent(source);
                if (source.GetType() == typeof(MyTabItemWithClose))
                {
                    var targetTabitem = source as MyTabItemWithClose;
                    if (targetTabitem != moveTabItem)
                    {
                        #region 移动选项卡
                        MyTabcontrolVM.TabItems.Remove(moveTabItem);
                        MyTabcontrolVM.TabItems.Insert(TabControl1.Items.IndexOf(targetTabitem), moveTabItem);
                        MyTabcontrolVM.OpenedNodes.Remove(moveTabItem.Item_Node);
                        MyTabcontrolVM.OpenedNodes.Insert(MyTabcontrolVM.OpenedNodes.IndexOf(targetTabitem.Item_Node), moveTabItem.Item_Node);
                        TabControl1.SelectedIndex = MyTabcontrolVM.OpenedNodes.IndexOf(targetTabitem.Item_Node) - 1;
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
            moveTabItem = null;
            IsDrop = false;
            this.TabControl1.Cursor = Cursors.Arrow;
        }
        #endregion
    }
}
