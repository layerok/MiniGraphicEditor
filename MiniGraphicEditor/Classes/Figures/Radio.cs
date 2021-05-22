using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGraphicEditor.Classes.Figures
{
    class Radio : Figure
    {

        public Radio()
        {
            _points = new PointF[9];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {
            float wd = endPoint.X - originPoint.X;
            float hg = endPoint.Y - originPoint.Y;

            Points[0].X = endPoint.X;
            Points[0].Y = endPoint.Y;
            Points[1].Y = Points[8].Y = Points[7].Y = Points[2].Y = originPoint.Y + (hg) / 3;
            Points[1].X = Points[8].X = Points[0].X - (wd) / 8;
            Points[3].X = Points[6].X = Points[7].X = Points[2].X = Points[0].X - (wd) / 5;
            Points[3].Y = Points[4].Y = originPoint.Y;
            Points[4].X = Points[5].X = originPoint.X;
            Points[5].Y = Points[6].Y = originPoint.Y + ((hg) / 3) * 2;
            createPath(originPoint, endPoint);
        }

        public override void createPath(PointF originPoint, PointF endPoint)
        {
            Path = new GraphicsPath();
            Path.AddPolygon(Points);
        }
    }
}
