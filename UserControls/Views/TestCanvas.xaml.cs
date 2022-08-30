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
using System.Windows.Controls.Primitives;
using ManageTreeDemo.UserControls.Canvas;

namespace ManageTreeDemo.UserControls.Views
{
    /// <summary>
    /// TestCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class TestCanvas : UserControl
    {
        /// <summary>
        /// 控件列表
        /// </summary>
        private List<Control> rightControls = new List<Control>();

        public TestCanvas()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //this.Focus();
            //this.one.SetIsEnableInput(false);
            foreach (var control in this.right.Children)
            {
                var thumb = control as MyBaseThumb;
                if (thumb != null)
                {
                    thumb.SetIsEnableInput(false);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var width = this.right.ActualWidth;
            var height = this.right.ActualHeight;

            int x = 0;
            int y = 0;
            //横线
            while (y < height)
            {
                System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();
                line.X1 = x;
                line.Y1 = y;
                line.X2 = width;
                line.Y2 = y;
                line.Stroke = Brushes.LightGray;
                line.StrokeThickness = 1;
                this.right.Children.Add(line);
                y = y + 10;
            }
            //重新初始化值
            x = 0;
            y = 0;
            //竖线
            while (x < width)
            {
                System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();
                line.X1 = x;
                line.Y1 = y;
                line.X2 = x;
                line.Y2 = height;
                line.Stroke = Brushes.LightGray;
                line.StrokeThickness = 1;
                this.right.Children.Add(line);
                x = x + 10;
            }
        }

        private void rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas.Rectangle squareControl = new Canvas.Rectangle();
            this.right.Children.Add(squareControl);
        }

        private void rhombus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rhombus rhombusControl = new Rhombus();
            this.right.Children.Add(rhombusControl);
        }

        private void line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas.Line lineArrowControl = new Canvas.Line();
            this.right.Children.Add(lineArrowControl);
        }

        private void circle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Circle circleControl = new Circle();
            this.right.Children.Add(circleControl);
        }
    }
}
