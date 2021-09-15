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

        public EditorForm()
        {
            InitializeComponent();

            // Чтобы фигура появилась как кнопка в UI формы
            // Ее нужно зарегистировать
            // Чтобы изображение подгрузилось как задний фон кнопки, изображение нужно разместить в папке bin/debug/icons с названием {className}.png
            // Например для класса Lightning нужно добавить изображение с именем Lightining.png

            Editor = new Editor(this);
            Editor.registerFigure(typeof(Lightning));
            Editor.registerFigure(typeof(Arrow));
            Editor.registerFigure(typeof(ArrowFlatEnd));
            Editor.registerFigure(typeof(Trapezoid));
            Editor.registerFigure(typeof(Trapezoid2));
            Editor.registerFigure(typeof(DialogBox));
            Editor.registerFigure(typeof(Butterfly));
            Editor.registerFigure(typeof(Butterfly2));
            Editor.registerFigure(typeof(Hedgehog));
            Editor.registerFigure(typeof(Turn));
            Editor.registerFigure(typeof(Radio));
            Editor.registerFigure(typeof(Smile));
            Editor.registerFigure(typeof(Line));
            Editor.registerFigure(typeof(Ellipse));
            Editor.registerFigure(typeof(Drop));
            Editor.registerFigure(typeof(ClosedBrokenLine));
            Editor.registerFigure(typeof(Vase));
            Editor.init();

            // Передаваемые параметры не выполняют никакой функции, передаю их просто чтобы запустить функцию
            updateEditorUIProps(new Button(), new EventArgs());
        }

        int i;
        public Graphics g;


        private void EditorForm_Shown(object sender, EventArgs e)
        {
            Editor.currentFigure.BorderColor = borderColorButton.BackColor;
            Editor.currentFigure.FillColor = fillColorButton.BackColor;
        }

        

        private void updateEditorUIProps(object sender, EventArgs e)
        {
            Editor.uiProps.originPoint.X = (float)numericUpDown1.Value;
            Editor.uiProps.originPoint.Y = (float)numericUpDown2.Value + this.panel1.Height + this.menuStrip1.Height;
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

            // При каждом нажатии мыши копируему существующие фигуры, потому что клоны понадобятся при ресайзе и сдвиге
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

            // Проверям если был нажат ползунок
            if (Editor.Rotator.checkIfHandleClicked(Editor.pressedPoint))
            {
                Editor.state = States.ROTATE_SELECTION_BY_MOUSE;
                return;
            }

            // Проверяем если клик пришелся по выделенной фигуре
            // В таком случае сохраняем состояние, по которому будет понятно при движении мышки, что мы перемещаем выделенные фигуры
            if(Editor.Selector.checkIfSelectedFigureClicked(Editor.pressedPoint))
            {
                Editor.state = States.SELECTED_FIGURE_CLICKED;
            }
        }



        private void EditorForm_MouseMove(object sender, MouseEventArgs e)
        {
            // Вращаем выделенную фигуру ползунком вращенмя
            if (Editor.state == States.ROTATE_SELECTION_BY_MOUSE)
            {
                Editor.Rotator.rotateSelected(e.Location);
                Invalidate();
                return;
            }

            // Если фигура была нажата и мышку начали двигать
            if (Editor.state == States.SELECTED_FIGURE_CLICKED)
            {
                Editor.state = States.MOVE_SELECTION;
            }

            // Двигаем выделенные фигуры
            if (Editor.state == States.MOVE_SELECTION)
            {
                Cursor = Cursors.Hand;
                Editor.Mover.moveSelected((e.X - Editor.pressedPoint.X), (e.Y - Editor.pressedPoint.Y));
                Editor.Resizer.calculatePoints();
                Invalidate();
                return;
            }

            // Рисуем новую фигуру
            if (Editor.state == States.DRAW_VIA_MOUSE)
            {

                Editor.Drawer.drawViaMouse(Editor.pressedPoint, e.Location);
                Invalidate();
                return;
            }

            // Ресайзим фигуру
            if (Editor.state == States.RESIZE_SELECTION)
            {
                // Для ресайза понадобится только знать где была изначальна нажата кнопка мышки, это мы сохраняем в Editor.pressedPoint
                // И где была отжата кнопка мышки. Это мы достанем из e.Location
                Editor.Resizer.resize(e);
                Invalidate();
                return;
            }

            
        }
        private void EditorForm_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            // Если мышка была отжата на фигуре с выделеним, то снимаем выделение
            // Но в случае если мы передвигали фигуру - то мы не хотим, чтобы с фигуры снималось выделения
            // Поэтому проверям состояние
            if (Editor.state != States.MOVE_SELECTION && Editor.state != States.RESIZE_SELECTION && Editor.state != States.ROTATE_SELECTION_BY_MOUSE)
            {
                bool selectionWasToggled = Editor.Selector.toggle(e.Location);
                if(selectionWasToggled)
                {
                    Editor.Resizer.calculatePoints();
                    Invalidate();
                    Editor.state = States.IDLE;
                    return;
                }
                
            }

            // В случае, если мышка была отжата и состояние до этого момента не как не изменилось, значит нужно снять выделение со всех фигур
            if (Editor.state == States.IDLE)
            {
                Editor.Selector.resetAll();
                Invalidate();
                return;
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
            
            g = e.Graphics;
            if (Editor.mouseDown)
            {
                Editor.currentFigure.FillColor = fillColorButton.BackColor;
                Editor.currentFigure.BorderColor = borderColorButton.BackColor;

                g.FillPath(new SolidBrush(fillColorButton.BackColor), Editor.currentFigure.Path);
                g.DrawPath(new Pen(borderColorButton.BackColor, (float)thicknessSpinner.Value), Editor.currentFigure.Path);
            }
            
            if (!(Editor.figures.Length > 0)) return;

            Editor.Selector.updateMeta();

            for (i = 0; i < Editor.figures.Length; i++)
            {
                g.FillPath(new SolidBrush(Editor.figures[i].FillColor), Editor.figures[i].Path);
                g.DrawPath(new Pen(Editor.figures[i].BorderColor, (float)Editor.figures[i].Thickness), Editor.figures[i].Path);
                if (Editor.state == States.RESIZE_SELECTION) Editor.Resizer.calculatePoints();
            }





            Editor.Rotator.showHandle();
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

            if (e.KeyCode == Keys.Delete && Editor.figures.Length > 0)
            {
                Editor.deleteSelected(); Editor.state = States.IDLE;
            }

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


    }
}
