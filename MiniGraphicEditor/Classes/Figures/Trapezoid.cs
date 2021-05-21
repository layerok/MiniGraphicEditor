using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

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
            float width = endPoint.X - originPoint.X;
            float height = endPoint.Y - originPoint.Y;

            Points[0].X = originPoint.X; Points[0].Y = Points[1].Y = originPoint.Y;
            Points[2].X = endPoint.X; Points[2].Y = Points[3].Y = originPoint.Y + height;
            Points[3].X = Points[0].X - (width) / 4;
            Points[1].X = Points[2].X - (width) / 4;

        }

    }
}
