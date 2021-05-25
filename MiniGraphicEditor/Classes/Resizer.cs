using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniGraphicEditor.Classes
{
    class Resizer
    {

        Editor Editor;
        int i, j, k;
        PointF[] points = new PointF[8];

        public RectangleF selectionRect;
        public RectangleF initialSelectionRect;
        public int pointIndex;

        public int pointSize = 10;




        public Resizer(Editor editor)
        {
            this.Editor = editor;
        }

        public void showPoints(Graphics g)
        {

            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Selected)
                {
                    for (j = 0; j < 8; j++)
                    {
                        g.FillRectangle(new SolidBrush(Color.Blue), this.points[j].X, this.points[j].Y, pointSize, pointSize);
                    }
                    break;
                }
            }
        }

        public void calculatePoints()
        {
            RectangleF rect = getRect();
            this.selectionRect = rect;


            this.points[0].X = this.points[3].X = this.points[5].X = rect.Left - pointSize;
            this.points[1].X = this.points[6].X = rect.Left + rect.Width / 2 - (pointSize / 2);
            this.points[2].X = this.points[4].X = this.points[7].X = rect.Right;
            this.points[0].Y = this.points[1].Y = this.points[2].Y = rect.Top - pointSize;
            this.points[3].Y = this.points[4].Y = rect.Top + rect.Height / 2 - (pointSize / 2);
            this.points[5].Y = this.points[6].Y = this.points[7].Y = rect.Bottom;
        }


        public RectangleF getRect()
        {
            RectangleF rect;
            Editor.currentFigure.Path.Reset();
            for (j = 0; j < Editor.figures.Length; j++)
            {
                if (Editor.figures[j].Selected) Editor.currentFigure.Path.AddPath(Editor.figures[j].NotTransformedPath, true);
            }

            rect = Editor.currentFigure.Path.GetBounds();
            return rect;
        }

        public bool checkIfPointWasClicked(MouseEventArgs e)
        {

            for (i = 0; i < this.points.Length; i++)
            {
                if (
                    e.X >= this.points[i].X &&
                    e.X <= this.points[i].X + pointSize &&
                    e.Y >= this.points[i].Y &&
                    e.Y <= this.points[i].Y + pointSize
                    )
                {
                    pointIndex = i;
                    Editor.state = States.RESIZE_SELECTION;

                    for (k = 0; k < Editor.figures.Length; k++)
                    {

                        if (!Editor.figures[k].Selected) continue;
                        Editor.figures[k].PathCopy = Editor.figures[k].NotTransformedPath;

                    }
                    return true;
                }
            }

            return false;
        }

        public void resize(MouseEventArgs e, EditorForm form)
        {
            PointF delta;

            delta = new PointF();


            delta.Y = e.Y - Editor.pressedPoint.Y;
            delta.X = e.X - Editor.pressedPoint.X;

            if (pointIndex == 0)
            {
                // top-left
                delta.Y *= -1;
                delta.X *= -1;
                if (e.X >= selectionRect.Right - pointSize || e.Y >= selectionRect.Bottom - pointSize) return;
            }
            else if (pointIndex == 1)
            {
                // top-center
                delta.Y *= -1;
                if (e.Y >= selectionRect.Bottom - pointSize) return;
            }
            else if (pointIndex == 2)
            {
                //right-top
                delta.Y *= -1;
                if (e.Y >= selectionRect.Bottom - pointSize || e.X <= selectionRect.Left + pointSize) return;
            }
            else if (pointIndex == 3)
            {
                //left-center
                delta.X *= -1;
                if (e.X >= selectionRect.Right - pointSize) { return; }
            }
            else if (pointIndex == 4)
            {
                // right-center
                if (e.X <= selectionRect.Left + pointSize) { return; }
            }
            else if (pointIndex == 5)
            {
                // left-bottom
                delta.X *= -1;
                if (e.X >= selectionRect.Right - pointSize || e.Y <= selectionRect.Top + pointSize) return;
            }
            else if (pointIndex == 6)
            {
                //bottom-center
                if (e.Y <= selectionRect.Top + pointSize) return;
            }
            else if (pointIndex == 7)
            {
                //bottom-right
                if (e.Y <= selectionRect.Top + pointSize || e.X <= selectionRect.Left + pointSize) return;
            }

            switch(pointIndex)
            {
                case 1: case 6: form.Cursor = Cursors.SizeNS; break;
                case 3: case 4: form.Cursor = Cursors.SizeWE; break;
                case 0: case 7: form.Cursor = Cursors.SizeNWSE; break;
                case 2: case 5: form.Cursor = Cursors.SizeNESW; break;
            }
                

            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Selected)
                {

                    // Логика пересчета точек фигур при ресайзе
                    // Механизм ресайза такой. Узнаем насколько процентов увеличелась высота и ширина. 
                    // Узнав процент смещаем точки на такой процент
                    
                    float resizedHeight = initialSelectionRect.Height + delta.Y;
                    float percentY = (100 * resizedHeight / initialSelectionRect.Height) - 100;
                    

                    float initialWidth = initialSelectionRect.Width;
                    float resizedWidth = initialSelectionRect.Width + delta.X;
                    float percentX = (100 * resizedWidth / initialSelectionRect.Width) - 100;

                    float originOffsetFromLeft = Editor.figures[i].CloneInstance.OriginPoint.X - initialSelectionRect.X;
                    float originOffsetFromTop = Editor.figures[i].CloneInstance.OriginPoint.Y - initialSelectionRect.Y;

                    float endOffsetFromLeft = Editor.figures[i].CloneInstance.EndPoint.X - initialSelectionRect.X;
                    float endOffsetFromTop = Editor.figures[i].CloneInstance.EndPoint.Y - initialSelectionRect.Y;

                    float resizedOriginOffsetX = originOffsetFromLeft / 100 * percentX;
                    float resizedOriginOffsetY = originOffsetFromTop / 100 * percentY;

                    float resizedEndOffsetX = endOffsetFromLeft / 100 * percentX;
                    float resizedEndOffsetY = endOffsetFromTop / 100 * percentY;

                    float originLeftX = Editor.figures[i].CloneInstance.OriginPoint.X;
                    float originLeftY = Editor.figures[i].CloneInstance.OriginPoint.Y;


                    float endLeftX = Editor.figures[i].CloneInstance.EndPoint.X;
                    float endLeftY = Editor.figures[i].CloneInstance.EndPoint.Y;

                    if(pointIndex == 0 || pointIndex == 3 || pointIndex == 5)
                    {
                        originLeftX -= delta.X;
                        endLeftX -= delta.X;

                    }

                    if (pointIndex == 1 || pointIndex == 0 || pointIndex == 2)
                    {
                        originLeftY -= delta.Y;
                        endLeftY -= delta.Y;
                    }

                    if(pointIndex == 1 || pointIndex == 6)
                    {
                        // точки которые ресайзят только по вертикали
                        resizedEndOffsetX = 0;
                        resizedOriginOffsetX = 0;
                    }

                    if (pointIndex == 3 || pointIndex == 4)
                    {
                        // точки которые ресайзят только по горизантали
                        resizedEndOffsetY = 0;
                        resizedOriginOffsetY = 0;
                    }


                    PointF startPoint = new PointF();
                    PointF endPoint = new PointF();

                    startPoint.X = originLeftX + resizedOriginOffsetX;
                    startPoint.Y = originLeftY + resizedOriginOffsetY;

                    endPoint.X = endLeftX + resizedEndOffsetX;
                    endPoint.Y = endLeftY + resizedEndOffsetY;



                    Editor.figures[i].initCalculations(startPoint, endPoint);
                }
                form.Invalidate();
            }


        }

    }
}
