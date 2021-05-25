using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGraphicEditor.Classes
{
    class Rotator
    {
        Editor Editor;

        protected Color orbitColor = Config.orbitColor;
        protected Color handleColor = Config.handleColor;
        int i;

        public PointF handleCenterPoint;
        public GraphicsPath handleButton;

        public Rotator(Editor Editor)
        {
            this.Editor = Editor;
            Editor.form.rotateSpinner.ValueChanged += new EventHandler(rotateSpinner_ValueChanged);
        }

        public void createHandleButton()
        {

            handleButton = new GraphicsPath();
            handleButton.AddEllipse(-20, -20, 40, 40);
        }

        public bool checkIfHandleClicked(PointF pressedPoint)
        {
            if (Editor.selectedAmount == 1)
            {
                // Проверяем был ли клик по ползунку вращения
                if (handleButton.IsVisible(pressedPoint.X - handleCenterPoint.X, pressedPoint.Y - handleCenterPoint.Y))
                {
                    // Сохраняем состояние, по этому состоянию при движении мышки будет понятно, что мы вращаем фигуру
                    return true;
                }
            }
            return false;
        }

        public void updateHandleCenterPoint(PointF centerPoint, float radius, float angle)
        {
            handleCenterPoint.X = (float)(centerPoint.X + radius * Math.Cos(angle * Math.PI / 180));
            handleCenterPoint.Y = (float)(centerPoint.Y + radius * Math.Sin(angle * Math.PI / 180));

        }

        public void showHandle()
        {
            if (Editor.selectedAmount == 1)
            {
                // Убираем обработчик события, чтобы все выделиные фигуры не приняли угол назначенный в спиннере угла
                Editor.form.rotateSpinner.ValueChanged -= rotateSpinner_ValueChanged;
                Editor.form.rotateSpinner.Value = (decimal)Editor.uiProps.rotateSpinnerValue;
                Editor.form.rotateSpinner.ValueChanged += this.rotateSpinner_ValueChanged;
                Editor.form.rotateSpinner.Enabled = true;

                float hg = Editor.Resizer.selectionRect.Height;
                float wd = Editor.Resizer.selectionRect.Width;



                float rotateCircleRadius = Math.Max((wd + 150) / 2, (hg + 150) / 2);

                float xOffset = (rotateCircleRadius * 2 - wd) / 2;
                float yOffset = (rotateCircleRadius * 2 - hg) / 2;

                Editor.form.g.DrawEllipse(new Pen(orbitColor, 0), Editor.Resizer.selectionRect.Left - xOffset, Editor.Resizer.selectionRect.Top - yOffset, rotateCircleRadius * 2, rotateCircleRadius * 2);

                Editor.Rotator.updateHandleCenterPoint(Editor.figures[Editor.selectedIndex].CenterPoint, rotateCircleRadius, Editor.figures[Editor.selectedIndex].Angle);


                Editor.form.g.TranslateTransform(Editor.Rotator.handleCenterPoint.X, Editor.Rotator.handleCenterPoint.Y);

                Editor.form.g.FillPath(new SolidBrush(handleColor), Editor.Rotator.handleButton);

                Editor.form.g.ResetTransform();

            }
            else
            {
                Editor.form.rotateSpinner.Enabled = false;
                Editor.form.rotateSpinner.ValueChanged -= rotateSpinner_ValueChanged;
                Editor.form.rotateSpinner.Value = 0;
                Editor.form.rotateSpinner.ValueChanged += rotateSpinner_ValueChanged;

            }
        }

        private void rotateSpinner_ValueChanged(object sender, EventArgs e)
        {
            int deg = (int)Editor.form.rotateSpinner.Value;

            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (!Editor.figures[i].Selected) continue;

                Editor.figures[i].PreviousAngle = Editor.figures[i].Angle;
                Editor.figures[i].Angle = deg;
                Editor.figures[i].initCalculations(Editor.figures[i].OriginPoint, Editor.figures[i].EndPoint);
                Editor.Resizer.calculatePoints();
            }

            Editor.state = States.ROTATE_SELECTION;

            Editor.form.Invalidate();
        }

        public void rotate(float angle, int figureIndex)
        {
            
            Editor.figures[figureIndex].Angle = (int)angle;
            Editor.figures[figureIndex].initCalculations(Editor.figures[figureIndex].OriginPoint, Editor.figures[figureIndex].EndPoint);
        }


        public float getNewAngle(PointF b2, PointF center)
        {
            float angle;

            const double pi2 = 180 / Math.PI;

            angle = -(float)(Math.Atan2(center.Y - center.Y, center.X + 10 - center.X) * pi2 - Math.Atan2(b2.Y - center.Y, b2.X - center.X) * pi2);

            return angle;
        }

        public void rotateSelected(PointF currentMousePoint)
        {
            float angle = Editor.Rotator.getNewAngle(currentMousePoint, Editor.figures[Editor.selectedIndex].CenterPoint);
            Editor.Rotator.rotate(angle, Editor.selectedIndex);
        }



    }
}
