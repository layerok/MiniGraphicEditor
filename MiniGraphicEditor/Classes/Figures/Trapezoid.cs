using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace MiniGraphicEditor.Classes.Figures
{
    class Trapezoid : Figure
    {

        public Trapezoid()
        {
            _points = new PointF[4];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {
            Points[0].X = originPoint.X + (_width) / 4;
            Points[0].Y = originPoint.Y;

            Points[1].Y = originPoint.Y;
            Points[1].X = endPoint.X - (_width) / 4;

            Points[2].X = endPoint.X;
            Points[2].Y = endPoint.Y;


            Points[3].X = originPoint.X;
            Points[3].Y = endPoint.Y;

        }

        public override GraphicsPath createPath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(Points);
            return path;
        }
    }
}
