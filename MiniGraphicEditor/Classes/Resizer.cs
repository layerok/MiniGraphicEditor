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


            if (pointIndex == 1 || pointIndex == 6)
            {
                form.Cursor = Cursors.SizeNS;
            }

            if (pointIndex == 3 || pointIndex == 4)
            {
                form.Cursor = Cursors.SizeWE;
            }
            if (pointIndex == 0 || pointIndex == 7)
            {
                form.Cursor = Cursors.SizeNWSE;
            }
            if (pointIndex == 2 || pointIndex == 5)
            {
                form.Cursor = Cursors.SizeNESW;
            }

            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Selected)
                {



                    // Логика пересчета точек фигур при ресайзе
                    float initialHeight = initialSelectionRect.Height;
                    float resizedHeight = initialSelectionRect.Height + delta.Y;

                    float initialWidth = initialSelectionRect.Width;
                    float resizedWidth = initialSelectionRect.Width + delta.X;


                    // Пропорциональное увеличение только на угловых точках
                    if (pointIndex == 0 || pointIndex == 2 || pointIndex == 5 || pointIndex == 7)
                    {
                        resizedHeight = (Editor.shiftPressed) ? Math.Max(resizedHeight, resizedWidth) : resizedHeight;
                        resizedWidth = (Editor.shiftPressed) ? Math.Max(resizedHeight, resizedWidth) : resizedWidth;
                    }

                    float originDeltaY = Editor.figures[i].PathCopy.GetBounds().Top - initialSelectionRect.Top;
                    float endDeltaY = Editor.figures[i].PathCopy.GetBounds().Bottom - initialSelectionRect.Top;

                    float originDeltaX = Editor.figures[i].PathCopy.GetBounds().Left - initialSelectionRect.Left;
                    float endDeltaX = Editor.figures[i].PathCopy.GetBounds().Right - initialSelectionRect.Left;

                    float originPercentY = 100 * originDeltaY / initialHeight;
                    float endPercentY = 100 * endDeltaY / initialHeight;

                    float originPercentX = 100 * originDeltaX / initialWidth;
                    float endPercentX = 100 * endDeltaX / initialWidth;

                    float originNewDeltaY = resizedHeight * originPercentY / 100;
                    float endNewDeltaY = resizedHeight * endPercentY / 100;

                    float originNewDeltaX = resizedWidth * originPercentX / 100;
                    float endNewDeltaX = resizedWidth * endPercentX / 100;


                    PointF startPoint = new Point();
                    PointF endPoint = new Point();
                    float top, left;

                    if (pointIndex == 0)
                    {
                        top = Editor.shiftPressed ? initialSelectionRect.Bottom - resizedHeight : initialSelectionRect.Top - delta.Y;
                        left = Editor.shiftPressed ? initialSelectionRect.Right - resizedWidth : initialSelectionRect.Left - delta.X;

                        startPoint.Y = top + originNewDeltaY;
                        startPoint.X = left + originNewDeltaX;
                        endPoint.Y = top + endNewDeltaY;
                        endPoint.X = left + endNewDeltaX;
                    }


                    if (pointIndex == 1)
                    {
                        startPoint.Y = initialSelectionRect.Top - delta.Y + originNewDeltaY;
                        startPoint.X = Editor.figures[i].PathCopy.GetBounds().Left;
                        endPoint.Y = initialSelectionRect.Top - delta.Y + endNewDeltaY;
                        endPoint.X = Editor.figures[i].PathCopy.GetBounds().Right;
                    }


                    if (pointIndex == 2)
                    {
                        top = Editor.shiftPressed ? initialSelectionRect.Bottom - resizedHeight : initialSelectionRect.Top - delta.Y;
                        left = Editor.shiftPressed ? initialSelectionRect.Left : initialSelectionRect.Left;

                        startPoint.Y = top + originNewDeltaY;
                        startPoint.X = left + originNewDeltaX;
                        endPoint.Y = top + endNewDeltaY;
                        endPoint.X = left + endNewDeltaX;
                    }

                    if (pointIndex == 3)
                    {
                        startPoint.Y = Editor.figures[i].PathCopy.GetBounds().Top;
                        startPoint.X = (initialSelectionRect.Left - delta.X) + originNewDeltaX;
                        endPoint.Y = Editor.figures[i].PathCopy.GetBounds().Bottom;
                        endPoint.X = (initialSelectionRect.Left - delta.X) + endNewDeltaX;
                    }

                    if (pointIndex == 4)
                    {
                        startPoint.Y = Editor.figures[i].PathCopy.GetBounds().Top;
                        startPoint.X = initialSelectionRect.Left + originNewDeltaX;
                        endPoint.Y = Editor.figures[i].PathCopy.GetBounds().Bottom;
                        endPoint.X = initialSelectionRect.Left + endNewDeltaX;
                    }

                    if (pointIndex == 5)
                    {

                        top = Editor.shiftPressed ? initialSelectionRect.Top : initialSelectionRect.Top;
                        left = Editor.shiftPressed ? initialSelectionRect.Right - resizedWidth : initialSelectionRect.Left - delta.X;

                        startPoint.Y = top + originNewDeltaY;
                        startPoint.X = left + originNewDeltaX;
                        endPoint.Y = top + endNewDeltaY;
                        endPoint.X = left + endNewDeltaX;
                    }


                    if (pointIndex == 6)
                    {
                        startPoint.Y = initialSelectionRect.Top + originNewDeltaY;
                        startPoint.X = Editor.figures[i].PathCopy.GetBounds().Left;
                        endPoint.Y = initialSelectionRect.Top + endNewDeltaY;
                        endPoint.X = Editor.figures[i].PathCopy.GetBounds().Right;
                    }


                    if (pointIndex == 7)
                    {
                        startPoint.Y = initialSelectionRect.Top + originNewDeltaY;
                        startPoint.X = initialSelectionRect.Left + originNewDeltaX;
                        endPoint.Y = initialSelectionRect.Top + endNewDeltaY;
                        endPoint.X = initialSelectionRect.Left + endNewDeltaX;
                    }

                    Editor.figures[i].initCalculations(startPoint, endPoint);
                }
                form.Invalidate();
            }


        }

    }
}
