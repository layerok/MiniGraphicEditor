using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGraphicEditor.Classes.Figures
{
    class Pie : Figure
    {

        public Pie()
        {
            _points = new PointF[3];
        }

        public override void calculatePoints(PointF originPoint, PointF endPoint)
        {
            // Расположение точек линий между точкой(originPoint) где пользователь решил рисовать и точко(endPoint) где пользователь закончил рисовать
            Points[0].Y = originPoint.Y;
            Points[0].X = originPoint.X + (_width / 5 * 2);
            Points[1].X = endPoint.X;
            Points[1].Y = endPoint.Y - (_height/ 2);
            Points[2].Y = endPoint.Y;
            Points[2].X = Points[0].X;

        }

        public override GraphicsPath createPath()
        {

            // Фигура рисуется с помощью 2 линий и дуги

            GraphicsPath path = new GraphicsPath();

            //Последовательность в которой соединются точки линий
            int[] sequanse = new int[3] { 0,1,2 };

            
            // Последовательность точек нужно менять потому, линии иногда не правильно соединяются с дугой, 
            // И из-за этого получается линия которая пересекает всю фигуру

            if (_originPoint.X > _endPoint.X || _originPoint.Y > _endPoint.Y)
            {
                // Если рисуем фигуру влево-вниз либо вверх-вправо 
                sequanse[0] = 2;
                sequanse[1] = 1;
                sequanse[2] = 0;
            }

            if (_originPoint.Y > _endPoint.Y && _originPoint.X > _endPoint.X)
            {
                // Если рисуем фигуру влево-вверх
                sequanse[0] = 0;
                sequanse[1] = 1;
                sequanse[2] = 2;
            }

       

            int wd = (int)(Math.Abs(_width) / 5) * 4;
            int hg = (int)Math.Abs(_height);

            // если ширина и высота дуги равна нулю, то дугу не рисуем, так как это выдаст ошибку
            if (wd != 0 && hg != 0)
            {
                // ширина и высота дуги
                Size size = new Size(wd, hg);

                // точка с которой рисуем дугу
                Point point = new Point();

                int startAngle = 90;

                // Если мы рисуем фигуру в левую сторону, то дуга должна находится с левой стороны, иначе справой
                if(_originPoint.X > _endPoint.X)
                {
                    point.X = (int)_originPoint.X - wd;
                    startAngle = 270;
                } else
                {
                    point.X = (int)_originPoint.X;
                }

                // Если мы рисуем вверх фигуру, то точка откуда мы рисуем дугу будет там где находится мышь, иначе там где была изначальна нажата мышь
                if (_originPoint.Y > _endPoint.Y)
                {
                    point.Y = (int)_endPoint.Y;
                }
                else
                {
                    point.Y = (int)_originPoint.Y;
                }

                Rectangle rect = new Rectangle(point, size);
                path.AddArc(rect, startAngle, 180);
            }

            // Добавляем линии в предопределенной последовательности
            path.AddLine(Points[sequanse[0]], Points[sequanse[1]]);
            path.AddLine(Points[sequanse[1]], Points[sequanse[2]]);



            return path;
        }
    }
}
