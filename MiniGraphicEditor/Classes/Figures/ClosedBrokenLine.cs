using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;



namespace MiniGraphicEditor.Classes.Figures
{
    class ClosedBrokenLine : Figure
    {

        PointF[] bezier1 = new PointF[4];
        PointF[] bezier2 = new PointF[4];

        public ClosedBrokenLine()
        {
            _points = new PointF[9];
        }
       
        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {

            // Фигура состоит из 7 линий и 2 кривых безье

            float oneEightHeight = (_height / 8);
            float oneEightWidth = (_width / 8);

            float halfWidth = (_width / 2);
            float halfHeight = (_height / 2);

            float thirdHeight = (_height / 3);
            float thirdWidth = (_width / 3);

            float twoFifthHeight = (_height / 5) * 2;
            float twoFifthWidth = (_width / 5) * 2;

            Points[0].X = originPoint.X;
            Points[0].Y = originPoint.Y + oneEightHeight;

            Points[1].X = originPoint.X + oneEightWidth;
            Points[1].Y = originPoint.Y;

            Points[2].X = originPoint.X + twoFifthWidth;
            Points[2].Y = originPoint.Y + thirdHeight;

            Points[3].X = endPoint.X - thirdWidth;
            Points[3].Y = originPoint.Y;

            //
            bezier1[0].X = Points[3].X;
            bezier1[0].Y = originPoint.Y;

            bezier1[1].X = Points[3].X + thirdWidth;
            bezier1[1].Y = originPoint.Y + halfHeight;

            bezier1[2].X = bezier1[1].X;
            bezier1[2].Y = bezier1[1].Y;

            bezier1[3].X = Points[3].X;
            bezier1[3].Y = endPoint.Y;
            //

            Points[4].X = Points[3].X;
            Points[4].Y = endPoint.Y;

            Points[5].X = Points[2].X;
            Points[5].Y = endPoint.Y - thirdHeight;

            Points[6].X = Points[1].X;
            Points[6].Y = endPoint.Y;

            Points[7].X = originPoint.X;
            Points[7].Y = endPoint.Y - oneEightHeight;

            Points[8].X = Points[2].X - oneEightWidth;
            Points[8].Y = Points[2].Y + oneEightHeight;


            //
            bezier2[0].X = originPoint.X;
            bezier2[0].Y = endPoint.Y - oneEightHeight;

            bezier2[1].X = originPoint.X;
            bezier2[1].Y = originPoint.Y + halfHeight;

            bezier2[2].X = Points[8].X;
            bezier2[2].Y = Points[8].Y;

            bezier2[3].X = Points[8].X;
            bezier2[3].Y = Points[8].Y;
            //

        }

        public override GraphicsPath createPath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(Points[0], Points[1]);
            path.AddLine(Points[1], Points[2]);
            path.AddLine(Points[2], Points[3]);

            path.AddBezier(bezier1[0], bezier1[1], bezier1[2], bezier1[3]);

            path.AddLine(Points[4], Points[5]);
            path.AddLine(Points[5], Points[6]);
            path.AddLine(Points[6], Points[7]);

            path.AddBezier(bezier2[0], bezier2[1], bezier2[2], bezier2[3]);

            path.AddLine(Points[8], Points[0]);
            return path;

        }


    }
}
