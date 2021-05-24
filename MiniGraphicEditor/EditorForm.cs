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

            Button[] figureButtons;

            Button[] alignmentButtons = new Button[4];

            PointF handleCenterPoint;

            String[] alignmentButtonsText = new string[] { "←", "↑", "→", "↓" };
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

                figureButtons = new Button[Editor.registeredFigures.Length];

                for (i = 0; i < alignmentButtons.Length; i++)
                {
                    alignmentButtons[i] = new Button();
                    alignmentButtons[i].Location = new Point(0, 0);
                    alignmentButtons[i].Text = alignmentButtonsText[i];
                    alignmentButtons[i].Size = new Size(30, 30);
                    alignmentButtons[i].BackgroundImageLayout = ImageLayout.Stretch;
                    alignmentButtons[i].Visible = false;
                    alignmentButtons[i].FlatStyle = FlatStyle.Flat;
                    alignmentButtons[i].FlatAppearance.BorderSize = 1;

                    this.Controls.Add(alignmentButtons[i]);
                    alignmentButtons[i].Click += new EventHandler(this.alignFigureButton_Click);
                }

                for (i = 0; i < Editor.registeredFigures.Length; i++)
                {
                    int row = i / Editor.buttonFigureColumns;
                    int col = i % Editor.buttonFigureColumns;

                    figureButtons[i] = new Button();
                    figureButtons[i].Location = new Point(Editor.buttonFigureBlockLeft + (col * (Editor.buttonFigureSize + Editor.buttonFigureMarginRight)), Editor.buttonFigureBlockTop + (row * (Editor.buttonFigureSize + Editor.buttonFigureMarginRight)));
                    figureButtons[i].Size = new Size(Editor.buttonFigureSize, Editor.buttonFigureSize);
                    figureButtons[i].BackgroundImageLayout = ImageLayout.Stretch;
                    figureButtons[i].BackgroundImage = Image.FromFile("icons/" + Editor.registeredFigures[i].Name + ".png");
                    figureButtons[i].Visible = true;
                    figureButtons[i].FlatStyle = FlatStyle.Flat;
                    figureButtons[i].FlatAppearance.BorderSize = 1;

                    if (i == Editor.selectedFigureIndex)
                    {
                        figureButtons[i].FlatAppearance.BorderColor = Color.Red;
                    }
                    else
                    {
                        figureButtons[i].FlatAppearance.BorderColor = Color.Black;
                    }

                    panel1.Controls.Add(figureButtons[i]);
                    figureButtons[i].Click += new EventHandler(this.selectFigureButton_Click);
                }

            }

            int i, k, j;
            Graphics g;


            private void EditorForm_Shown(object sender, EventArgs e)
            {
                Editor.currentFigure.BorderColor = borderColorButton.BackColor;
                Editor.currentFigure.FillColor = fillColorButton.BackColor;



            }

            private void alignFigureButton_Click(object sender, EventArgs e)
            {
                for (i = 0; i < alignmentButtons.Length; i++)
                {
                    if (sender as Button == alignmentButtons[i])
                    {
                        //0 - left
                        //1 - top
                        //2 - right
                        //3 - bottom

                        for (k = 0; k < Editor.figures.Length; k++)
                        {
                            if (Editor.figures[k].Selected)
                            {
                                PointF p1 = new PointF();
                                PointF p2 = new PointF();



                                if (i == 0)
                                {
                                    p1.X = Editor.Resizer.selectionRect.Left;
                                    p1.Y = Editor.figures[k].OriginPoint.Y;

                                    p2.X = Editor.Resizer.selectionRect.Left + Editor.figures[k].Width;
                                    p2.Y = Editor.figures[k].OriginPoint.Y + Editor.figures[k].Height;

                                }
                                else if (i == 1)
                                {
                                    p1.X = Editor.figures[k].OriginPoint.X;
                                    p1.Y = Editor.Resizer.selectionRect.Top;

                                    p2.X = Editor.figures[k].OriginPoint.X + Editor.figures[k].Width;
                                    p2.Y = Editor.Resizer.selectionRect.Top + Editor.figures[k].Height;
                                }
                                else if (i == 2)
                                {
                                    p1.X = Editor.Resizer.selectionRect.Right - Editor.figures[k].Width;
                                    p1.Y = Editor.figures[k].OriginPoint.Y;

                                    p2.X = Editor.Resizer.selectionRect.Right;
                                    p2.Y = Editor.figures[k].OriginPoint.Y + Editor.figures[k].Height;
                                }
                                else if (i == 3)
                                {
                                    p1.X = Editor.figures[k].OriginPoint.X;
                                    p1.Y = Editor.Resizer.selectionRect.Bottom - Editor.figures[k].Height;

                                    p2.X = Editor.figures[k].OriginPoint.X + Editor.figures[k].Width;
                                    p2.Y = Editor.Resizer.selectionRect.Bottom;
                                }

                                Editor.figures[k].initCalculations(p1, p2);
                            }
                        }
                    }

                    Editor.Resizer.calculatePoints();

                    Invalidate();

                }
            }

            private void selectFigureButton_Click(object sender, EventArgs e)
            {
                for (i = 0; i < figureButtons.Length; i++)
                {
                    if (sender as Button == figureButtons[i])
                    {
                        Editor.selectedFigureIndex = i;
                        Editor.resetSelectedFigure();
                        figureButtons[i].FlatAppearance.BorderColor = Color.Red;
                    }
                    else
                    {
                        figureButtons[i].FlatAppearance.BorderColor = Color.Black;
                    }

                }
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

                Editor.pressedPoint = e.Location;

                if (Editor.state == States.DRAW_VIA_MOUSE)
                {
                    Editor.mouseDown = true;
                    return;
                }

                if (Editor.selectedAmount == 1)
                {

                    if (rotButton.IsVisible(e.X - handleCenterPoint.X, e.Y - handleCenterPoint.Y))
                    {
                        Editor.state = States.ROTATE_SELECTION_BY_MOUSE;
                        return;
                    }
                }


                if (Editor.Resizer.checkIfPointWasClicked(e))
                {
                    Cursor = Cursors.SizeAll;
                    Editor.Resizer.initialSelectionRect = Editor.Resizer.getRect();
                }



                if (!(Editor.figures.Length > 0))
                {
                    // Если еще не были созданые фигиры, то выходим
                    return;
                }

                for (i = 0; i < Editor.figures.Length; i++)
                {
                    Editor.figures[i].PathCopy = Editor.figures[i].NotTransformedPath;
                    if (Editor.figures[i].Selected && Editor.figures[i].Path.IsVisible(e.Location))
                    {
                        Editor.state = States.MOVE_SELECTION;
                    }
                }
            }



            private void EditorForm_MouseMove(object sender, MouseEventArgs e)
            {

                if (Editor.state == States.ROTATE_SELECTION_BY_MOUSE)
                {
                    float angle = Editor.getNewAngle(e.Location, Editor.figures[Editor.selectedIndex].CenterPoint);
                    Editor.figures[Editor.selectedIndex].Angle = (int)angle;

                    PointF startPoint = new PointF(Editor.figures[Editor.selectedIndex].NotTransformedPath.GetBounds().Left, Editor.figures[Editor.selectedIndex].NotTransformedPath.GetBounds().Top);
                    PointF endPoint = new PointF(Editor.figures[Editor.selectedIndex].NotTransformedPath.GetBounds().Left + Editor.figures[Editor.selectedIndex].NotTransformedPath.GetBounds().Width, Editor.figures[Editor.selectedIndex].NotTransformedPath.GetBounds().Top + Editor.figures[Editor.selectedIndex].NotTransformedPath.GetBounds().Height);

                    Editor.figures[Editor.selectedIndex].initCalculations(startPoint, endPoint);
                    Invalidate();
                    return;
                   }

                if (Editor.figures.Length > 0 && Editor.state == States.MOVE_SELECTION)
                {
                    Editor.moveSelected((e.X - Editor.pressedPoint.X), (e.Y - Editor.pressedPoint.Y));
                }

                if (Editor.state == States.DRAW_VIA_MOUSE)
                {

                    PointF p1 = new PointF();
                    PointF p2 = new PointF();

                    if (Editor.ctrlPressed)
                    {
                        float diffX = ((e.Location.X - Editor.pressedPoint.X) / 2);
                        float diffY = ((e.Location.Y - Editor.pressedPoint.Y) / 2);
                        p1.X = Editor.pressedPoint.X + diffX;
                        p1.Y = Editor.pressedPoint.Y + diffY;

                        p2.X = e.Location.X + diffX;
                        p2.Y = e.Location.Y + diffY;
                    }
                    else
                    {
                        p1 = Editor.pressedPoint;
                        p2 = e.Location;
                    }
                    Editor.currentFigure.initCalculations(p1, p2);

                    Invalidate();
                }
                else
                {

                    if (Editor.state != States.RESIZE_SELECTION) return;

                    Editor.Resizer.resize(e, this);
                }
            }
            private void EditorForm_MouseUp(object sender, MouseEventArgs e)
            {
                if (Editor.figures.Length > 0 && Editor.mouseDown == false && Editor.state != States.RESIZE_SELECTION && Editor.state != States.MOVE_SELECTION && Editor.state != States.ROTATE_SELECTION_BY_MOUSE)
                {
                    for (i = Editor.figures.Length - 1; i > -1; i--)
                    {
                        if (Editor.figures[i].Path.IsVisible(e.Location))
                        {
                            Editor.figures[i].Selected = !Editor.figures[i].Selected;
                            Editor.Resizer.calculatePoints();
                            Invalidate(); return;
                        }
                    }
                    for (i = 0; i < Editor.figures.Length; i++) Editor.figures[i].Selected = false;
                    Invalidate();
                }
                Cursor = Cursors.Default;
                Editor.state = States.IDLE;

                if (Editor.mouseDown)
                {
                    Editor.mouseDown = false;
                    Cursor = Cursors.Default;
                    Editor.currentFigure.Thickness = thicknessSpinner.Value;

                    Editor.addFigure(Editor.currentFigure);

                    Invalidate();
                }
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

                if (Editor.selectedAmount > 1)
                {
                    for (i = 0; i < alignmentButtons.Length; i++)
                    {
                        //
                        alignmentButtons[i].Visible = true;
                        alignmentButtons[i].Location = new Point((int)Editor.Resizer.selectionRect.X - Editor.Resizer.pointSize + (i * 35), (int)Editor.Resizer.selectionRect.Y - (Editor.Resizer.pointSize * 3));


                    }
                }
                else
                {
                    for (i = 0; i < alignmentButtons.Length; i++)
                    {
                        //
                        alignmentButtons[i].Visible = false;

                    }
                }


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

                if (e.KeyCode == Keys.Delete && Editor.figures.Length > 0) Editor.deleteSelected();

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
