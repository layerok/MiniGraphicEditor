using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;



namespace MiniGraphicEditor.Classes.Figures
{
    class Lightning : Figure
    {


        public Lightning()
        {
            _points = new PointF[11];
        }
       
        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {

            float width = endPoint.X - originPoint.X;
            float height = endPoint.Y - originPoint.Y;

            Points[0] = endPoint;
            Points[1].X = endPoint.X - (width) / 2;
            Points[1].Y = endPoint.Y - (height) / 4;
            Points[2].X = Points[1].X + (width) / 12;
            Points[2].Y = Points[1].Y - (height) / 12;
            Points[3].X = endPoint.X - ((width) / 4) * 3;
            Points[3].Y = endPoint.Y - (height) / 2;
            Points[4].X = Points[3].X + (width) / 12;
            Points[4].Y = Points[3].Y - (height) / 12;
            Points[5].X = originPoint.X;
            Points[5].Y = originPoint.Y + (height) / 5;
            Points[6].X = originPoint.X + (width) / 3;
            Points[6].Y = originPoint.Y;
            Points[7].X = Points[4].X + (Points[4].X - Points[3].X) * 2.25f;
            Points[7].Y = Points[4].Y + (Points[4].Y - Points[3].Y) * 2.25f;
            Points[8].X = Points[3].X + (Points[4].X - Points[3].X) * 2.5f;
            Points[8].Y = Points[3].Y + (Points[4].Y - Points[3].Y) * 2.5f;
            Points[9].X = Points[2].X + (Points[2].X - Points[1].X) * 1.5f;
            Points[9].Y = Points[2].Y + (Points[2].Y - Points[1].Y) * 1.5f;
            Points[10].X = Points[1].X + (Points[2].X - Points[1].X) * 2;
            Points[10].Y = Points[1].Y + (Points[2].Y - Points[1].Y) * 2;
            createPath(originPoint, endPoint);
        }

        public override void createPath(PointF originPoint, PointF endPoint)
        {
            Path = new GraphicsPath();
            Path.AddPolygon(Points);
        }
    }
}
