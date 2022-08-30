using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace ManageTreeDemo.UserControls.Canvas
{
    public class MyBaseThumb : Thumb
    {
        /// <summary>
        /// 是否可以输入
        /// </summary>
        public bool IsEnableInput { get { return (bool)GetValue(IsEnableInputProperty); } set { SetValue(IsEnableInputProperty, value); } }

        /// <summary>
        /// 依赖属性
        /// </summary>
        public static readonly DependencyProperty IsEnableInputProperty = DependencyProperty.Register("IsEnableInput", typeof(bool), typeof(MyBaseThumb));

        public MyBaseThumb() : base()
        {
            this.DragStarted += ThumbControl_DragStarted;
            this.DragDelta += ThumbControl_DragDelta;
            this.DragCompleted += ThumbControl_DragCompleted;
            this.MouseDoubleClick += ThumbControl_MouseDoubleClick;
            this.IsKeyboardFocusedChanged += ThumbControl_IsKeyboardFocusedChanged;
        }

        public void SetIsEnableInput(bool flag)
        {
            this.IsEnableInput = flag;
        }

        /// <summary>
        /// 是否具有键盘焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThumbControl_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            this.IsEnableInput = this.IsKeyboardFocused;

        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThumbControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.IsEnableInput = true;
        }

        private void ThumbControl_DragStarted(object sender, DragStartedEventArgs e)
        {
            //开始移动
        }

        private void ThumbControl_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb myThumb = (Thumb)sender;
            double nTop = System.Windows.Controls.Canvas.GetTop(myThumb) + e.VerticalChange;
            double nLeft = System.Windows.Controls.Canvas.GetLeft(myThumb) + e.HorizontalChange;
            System.Windows.Controls.Canvas.SetTop(myThumb, nTop);
            System.Windows.Controls.Canvas.SetLeft(myThumb, nLeft);
        }

        private void ThumbControl_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            //移动结束
        }
    }
}
