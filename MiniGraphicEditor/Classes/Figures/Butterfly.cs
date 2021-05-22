using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGraphicEditor.Classes.Figures
{
    class Butterfly : Figure
    {

        public Butterfly()
        {
            _points = new PointF[4];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {

            float width = endPoint.X - originPoint.X;
            float height = endPoint.Y - originPoint.Y;


            Points[0].X = Points[3].X = originPoint.X; Points[0].Y = Points[2].Y = originPoint.Y;
            Points[1].X = Points[2].X = endPoint.X; Points[1].Y = Points[3].Y = endPoint.Y;
            createPath(originPoint, endPoint);
        }

        public override void createPath(PointF originPoint, PointF endPoint)
        {
            Path = new GraphicsPath();
            Path.AddPolygon(Points);
        }
    }
}
