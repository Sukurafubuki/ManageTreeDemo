using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ManageTreeDemo.UserControls.Canvas
{
    public class MySelectionAlignLine:Adorner
    {
        public MySelectionAlignLine(UIElement adornedElement, Point start, Point end) : base(adornedElement)
        {
            startPoint = start;
            endPoint = end;
        }

        Point startPoint = default(Point);
        Point endPoint = default(Point);
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Rect adornerRect = new Rect(AdornedElement.DesiredSize);
            Pen render = new Pen(new SolidColorBrush(Colors.Gray), 1);
            drawingContext.DrawLine(render, startPoint, endPoint);
        }
    }
}
