using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;



namespace MiniGraphicEditor.Classes.Figures
{
    class ArrowDown : Figure
    {

        PointF[] bezier1 = new PointF[4];
        PointF[] bezier2 = new PointF[4];

        public ArrowDown()
        {
            _points = new PointF[10];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {

            float colW = (_width / 12);
            float colH = (_height / 12);

            Points[0].X = endPoint.X;
            Points[0].Y = originPoint.Y;

            Points[1].X = Points[0].X;
            Points[1].Y = originPoint.Y + (colH * 6);

            Points[2].X = endPoint.X - (colW * 4);
            Points[2].Y = originPoint.Y + (colH * 6);

            Points[3].X = Points[2].X;
            Points[3].Y = Points[2].Y + (colH * 3);

            Points[4].X = Points[3].X + (colW * 1);
            Points[4].Y = Points[3].Y;

            Points[5].X = endPoint.X - (colW * 5.5f);
            Points[5].Y = endPoint.Y;

            Points[6].X = endPoint.X - (colW * 8);
            Points[6].Y = Points[3].Y;

            Points[7].X = Points[6].X + (colW);
            Points[7].Y = Points[3].Y;

            Points[8].X = Points[2].X - (colW * 3);
            Points[8].Y = Points[2].Y;

            Points[9].X = Points[8].X - (colW * 4);
            Points[9].Y = Points[2].Y;

            bezier1[0] = Points[9];



            bezier1[1].X = Points[9].X - colW;
            bezier1[1].Y = originPoint.Y + (colH * 3);

            bezier1[2].X = bezier1[1].X;
            bezier1[2].Y = originPoint.Y + (colH * 3);

            bezier1[3].X = Points[9].X;
            bezier1[3].Y = originPoint.Y;



            bezier2[0] = bezier1[3];

            bezier2[1].X = endPoint.X - (colW * 5.5f);
            bezier2[1].Y = originPoint.Y + (colH * 3);

            bezier2[2] = bezier2[1];

            bezier2[3] = Points[0];


        }

        public override GraphicsPath createPath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLines(Points);

            path.AddBezier(bezier1[0], bezier1[1], bezier1[2], bezier1[3]);


            path.AddBezier(bezier2[0], bezier2[1], bezier2[2], bezier2[3]);

            return path;

        }


    }
}
