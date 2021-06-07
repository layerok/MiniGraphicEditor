using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;



namespace MiniGraphicEditor.Classes.Figures
{
    class Vase : Figure
    {

        PointF[] bezierLeft = new PointF[4];
        PointF[] bezierRight = new PointF[4];

        public Vase()
        {
            _points = new PointF[6];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {

            float colW = (_width / 12);
            float colH = (_height / 12);

            Points[0] = originPoint;

            Points[1].X = endPoint.X;
            Points[1].Y = originPoint.Y;

            Points[2].X = endPoint.X - (colW * 2);
            Points[2].Y = originPoint.Y + (colH * 6);

            Points[3] = endPoint;

            Points[4].X = originPoint.X;
            Points[4].Y = endPoint.Y;

            Points[5].X = originPoint.X + (colW * 2);
            Points[5].Y = originPoint.Y + (colH * 6);


            //
            bezierLeft[3] = Points[5];

            bezierLeft[2].X = Points[5].X;
            bezierLeft[2].Y = endPoint.Y - (colH * 2);

            bezierLeft[1].X = originPoint.X + (colW * 1);
            bezierLeft[1].Y = endPoint.Y - (colH * 1);

            bezierLeft[0] = Points[4];


            //
            bezierRight[0] = Points[2];

            bezierRight[1].X = Points[2].X;
            bezierRight[1].Y = endPoint.Y - (colH * 2);

            bezierRight[2].X = endPoint.X - (colW * 1);
            bezierRight[2].Y = endPoint.Y - (colH);

            bezierRight[3] = Points[3];

        }

        double radiansToDegres(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public override GraphicsPath createPath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(Points[3], Points[4]);
            
            path.AddBezier(bezierLeft[0], bezierLeft[1], bezierLeft[2], bezierLeft[3]);

            path.AddLine(Points[5], Points[0]);

            path.AddLine(Points[0], Points[1]);
            path.AddLine(Points[1], Points[2]);

            path.AddBezier(bezierRight[0], bezierRight[1], bezierRight[2], bezierRight[3]);

            

            
           
            
            return path;

        }





    }
}
