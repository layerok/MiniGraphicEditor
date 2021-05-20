using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using MiniEditor.Classes;

namespace MiniEditor.Classes
{
    class Resizer
    {

        Editor Editor;
        int i, j, k;
        PointF[] points = new PointF[8];

        RectangleF selectionRect;
        public float initialSelectionWidth, initialSelectionHeight;
        public int pointIndex;




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
                        g.FillRectangle(new SolidBrush(Color.Blue), this.points[j].X, this.points[j].Y, 10, 10);
                    }
                    break;
                }
            }
        }

        public void calculatePoints()
        {
            RectangleF rect;
            Editor.currentPath.Reset();
            for (j = 0; j < Editor.figures.Length; j++)
            {
                if (Editor.figures[j].Selected) Editor.currentPath.AddPath(Editor.paths[j], true);
            }

            rect = Editor.currentPath.GetBounds();
            this.selectionRect = rect;


            this.points[0].X = this.points[3].X = this.points[5].X = rect.Left - 10;
            this.points[1].X = this.points[6].X = rect.Left + rect.Width / 2 - 5;
            this.points[2].X = this.points[4].X = this.points[7].X = rect.Right;
            this.points[0].Y = this.points[1].Y = this.points[2].Y = rect.Top - 10;
            this.points[3].Y = this.points[4].Y = rect.Top + rect.Height / 2 - 5;
            this.points[5].Y = this.points[6].Y = this.points[7].Y = rect.Bottom;
        }

        public bool checkIfPointWasClicked(MouseEventArgs e)
        {

            for (i = 0; i < this.points.Length; i++)
            {
                if (
                    e.X >= this.points[i].X &&
                    e.X <= this.points[i].X + 10 &&
                    e.Y >= this.points[i].Y &&
                    e.Y <= this.points[i].Y + 10
                    )
                {
                    pointIndex = i;
                    Editor.state = States.RESIZE_SELECTION;


                    initialSelectionHeight = selectionRect.Height;
                    initialSelectionWidth = selectionRect.Width;


                    for (k = 0; k < Editor.paths.Length; k++)
                    {

                        if (!Editor.figures[k].Selected) continue;
                        Editor.pathsCopy[k] = Editor.paths[k].PathPoints;

                    }
                    return true;
                }
            }

            return false;
        }

        public void resize(MouseEventArgs e, EditorForm form)
        {
            PointF w1, w2, dh;

            dh = new PointF();
            w1 = new PointF();
            w2 = new PointF();

            dh.Y = e.Y - Editor.pressedPoint.Y;
            dh.X = e.X - Editor.pressedPoint.X;


            if (pointIndex == 1 || pointIndex == 6)
            {
                form.Cursor = Cursors.SizeNS;
                if (pointIndex == 1) { w1.Y = selectionRect.Bottom; dh.Y *= -1; if (e.Y >= selectionRect.Bottom - 10) { return; } } else { w1.Y = selectionRect.Top; if (e.Y <= selectionRect.Top + 10) { return; } }

                for (i = 0; i < Editor.paths.Length; i++)
                {
                    if (Editor.figures[i].Selected)
                    {

                        for (k = 0; k < Editor.currentFigure.Points.Length; k++)
                        {
                            Editor.currentFigure.Points[k].Y = (Editor.pathsCopy[i][k].Y - w1.Y) * (initialSelectionHeight + dh.Y) / initialSelectionHeight;
                            Editor.currentFigure.Points[k].Y += w1.Y;
                            Editor.currentFigure.Points[k].X = Editor.pathsCopy[i][k].X;
                        }
                        Editor.paths[i].Reset(); Editor.paths[i].AddPolygon(Editor.currentFigure.Points);
                    }
                    form.Invalidate();
                }
            }
            if (pointIndex == 3 || pointIndex == 4)
            {
                form.Cursor = Cursors.SizeWE;

                if (pointIndex == 3) { w1.X = selectionRect.Right; dh.X *= -1; if (e.X >= selectionRect.Right - 10) { return; } } else { w1.X = selectionRect.Left; if (e.X <= selectionRect.Left + 10) { return; } }

                for (i = 0; i < Editor.paths.Length; i++)
                {
                    if (Editor.figures[i].Selected)
                    {
                        for (k = 0; k < Editor.currentFigure.Points.Length; k++)
                        {
                            Editor.currentFigure.Points[k].X = (Editor.pathsCopy[i][k].X - w1.X) * (initialSelectionWidth + dh.X) / initialSelectionWidth;
                            Editor.currentFigure.Points[k].X += w1.X;
                            Editor.currentFigure.Points[k].Y = Editor.pathsCopy[i][k].Y;
                        }

                        Editor.paths[i].Reset(); Editor.paths[i].AddPolygon(Editor.currentFigure.Points);
                    }
                    form.Invalidate();

                }
            }
            if (pointIndex == 0 || pointIndex == 7)
            {
                form.Cursor = Cursors.SizeNWSE;

                if (pointIndex == 0) { w2.Y = selectionRect.Bottom; w1.X = selectionRect.Right; dh.X *= -1; dh.Y *= -1; if (e.X >= selectionRect.Right - 10 || e.Y >= selectionRect.Bottom - 10) { return; } }
                else { w1.X = selectionRect.Left; w2.Y = selectionRect.Top; if (e.Y <= selectionRect.Top + 10 || e.X <= selectionRect.Left + 10) { return; } }

                for (i = 0; i < Editor.paths.Length; i++)
                {
                    if (Editor.figures[i].Selected)
                    {
                        for (k = 0; k < Editor.currentFigure.Points.Length; k++)
                        {
                            Editor.currentFigure.Points[k].Y = (Editor.pathsCopy[i][k].Y - w2.Y) * (initialSelectionHeight + dh.Y) / initialSelectionHeight;
                            Editor.currentFigure.Points[k].Y += w2.Y;
                            if (Editor.shiftPressed) { dh.X = dh.Y; } else { dh.X = dh.X; }
                            Editor.currentFigure.Points[k].X = (Editor.pathsCopy[i][k].X - w1.X) * (initialSelectionWidth + dh.X) / initialSelectionWidth;
                            Editor.currentFigure.Points[k].X += w1.X;
                        }

                        Editor.paths[i].Reset(); Editor.paths[i].AddPolygon(Editor.currentFigure.Points);
                    }
                    form.Invalidate();

                }
            }
            if (pointIndex == 2 || pointIndex == 5)
            {
                form.Cursor = Cursors.SizeNESW;

                if (pointIndex == 2) { w2.Y = selectionRect.Bottom; w1.X = selectionRect.Left; dh.Y *= -1; if (e.Y >= selectionRect.Bottom - 10 || e.X <= selectionRect.Left + 10) { return; } }
                else { w1.X = selectionRect.Right; w2.Y = selectionRect.Top; dh.X *= -1; if (e.X >= selectionRect.Right - 10 || e.Y <= selectionRect.Top + 10) { return; } }

                for (i = 0; i < Editor.paths.Length; i++)
                {
                    if (Editor.figures[i].Selected)
                    {
                        for (k = 0; k < Editor.currentFigure.Points.Length; k++)
                        {
                            Editor.currentFigure.Points[k].Y = (Editor.pathsCopy[i][k].Y - w2.Y) * (initialSelectionHeight + dh.Y) / initialSelectionHeight;
                            Editor.currentFigure.Points[k].Y += w2.Y;
                            if (Editor.shiftPressed) { dh.X = dh.Y; } else { dh.X = dh.X; }
                            Editor.currentFigure.Points[k].X = (Editor.pathsCopy[i][k].X - w1.X) * (initialSelectionWidth + dh.X) / initialSelectionWidth;
                            Editor.currentFigure.Points[k].X += w1.X;
                        }

                        Editor.paths[i].Reset(); Editor.paths[i].AddPolygon(Editor.currentFigure.Points);
                    }
                    form.Invalidate();

                }
            }
        }

    }
}
