using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ManageTreeDemo.UserControls.Canvas
{
    public class MyCanvas:System.Windows.Controls.Canvas
    {

        #region constructions
        /// <summary>
        /// 左键选择起始点
        /// </summary>
        Point selectionStart = default;
        /// <summary>
        /// 构造函数
        /// </summary>
        public MyCanvas()
        { }
        #endregion

        #region 单击选中项处理
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            if (visualAdded is Control ctrl)
            {
                ctrl.PreviewMouseLeftButtonDown += Ctrl_MouseLeftButtonDown;
            }
            if (visualRemoved is Control ctr)
            {
                ctr.PreviewMouseLeftButtonDown -= Ctrl_MouseLeftButtonDown;
            }
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        private void Ctrl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Control ctl)
            {
                var cp = GetParentObject<MyCanvas>(ctl);
                cp.SelectedItems = new ObservableCollection<Control>() { ctl };
            }
        }

        public static T GetParentObject<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
        #endregion

        #region 框选
        public ObservableCollection<Control> SelectedItems
        {
            get { return (ObservableCollection<Control>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<Control>), typeof(MyCanvas), new PropertyMetadata(null, OnSelectedItemsChanged));

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as MyCanvas)?.RefreshSelection();

        private void RefreshSelection()
        {
            foreach (var item in Children)
            {
                var ele = item as Control;
                if (ele == null) continue;
                var layer = AdornerLayer.GetAdornerLayer(ele);
                var arr = layer.GetAdorners(ele);//获取该控件上所有装饰器，返回一个数组
                if (arr != null)
                {
                    for (int i = arr.Length - 1; i >= 0; i--)
                    {
                        layer.Remove(arr[i]);
                    }
                }
            }

            if (SelectedItems != null)
            {
                foreach (var item in SelectedItems)
                {
                    var layer = AdornerLayer.GetAdornerLayer(item);
                    layer.Add(new SelectionAdorner(item));
                }
            }
        }

        /// <summary>
        /// 计算框选项
        /// </summary>
        private void FrameSelection(double x, double y, double width, double height)
        {
            SelectedItems = new ObservableCollection<Control>();
            foreach (var item in Children)
            {
                if (item is Control ctrl)
                {
                    var left = GetLeft(ctrl);
                    var top = GetTop(ctrl);
                    if (left >= x && left <= x + width && top >= y && top <= y + height)
                    {
                        SelectedItems.Add(ctrl);
                    }
                }
            }
            RefreshSelection();
        }
        #endregion
        #region 绘制选择框

        Border selectionBorder = new Border()
        {
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#637DB7F4")),
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBBBBBB")),
        };
        /// <summary>
        /// 重写左键点击
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.Children.Add(selectionBorder);
            selectionStart = e.GetPosition(this);
            this.CaptureMouse();
        }
        /// <summary>
        /// 重写鼠标移动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var nowPoint = e.GetPosition(this);
                //获取移动矢量
                var offsetX = nowPoint.X - selectionStart.X;
                var offsetY = nowPoint.Y - selectionStart.Y;
                Clear();

                selectionBorder.Width = Math.Abs(offsetX);
                selectionBorder.Height = Math.Abs(offsetY);
                // 分四种情况绘制
                if (offsetX >= 0 && offsetY >= 0)// 右下
                {
                    SetLeft(selectionBorder, selectionStart.X);
                    SetTop(selectionBorder, selectionStart.Y);
                }
                else if (offsetX > 0 && offsetY < 0)// 右上
                {
                    SetLeft(selectionBorder, selectionStart.X);
                    SetBottom(selectionBorder, ActualHeight - selectionStart.Y);
                }
                else if (offsetX < 0 && offsetY > 0)// 左下
                {
                    SetRight(selectionBorder, ActualWidth - selectionStart.X);
                    SetTop(selectionBorder, selectionStart.Y);
                }
                else if (offsetX < 0 && offsetY < 0)// 左上
                {
                    SetRight(selectionBorder, ActualWidth - selectionStart.X);
                    SetBottom(selectionBorder, ActualHeight - selectionStart.Y);
                }
            }
        }
        /// <summary>
        /// 重写鼠标左键弹起
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            //计算选择框坐标
            if (double.IsNaN(GetLeft(selectionBorder)))
            {
                SetLeft(selectionBorder, ActualWidth - GetRight(selectionBorder) - selectionBorder.ActualWidth);
            }
            if (double.IsNaN(GetTop(selectionBorder)))
            {
                SetTop(selectionBorder, ActualHeight - GetBottom(selectionBorder) - selectionBorder.ActualHeight);
            }
            FrameSelection(GetLeft(selectionBorder), GetTop(selectionBorder), selectionBorder.Width, selectionBorder.Height);
            selectionBorder.Width = 0;
            selectionBorder.Height = 0;
            //移除选择框
            this.Children.Remove(selectionBorder);
            this.ReleaseMouseCapture();
        }
        /// <summary>
        /// 清除选择框
        /// </summary>
        private void Clear()
        {
            SetLeft(selectionBorder, double.NaN);
            SetRight(selectionBorder, double.NaN);
            SetTop(selectionBorder, double.NaN);
            SetBottom(selectionBorder, double.NaN);
        }
        #endregion

        #region 拖动对齐
        /// <summary>
        /// 拖动处理
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void MoveControls(int offsetX, int offsetY)
        {
            ClearAlignLine();
            // 获取可对齐的点
            List<Point> points = new List<Point>();
            //foreach (Control ctrl in Children)
            //{
            //    if (!SelectedItems.Contains(ctrl))
            //    {
            //        Point item = new Point(GetLeft(ctrl), GetTop(ctrl));
            //        points.Add(item);
            //    }
            //}
            foreach (UIElement UIele in Children)
            {
                if (!SelectedItems.Contains(UIele))
                {
                    Point item = new Point(GetLeft(UIele), GetTop(UIele));
                    points.Add(item);
                }
            }
            foreach (var item in SelectedItems)
            {
                SetLeft(item, GetLeft(item) + offsetX);
                SetTop(item, GetTop(item) + offsetY);

                // 计算是否显示对齐线
                var lefAlign = points.FirstOrDefault(x => Math.Abs(x.X - GetLeft(item)) <= 2);
                if (lefAlign != default)
                {
                    SetLeft(item, lefAlign.X);
                    var layer = AdornerLayer.GetAdornerLayer(this);
                    layer.Add(new MySelectionAlignLine(this, lefAlign, new Point(GetLeft(item), GetTop(item))));
                }

                var topAlign = points.FirstOrDefault(x => Math.Abs(x.Y - GetTop(item)) <= 2);
                if (topAlign != default)
                {
                    SetTop(item, topAlign.Y);
                    var layer = AdornerLayer.GetAdornerLayer(this);
                    layer.Add(new MySelectionAlignLine(this, topAlign, new Point(GetLeft(item), GetTop(item))));
                }
            }
        }

        /// <summary>
        /// 清除绘制的对齐线
        /// </summary>
        public void ClearAlignLine()
        {
            var arr = AdornerLayer.GetAdornerLayer(this).GetAdorners(this);
            if (arr != null)
            {
                for (int i = arr.Length - 1; i >= 0; i--)
                {
                    AdornerLayer.GetAdornerLayer(this).Remove(arr[i]);
                }
            }
        }

        public void ZoomControls(int offsetX, int offsetY)
        {
            foreach (var item in SelectedItems)
            {
                if (item.ActualHeight + offsetY > 10)
                {
                    item.Height += offsetY;
                }
                if (item.ActualWidth + offsetX > 10)
                {
                    item.Width += offsetX;
                }
            }
        }
        #endregion
            
        #region 对齐操作
        public void AlignLeft()
        {
            if (SelectedItems == null || SelectedItems.Count == 0)
                return;
            var leftMin = SelectedItems.Min(x => System.Windows.Controls.Canvas.GetLeft(x));
            foreach (var item in SelectedItems)
            {
                SetLeft(item, leftMin);
            }
        }

        public void AlignRight()
        {
            if (SelectedItems == null || SelectedItems.Count == 0)
                return;

            var rightMax = SelectedItems.Max(x => GetLeft(x) + x.ActualWidth);
            foreach (var item in SelectedItems)
            {
                var targetLeft = rightMax - item.ActualWidth;
                SetLeft(item, targetLeft);
            }
        }

        public void AlignTop()
        {
            if (SelectedItems == null || SelectedItems.Count == 0)
                return;
            var topMin = SelectedItems.Min(x => GetTop(x));
            foreach (var item in SelectedItems)
            {
                SetTop(item, topMin);
            }
        }

        public void AlignBottom()
        {
            if (SelectedItems == null || SelectedItems.Count == 0)
                return;

            var botMax = SelectedItems.Max(x => GetTop(x) + x.ActualHeight);
            foreach (var item in SelectedItems)
            {
                var targetLeft = botMax - item.ActualHeight;
                SetTop(item, targetLeft);
            }
        }

        /// <summary>
        /// 垂直分布
        /// </summary>
        public void VertialLayout()
        {
            if (SelectedItems == null || SelectedItems.Count < 3)
                return;

            var topCtl = SelectedItems.Min(x => GetTop(x) + x.ActualHeight);
            var botCtrl = SelectedItems.Max(x => GetTop(x));
            var emptyHeight = botCtrl - topCtl;

            var orderCtrl = SelectedItems.OrderBy(x => GetTop(x)).ToList();
            orderCtrl.RemoveAt(0);
            orderCtrl.RemoveAt(orderCtrl.Count - 1);
            var useSpace = orderCtrl.Sum(x => x.ActualHeight);

            var ableSpaceAvg = (emptyHeight - useSpace) / (SelectedItems.Count - 1);
            double nowPostion = topCtl;
            foreach (var item in orderCtrl)
            {
                SetTop(item, nowPostion + ableSpaceAvg);
                nowPostion += item.ActualHeight;
            }
        }
        /// <summary>
        /// 水平分布
        /// </summary>
        public void HorizontalLayout()
        {
            if (SelectedItems == null || SelectedItems.Count < 3)
                return;

            var leftCtl = SelectedItems.Min(x => GetLeft(x) + x.ActualWidth);
            var rightCtrl = SelectedItems.Max(x => GetLeft(x));
            var emptyHeight = rightCtrl - leftCtl;

            var orderCtrl = SelectedItems.OrderBy(x => GetLeft(x)).ToList();
            orderCtrl.RemoveAt(0);
            orderCtrl.RemoveAt(orderCtrl.Count - 1);
            var useSpace = orderCtrl.Sum(x => x.ActualWidth);

            var ableSpaceAvg = (emptyHeight - useSpace) / (SelectedItems.Count - 1);
            double nowPostion = leftCtl;
            foreach (var item in orderCtrl)
            {
                SetLeft(item, nowPostion + ableSpaceAvg);
                nowPostion += item.ActualWidth;
            }
        }
        #endregion

        #region SelectionAdorner
        /// <summary>
        /// 选择装饰器:控件拖拽
        /// </summary>
        internal class SelectionAdorner : Adorner
        {
            public SelectionAdorner(UIElement adornedEIeent) : base(adornedEIeent) { }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);
                Rect adornerRect = new Rect(AdornedElement.DesiredSize);
                SolidColorBrush renderBrush = Brushes.Transparent;
                Pen render = new Pen(new SolidColorBrush(Colors.OrangeRed), 1);
                drawingContext.DrawRectangle(renderBrush, render, new Rect(adornerRect.TopLeft.X, adornerRect.TopLeft.Y, adornerRect.Width, adornerRect.Height));

                MouseDown += SelectionAdorner_MouseDown;
                MouseMove += SelectionAdorner_MouseMove;
                MouseUp += SelectionAdorner_MouseUp;
            }

            private void SelectionAdorner_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                ReleaseMouseCapture();
                MyCanvas.GetParentObject<MyCanvas>(AdornedElement).ClearAlignLine();
            }

            Point lastPoint = new Point();
            private void SelectionAdorner_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    CaptureMouse();
                    var nowPoint = e.GetPosition(MyCanvas.GetParentObject<MyCanvas>(AdornedElement));
                    int offsetX = (int)(nowPoint.X - lastPoint.X);
                    int offsetY = (int)(nowPoint.Y - lastPoint.Y);

                    MyCanvas.GetParentObject<MyCanvas>(AdornedElement).MoveControls(offsetX, offsetY);
                    lastPoint = nowPoint;
                }
                else if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    CaptureMouse();
                    var nowPoint = e.GetPosition(MyCanvas.GetParentObject<MyCanvas>(AdornedElement));
                    int offsetX = (int)(nowPoint.X - lastPoint.X);
                    int offsetY = (int)(nowPoint.Y - lastPoint.Y);

                    MyCanvas.GetParentObject<MyCanvas>(AdornedElement).ZoomControls(offsetX, offsetY);
                    lastPoint = nowPoint;
                }
            }

            private void SelectionAdorner_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                lastPoint = e.GetPosition(MyCanvas.GetParentObject<MyCanvas>(AdornedElement));
            }
        }
        #endregion
    }
}
