using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGraphicEditor.Classes.Figures
{
    class Hedgehog : Figure
    {

        public Hedgehog()
        {
            _points = new PointF[26];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {

            Points[0].X = Points[24].X = originPoint.X; Points[0].Y = Points[2].Y = Points[4].Y = originPoint.Y;
            Points[1].X = Points[2].X = originPoint.X + (_width / 4); Points[1].Y = Points[3].Y = originPoint.Y + (_height / 5);
            Points[3].X = originPoint.X + (_width / 10) * 4;
            Points[4].X = Points[18].X = originPoint.X + (_width / 2);
            Points[5].Y = Points[1].Y + (Points[1].Y - originPoint.Y) / 2; Points[5].X = originPoint.X + (_width / 3) * 2;
            Points[6].Y = originPoint.Y + (Points[1].Y - originPoint.Y) / 2; Points[6].X = originPoint.X + (_width / 15) * 11;
            Points[7].Y = originPoint.Y + (_height / 9) * 3; Points[7].X = originPoint.X + (_width / 15) * 12;
            Points[8].Y = originPoint.Y + (_height / 9); Points[8].X = originPoint.X + (_width / 15) * 14;
            Points[9].Y = Points[10].Y = originPoint.Y + (_height / 9) * 4; Points[9].X = Points[13].X = originPoint.X + (_width / 16) * 14;
            Points[10].X = Points[12].X = Points[14].X = endPoint.X;
            Points[11].Y = Points[24].Y = originPoint.Y + (_height / 9) * 5; Points[11].X = originPoint.X + (_width / 16) * 15;
            Points[12].Y = Points[23].Y = originPoint.Y + (_height / 12) * 8;
            Points[13].Y = originPoint.Y + (_height / 9) * 6;
            Points[14].Y = originPoint.Y + (_height / 9) * 7;
            Points[15].X = Points[16].X = originPoint.X + (_width / 15) * 13; Points[15].Y = Points[19].Y = Points[22].Y = originPoint.Y + (_height / 10) * 7;
            Points[16].Y = Points[18].Y = Points[20].Y = endPoint.Y;
            Points[17].Y = originPoint.Y + (_height / 10) * 8;
            Points[17].X = originPoint.X + (_width / 15) * 10;
            Points[19].X = originPoint.X + (_width / 15) * 8;
            Points[20].X = originPoint.X + (_width / 15) * 3;
            Points[21].X = Points[23].X = originPoint.X + (_width / 15) * 4; Points[21].Y = originPoint.Y + (_height / 10) * 8;
            Points[22].X = originPoint.X + (_width / 15) * 2;
            Points[25].X = originPoint.X + (_width / 15) * 5;
            Points[25].Y = originPoint.Y + (_height / 10) * 5;
        }


        public override GraphicsPath createPath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(Points);
            return path;
        }
    }
}
