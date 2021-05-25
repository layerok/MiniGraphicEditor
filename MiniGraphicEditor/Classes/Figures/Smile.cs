using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGraphicEditor.Classes.Figures
{
    class Smile : Figure
    {

        public Smile()
        {
            _points = new PointF[0];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {
            
        }

        public override GraphicsPath createPath()
        {
            GraphicsPath path = new GraphicsPath();

            path.AddEllipse(_originPoint.X, _originPoint.Y, (_width), (_height));
            path.AddBezier(_originPoint.X + _width / 4, _originPoint.Y + (_height / 12) * 7, _originPoint.X + (_width / 4) * 2, _originPoint.Y + (_height / 12) * 9, _originPoint.X + (_width / 4) * 2, _originPoint.Y + (_height / 12) * 9, _originPoint.X + (_width / 4) * 3, _originPoint.Y + (_height / 12) * 7);
            path.AddEllipse(_originPoint.X + (_width / 12) * 3, _originPoint.Y + _height / 3, _width / 5, _height / 5);
            path.AddEllipse(_originPoint.X + (_width / 12) * 7, _originPoint.Y + _height / 3, _width / 5, _height / 5);
            return path;
          
            
        }
    }
}
