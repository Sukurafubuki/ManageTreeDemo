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
using ManageTreeDemo.Common;

namespace ManageTreeDemo.UserControls.Canvas
{
    /// <summary>
    /// Rhombus.xaml 的交互逻辑
    /// </summary>
    public partial class Rhombus : UserControl
    {
        //RhomhubsViewModel myVM = null;
        public Polygon polygon;

        public Rhombus()
        {
            polygon = new Polygon();
            InitializeComponent();
            //myVM = new RhomhubsViewModel(this);
            //this.DataContext = myVM;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            var layer = AdornerLayer.GetAdornerLayer(s);
            layer.Add(new MyCanvasAdorner(s));
            polygon.Points = GetPoints(s);
            polygon.Fill = System.Windows.Media.Brushes.Green;
            canv.Children.Add(polygon);
        }

        private PointCollection GetPoints(MyBaseThumb _myBaseThumb)
        {
            UIElement parentele = VisualTreeHelper.GetParent(_myBaseThumb) as UIElement;
            Point p = _myBaseThumb.TranslatePoint(new Point(0, 0), parentele);
            Point top = new Point(p.X + _myBaseThumb.Width / 2, p.Y);
            Point left = new Point(p.X, p.Y + _myBaseThumb.Height / 2);
            Point right = new Point(p.X + _myBaseThumb.Width, p.Y + _myBaseThumb.Height / 2);
            Point bottom = new Point(p.X + _myBaseThumb.Width / 2, p.X + _myBaseThumb.Height);
            PointCollection result = new PointCollection();
            result.Add(top);
            result.Add(left);
            result.Add(bottom);
            result.Add(right);
            return result;
        }

        private void s_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //myVM.polygonpoints = myVM.GetPoints(s);
        }
    }

    /// <summary>
    /// 视图模型，部分动态数据绑定
    /// </summary>
    public class RhomhubsViewModel:NotifyPropertyBase
    {
        private PointCollection _polygonpoints;
        /// <summary>
        /// 闭合图形点集合
        /// </summary>
        public PointCollection polygonpoints { get { return _polygonpoints; } set { _polygonpoints = value; OnPropertyChanged("_polygonpoints"); } }
        /// <summary>
        /// 视图对象
        /// </summary>
        private Rhombus myView { get; set; }

        public RhomhubsViewModel(Rhombus rhombus)
        {
            myView = rhombus;
            polygonpoints = GetPoints(rhombus.s);
        }

        public PointCollection GetPoints(MyBaseThumb _myBaseThumb)
        {
            UIElement parentele = VisualTreeHelper.GetParent(_myBaseThumb) as UIElement;
            Point p = _myBaseThumb.TranslatePoint(new Point(0, 0), parentele);
            Point top = new Point(p.X+_myBaseThumb.Width/2, p.Y);
            Point left = new Point(p.X,p.Y+_myBaseThumb.Height/2);
            Point right = new Point(p.X+_myBaseThumb.Width, p.Y + _myBaseThumb.Height / 2);
            Point bottom = new Point(p.X + _myBaseThumb.Width / 2, p.X + _myBaseThumb.Height);
            PointCollection result = new PointCollection();
            result.Add(top);
            result.Add(left);
            result.Add(bottom);
            result.Add(right);
            return result;
        }
    }
}
