using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MiniGraphicEditor.Classes
{
    class Mover
    {
        Editor Editor;
        int i;


        public Mover(Editor Editor)
        {
            this.Editor = Editor;
        }

        public void moveSelected(float x, float y)
        {
            for (i = Editor.figures.Length - 1; i > -1; i--)
            {
                if (!Editor.figures[i].Selected) continue;
                move(i, x, y);
            }
        }

        public void move(int figureIndex, float x, float y)
        {
            PointF originPoint = new PointF(), endPoint = new PointF();



            if (Editor.figures[figureIndex].EndPoint.X > Editor.figures[figureIndex].OriginPoint.X)
            {
                originPoint.X = Editor.figures[figureIndex].PathCopy.GetBounds().Left + x;
                endPoint.X = Editor.figures[figureIndex].PathCopy.GetBounds().Right + x;
            }
            else
            {
                originPoint.X = Editor.figures[figureIndex].PathCopy.GetBounds().Right + x;
                endPoint.X = Editor.figures[figureIndex].PathCopy.GetBounds().Left + x;
            }

            if (Editor.figures[figureIndex].EndPoint.Y > Editor.figures[figureIndex].OriginPoint.Y)
            {
                originPoint.Y = Editor.figures[figureIndex].PathCopy.GetBounds().Top + y;
                endPoint.Y = Editor.figures[figureIndex].PathCopy.GetBounds().Bottom + y;
            }
            else
            {
                originPoint.Y = Editor.figures[figureIndex].PathCopy.GetBounds().Bottom + y;
                endPoint.Y = Editor.figures[figureIndex].PathCopy.GetBounds().Top + y;
            }



            Editor.figures[figureIndex].initCalculations(originPoint, endPoint);
        }

    }
}
