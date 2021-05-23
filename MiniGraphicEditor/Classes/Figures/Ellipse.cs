using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGraphicEditor.Classes.Figures
{
    class Ellipse : Figure
    {

        public Ellipse()
        {
            _points = new PointF[0];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {}

        public override GraphicsPath createPath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(_originPoint.X, _originPoint.Y, (_width), (_height));
            return path;
        }
    }
}
