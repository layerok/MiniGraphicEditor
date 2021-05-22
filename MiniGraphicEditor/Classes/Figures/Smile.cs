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
            _points = new PointF[43];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {
            createPath(originPoint, endPoint);
        }

        public override void createPath(PointF originPoint, PointF endPoint)
        {
            Path = new GraphicsPath();
            float wd = endPoint.X - originPoint.X;
            float hg = endPoint.Y - originPoint.Y;
            GraphicsPath p = new GraphicsPath();
            Path.AddEllipse(originPoint.X, originPoint.Y, (wd), (hg));
            Path.AddBezier(originPoint.X + wd / 4, originPoint.Y + (hg / 12) * 7, originPoint.X + (wd / 4) * 2, originPoint.Y + (hg / 12) * 9, originPoint.X + (wd / 4) * 2, originPoint.Y + (hg / 12) * 9, originPoint.X + (wd / 4) * 3, originPoint.Y + (hg / 12) * 7);
            Path.AddEllipse(originPoint.X + (wd / 12) * 3, originPoint.Y + hg / 3, wd / 5, hg / 5);
            Path.AddEllipse(originPoint.X + (wd / 12) * 7, originPoint.Y + hg / 3, wd / 5, hg / 5);
          
            
        }
    }
}
