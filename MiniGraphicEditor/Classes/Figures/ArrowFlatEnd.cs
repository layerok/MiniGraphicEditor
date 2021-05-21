using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor.Classes
{
    class ArrowFlatEnd : Figure
    {

        public ArrowFlatEnd()
        {
            _points = new PointF[7];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {
            float width = endPoint.X - originPoint.X;
            float height = endPoint.Y - originPoint.Y;

            Points[0].X = Points[1].X = originPoint.X;
            Points[0].Y = Points[6].Y = originPoint.Y + height / 5;
            Points[1].Y = Points[2].Y = endPoint.Y - height / 5;
            Points[2].X = Points[3].X = Points[5].X = Points[6].X = originPoint.X + width / 2;
            Points[3].Y = endPoint.Y;
            Points[4].Y = originPoint.Y + height / 2;
            Points[4].X = endPoint.X;
            Points[5].Y = originPoint.Y;


        }
    }
}
