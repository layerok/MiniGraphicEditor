using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MiniGraphicEditor.Classes;
using MiniGraphicEditor.Classes.Figures;


namespace MiniGraphicEditor
{
    public partial class EditorForm : Form
    {
        Editor Editor;



        
        PointF handleCenterPoint;

        
        GraphicsPath rotButton;


        double pi = Math.PI / 180;

        public EditorForm()
        {
            InitializeComponent();
            Editor = new Editor(this);
            Editor.registerFigure(typeof(Lightning));
            Editor.registerFigure(typeof(Arrow));
            Editor.registerFigure(typeof(ArrowFlatEnd));
            Editor.registerFigure(typeof(Trapezoid));
            Editor.registerFigure(typeof(DialogBox));
            Editor.registerFigure(typeof(Butterfly));
            Editor.registerFigure(typeof(Hedgehog));
            Editor.registerFigure(typeof(Turn));
            Editor.registerFigure(typeof(Radio));
            Editor.registerFigure(typeof(Smile));
            Editor.registerFigure(typeof(Line));
            Editor.registerFigure(typeof(Ellipse));
            Editor.init();


            rotButton = new GraphicsPath();
            rotButton.AddEllipse(-20, -20, 40, 40);

            

        }

        int i, k, j;
        Graphics g;


        private void EditorForm_Shown(object sender, EventArgs e)
        {
            Editor.currentFigure.BorderColor = borderColorButton.BackColor;
            Editor.currentFigure.FillColor = fillColorButton.BackColor;
        }

        

        private void updateEditorUIProps(object sender, EventArgs e)
        {
            Editor.uiProps.originPoint.X = (float)numericUpDown1.Value;
            Editor.uiProps.originPoint.Y = (float)numericUpDown2.Value + this.panel1.Height;
            Editor.uiProps.wd = (float)numericUpDown4.Value;
            Editor.uiProps.hg = (float)numericUpDown3.Value;
            Editor.uiProps.endPoint.X = Editor.uiProps.originPoint.X + Editor.uiProps.wd;
            Editor.uiProps.endPoint.Y = Editor.uiProps.originPoint.Y + Editor.uiProps.hg;
        }

        private void createFigureFromUIButton_Click(object sender, EventArgs e)
        {
            Editor.currentFigure.initCalculations(Editor.uiProps.originPoint, Editor.uiProps.endPoint);
            Editor.currentFigure.BorderColor = borderColorButton.BackColor;
            Editor.currentFigure.FillColor = fillColorButton.BackColor;
            Editor.currentFigure.Thickness = thicknessSpinner.Value;

            Editor.addFigure(Editor.currentFigure);

            Invalidate();
        }

        private void createFigureByMouseButton_Click(object sender, EventArgs e)
        {

            Editor.state = States.DRAW_VIA_MOUSE;
            Cursor = Cursors.Cross;
            if (Editor.figures.Length > 0)
            {
                for (i = 0; i < Editor.figures.Length; i++) Editor.figures[i].Selected = false;
                Invalidate();
            }
        }

        private void borderColorButton_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = borderColorButton.BackColor;
            colorDialog1.ShowDialog();
            borderColorButton.BackColor = colorDialog1.Color;

            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (!Editor.figures[i].Selected) continue;
                Editor.figures[i].BorderColor = borderColorButton.BackColor;
            }

            Invalidate();
        }

        private void fillColorButton_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = fillColorButton.BackColor;
            colorDialog1.ShowDialog();
            fillColorButton.BackColor = colorDialog1.Color;

            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (!Editor.figures[i].Selected) continue;
                Editor.figures[i].FillColor = fillColorButton.BackColor;
            }

            Invalidate();
        }

        private void updateThickness_ValueChanged(object sender, EventArgs e)
        {
            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Selected) { Editor.figures[i].Thickness = thicknessSpinner.Value; }
            }

            Invalidate();
        }


        private void EditorForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Сохраняем точку где была изначально нажата мышь, она понадобится при перемещении, ресайзе
            Editor.pressedPoint = e.Location;

            // При каждом нажатии мышки копируему существующие фигуры, потому что клоны понадобятся при ресайзе и сдвиге
            Editor.cloneFigures();

            
            // Проверяем состояние редактора, состояние ниже активизируется при нажатии кнопки "Рисовать с помощью мышки"
            if (Editor.state == States.DRAW_VIA_MOUSE)
            {
                // Выходим и начинаем рисовать фигуру с помощью мышы
                Editor.mouseDown = true;
                return;
            }

            // Проверяем была ли нажата точка ресайза
            if (Editor.Resizer.checkIfPointWasClicked(e))
            {
                Cursor = Cursors.SizeAll;
                Editor.state = States.RESIZE_SELECTION;
                
                return;
            }

            // Проверяем, что в текущем выделении находится 1 фигура
            if (Editor.selectedAmount == 1)
            {
                // Проверяем был ли клик по ползунку вращения
                if (rotButton.IsVisible(e.X - handleCenterPoint.X, e.Y - handleCenterPoint.Y))
                {
                    // Сохраняем состояние, по этому состоянию при движении мышки будет понятно, что мы вращаем фигуру
                    Editor.state = States.ROTATE_SELECTION_BY_MOUSE;
                    return;
                }
            }

            // Проверяем если клик пришелся по выделенной фигуре
            // В таком случае сохраняем состояние, по которому будет понятно при движении мышки, что мы перемещаем выделенные фигуры
            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Path.IsVisible(e.Location))
                {
                    Editor.state = States.FIGURE_CLICKED;
                }
            }
        }



        private void EditorForm_MouseMove(object sender, MouseEventArgs e)
        {
            // Вращаем выделенную фигуру ползунком вращенмя
            if (Editor.state == States.ROTATE_SELECTION_BY_MOUSE)
            {
                float angle = Editor.getNewAngle(e.Location, Editor.figures[Editor.selectedIndex].CenterPoint);
                Editor.figures[Editor.selectedIndex].Angle = (int)angle;
                Editor.figures[Editor.selectedIndex].initCalculations(Editor.figures[Editor.selectedIndex].OriginPoint, Editor.figures[Editor.selectedIndex].EndPoint);
                Invalidate();
                return;
            }

            // Двигаем выделенные фигуры
            if (Editor.state == States.FIGURE_CLICKED)
            {
                Editor.state = States.MOVE_SELECTION;
            }

            if (Editor.state == States.MOVE_SELECTION)
            {
                Editor.moveSelected((e.X - Editor.pressedPoint.X), (e.Y - Editor.pressedPoint.Y));
            }

            // Рисуем новую фигуру
            if (Editor.state == States.DRAW_VIA_MOUSE)
            {

                PointF p1 = new PointF();
                PointF p2 = new PointF();


                if (Editor.ctrlPressed)
                {
                    // Если была нажата кнопка ctrl, то смещаем фигуры, так чтобы она была в середине курсора
                    float diffX = ((e.Location.X - Editor.pressedPoint.X) / 2);
                    float diffY = ((e.Location.Y - Editor.pressedPoint.Y) / 2);

                    p1.X = Editor.pressedPoint.X + diffX;
                    p1.Y = Editor.pressedPoint.Y + diffY;

                    p2.X = e.Location.X + diffX;
                    p2.Y = e.Location.Y + diffY;
                }
                else
                {
                    // Курсор в левом верхем углу фигуры
                    p1 = Editor.pressedPoint;
                    p2 = e.Location;
                }

                // Для просчета кординат фигуры, нужно знать только 2 точки
                // Это точка где была изначально нажата мышка
                // И точка где была отжата мышка
                Editor.currentFigure.initCalculations(p1, p2);

                Invalidate();
                return;
            }

            // Ресайзим фигуру
            if (Editor.state == States.RESIZE_SELECTION)
            {
                // Для ресайза понадобится только знать где была изначальна нажата кнопка мышки, это мы сохраняем в Editor.pressedPoint
                // И где была отжата кнопка мышки. Это мы достанем из e.Location
                Editor.Resizer.resize(e);
            }

            
        }
        private void EditorForm_MouseUp(object sender, MouseEventArgs e)
        {


            Cursor = Cursors.Default;


            if (Editor.figures.Length > 0 && Editor.state == States.FIGURE_CLICKED)
            {
                Editor.state = States.IDLE;
                for (i = Editor.figures.Length - 1; i > -1; i--)
                {
                    if (Editor.figures[i].Path.IsVisible(e.Location))
                    {
                        Editor.figures[i].Selected = !Editor.figures[i].Selected;
                        Editor.Resizer.calculatePoints();
                        Invalidate(); 
                        return;
                    }
                }
                
               
            } else if (Editor.state != States.RESIZE_SELECTION && Editor.state != States.ROTATE_SELECTION_BY_MOUSE && Editor.state != States.MOVE_SELECTION)
            {
                for (i = 0; i < Editor.figures.Length; i++) Editor.figures[i].Selected = false;
                Invalidate();
            }

            Editor.state = States.IDLE;


            if (Editor.mouseDown)
            {
                
                Cursor = Cursors.Default;
                Editor.currentFigure.Thickness = thicknessSpinner.Value;

                Editor.addFigure(Editor.currentFigure);

                Invalidate();
            }
            Editor.mouseDown = false;
        }
        private void EditorForm_Paint(object sender, PaintEventArgs e)
        {
            int rotateSpinnerValue = 0;
            g = e.Graphics;
            if (Editor.mouseDown)
            {
                Editor.currentFigure.FillColor = fillColorButton.BackColor;
                Editor.currentFigure.BorderColor = borderColorButton.BackColor;

                g.FillPath(new SolidBrush(fillColorButton.BackColor), Editor.currentFigure.Path);
                g.DrawPath(new Pen(borderColorButton.BackColor, (float)thicknessSpinner.Value), Editor.currentFigure.Path);
            }
            Editor.selectedAmount = 0;
            if (!(Editor.figures.Length > 0)) return;

            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Selected)
                {
                    Editor.selectedAmount++;
                    Editor.selectedIndex = i;
                    rotateSpinnerValue = Editor.figures[i].Angle;
                }
            }

            for (i = 0; i < Editor.figures.Length; i++)
            {

                g.FillPath(new SolidBrush(Editor.figures[i].FillColor), Editor.figures[i].Path);
                g.DrawPath(new Pen(Editor.figures[i].BorderColor, (float)Editor.figures[i].Thickness), Editor.figures[i].Path);
                if (Editor.state == States.RESIZE_SELECTION) Editor.Resizer.calculatePoints();
            }





            if (Editor.selectedAmount == 1)
            {
                // Убираем обработчик события, чтобы все выделиные фигуры не приняли угол назначенный в спиннере угла
                rotateSpinner.ValueChanged -= rotateSpinner_ValueChanged;
                rotateSpinner.Value = rotateSpinnerValue;
                rotateSpinner.ValueChanged += rotateSpinner_ValueChanged;
                rotateSpinner.Enabled = true;

                float hg = Editor.Resizer.selectionRect.Height;
                float wd = Editor.Resizer.selectionRect.Width;



                float rotateCircleRadius = Math.Max((wd + 150) / 2, (hg + 150) / 2);

                float xOffset = (rotateCircleRadius * 2 - wd) / 2;
                float yOffset = (rotateCircleRadius * 2 - hg) / 2;

                g.DrawEllipse(new Pen(Color.Red, 0), Editor.Resizer.selectionRect.Left - xOffset, Editor.Resizer.selectionRect.Top - yOffset, rotateCircleRadius * 2, rotateCircleRadius * 2);


                handleCenterPoint = Editor.getHandleCenterPoint(Editor.figures[Editor.selectedIndex].CenterPoint, rotateCircleRadius, Editor.figures[Editor.selectedIndex].Angle);

                g.TranslateTransform(handleCenterPoint.X, handleCenterPoint.Y);

                g.FillPath(new SolidBrush(Color.Green), rotButton);

                g.ResetTransform();

            }
            else
            {
                rotateSpinner.Enabled = false;
                rotateSpinner.ValueChanged -= rotateSpinner_ValueChanged;
                rotateSpinner.Value = 0;
                rotateSpinner.ValueChanged += rotateSpinner_ValueChanged;

            }

            Editor.Aligner.showButtons();


            Editor.Resizer.showPoints(g);


        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void выделитьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {

            for (i = 0; i < Editor.figures.Length; i++) Editor.figures[i].Selected = true;
            Editor.Resizer.calculatePoints();
            Invalidate();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Editor.deleteSelected();
            Editor.state = States.IDLE;
        }

        private void EditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                Editor.shiftPressed = true;
            }

            if (e.KeyCode == Keys.ControlKey)
            {
                Editor.ctrlPressed = true;
            }

            if (e.KeyCode == Keys.Delete && Editor.figures.Length > 0) Editor.deleteSelected(); Editor.state = States.IDLE;

        }
        private void EditorForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                Editor.shiftPressed = false;
            }

            if (e.KeyCode == Keys.ControlKey)
            {
                Editor.ctrlPressed = false;
            }
        }

        private void rotateSpinner_ValueChanged(object sender, EventArgs e)
        {
            int deg = (int)rotateSpinner.Value;

            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (!Editor.figures[i].Selected) continue;

                Editor.figures[i].PreviousAngle = Editor.figures[i].Angle;
                Editor.figures[i].Angle = deg;
                Editor.figures[i].initCalculations(Editor.figures[i].OriginPoint, Editor.figures[i].EndPoint);
                Editor.Resizer.calculatePoints();
            }

            Editor.state = States.ROTATE_SELECTION;

            Invalidate();
        }
    }
}
