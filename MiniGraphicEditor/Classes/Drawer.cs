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
    class Drawer
    {
        Editor Editor;
        int i;


        public Drawer(Editor Editor)
        {
            this.Editor = Editor;
        }

        public void drawViaMouse(PointF originPoint, PointF endPoint)
        {
            PointF p1 = new PointF();
            PointF p2 = new PointF();


            if (Editor.ctrlPressed)
            {
                // Если была нажата кнопка ctrl, то смещаем фигуры, так чтобы она была в середине курсора
                float diffX = ((endPoint.X - originPoint.X) / 2);
                float diffY = ((endPoint.Y - originPoint.Y) / 2);

                p1.X = originPoint.X + diffX;
                p1.Y = originPoint.Y + diffY;

                p2.X = endPoint.X + diffX;
                p2.Y = endPoint.Y + diffY;
            }
            else
            {
                // Курсор в левом верхем углу фигуры
                p1 = originPoint;
                p2 = endPoint;
            }

            // Для просчета кординат фигуры, нужно знать только 2 точки
            // Это точка где была изначально нажата мышка
            // И точка где была отжата мышка
            Editor.currentFigure.initCalculations(p1, p2);
        }

    }
}
