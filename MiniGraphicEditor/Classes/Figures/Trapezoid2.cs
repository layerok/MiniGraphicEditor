using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace MiniGraphicEditor.Classes.Figures
{
    class Trapezoid2 : Figure
    {
        PointF[] bezier1 = new PointF[4];
        PointF[] bezier2 = new PointF[4];
        public Trapezoid2()
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

            int divider = 4;



            bezier1[0].X = Points[0].X;
            bezier1[0].Y = originPoint.Y;

            bezier1[1].X = Points[0].X + Width / divider;
            bezier1[1].Y = originPoint.Y + Height / divider;

            bezier1[2].X = Points[1].X - Width / divider;
            bezier1[2].Y = originPoint.Y + Height / divider;

            bezier1[3].X = Points[1].X;
            bezier1[3].Y = originPoint.Y;


            bezier2[0].X = endPoint.X ;
            bezier2[0].Y = endPoint.Y;

            bezier2[1].X = endPoint.X - Width / divider;
            bezier2[1].Y = endPoint.Y - Height / divider;

            bezier2[2].X = originPoint.X + Width / divider;
            bezier2[2].Y = endPoint.Y - Height / divider;

            bezier2[3].X = originPoint.X;
            bezier2[3].Y = endPoint.Y;

        }

        public override GraphicsPath createPath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddBezier(bezier1[0], bezier1[1], bezier1[2], bezier1[3]);
            path.AddLine(Points[1], Points[2]);
            path.AddBezier(bezier2[0], bezier2[1], bezier2[2], bezier2[3]);

            path.AddLine(Points[3], Points[0]);

            





            return path;
        }
    }
}
