using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGraphicEditor.Classes.Figures
{
    class Turn : Figure
    {

        public Turn()
        {
            _points = new PointF[9];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {

            float wd = endPoint.X - originPoint.X;
            float hg = endPoint.Y - originPoint.Y;

            Points[0].X = Points[8].X = originPoint.X; 
            Points[0].Y = Points[1].Y = originPoint.Y + (hg / 5) * 4;
            Points[1].X = Points[2].X = Points[0].X + wd / 8 * 6;
            Points[2].Y = Points[6].Y = Points[5].Y = Points[3].Y = originPoint.Y+ hg / 5;
            Points[3].X = Points[2].X - wd / 12;
            Points[4].X = Points[3].X + (endPoint.X - Points[3].X) / 2;
            Points[4].Y = originPoint.Y;
            Points[5].X = endPoint.X;
            Points[6].X = Points[7].X = endPoint.X - wd / 12;
            Points[7].Y = Points[8].Y = endPoint.Y;

            createPath(originPoint, endPoint);
        }

        public override void createPath(PointF originPoint, PointF endPoint)
        {
            Path = new GraphicsPath();
            Path.AddPolygon(Points);
        }
    }
}
