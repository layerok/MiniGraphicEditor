using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGraphicEditor.Classes
{
    class Selector
    {
        Editor Editor;
        int i;

        public Selector(Editor Editor)
        {
            this.Editor = Editor;
        }

        public void updateMeta()
        {
            Editor.selectedAmount = 0;
            Editor.uiProps.rotateSpinnerValue = 0;
            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Selected)
                {
                    Editor.selectedAmount++;
                    Editor.selectedIndex = i;
                    Editor.uiProps.rotateSpinnerValue = Editor.figures[i].Angle;
                }
            }
        }

        public bool checkIfSelectedFigureClicked(PointF pressedPoint)
        {
            for (i = 0; i < Editor.figures.Length; i++)
            {
                // Проверка на то, что точка находится внутри фигуры или на ее границе и фигура выделенна
                if (Editor.figures[i].Selected && isInsideFigure(i,pressedPoint))
                {
                    return true;
                }
            }

            return false;
        }

        public bool isInsideFigure(int figureIndex, PointF pressedPoint)
        {
            Pen pen = new Pen(Editor.figures[figureIndex].BorderColor, (float)Editor.figures[figureIndex].Thickness);

            // Точка находится внутри фигуры
            if (Editor.figures[figureIndex].Path.IsVisible(pressedPoint))
            {
                return true;
            }

            // Точка находится на границе фигуры
            if(Editor.figures[figureIndex].Path.IsOutlineVisible(pressedPoint.X, pressedPoint.Y, pen)) {
                return true;
            }
            return false;
        }

        public bool toggle(PointF mouseUpPoint)
        {
            for (i = Editor.figures.Length - 1; i > -1; i--)
            {
                if (isInsideFigure(i,mouseUpPoint))
                {
                    Editor.figures[i].Selected = !Editor.figures[i].Selected;
                    return true;
                }
            }
            return false;
        }

        public void resetAll()
        {
            for (i = 0; i < Editor.figures.Length; i++) Editor.figures[i].Selected = false;
        }

    }
}
