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
using MiniGraphicEditor.Classes;
using MiniGraphicEditor.Classes.Figures;


namespace MiniGraphicEditor
{
    public partial class EditorForm : Form
    {
        Editor Editor;

        Button[] figureButtons;
        
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
            Editor.init();

            figureButtons = new Button[Editor.registeredFigures.Length];

            for (i = 0; i < Editor.registeredFigures.Length; i++)
            {
                int row = i / Editor.buttonFigureColumns;
                int col = i % Editor.buttonFigureColumns;

                figureButtons[i] = new Button();
                figureButtons[i].Location = new Point(5 + (col * (Editor.buttonFigureSize + Editor.buttonFigureMarginRight)), 418 + (row * (Editor.buttonFigureSize + Editor.buttonFigureMarginRight)));
                figureButtons[i].Size = new Size(Editor.buttonFigureSize, Editor.buttonFigureSize);
                figureButtons[i].BackgroundImageLayout = ImageLayout.Stretch;
                figureButtons[i].BackgroundImage = Image.FromFile("icons/" + Editor.registeredFigures[i].Name + ".png");
                figureButtons[i].Visible = true;
                figureButtons[i].FlatStyle = FlatStyle.Flat;
                figureButtons[i].FlatAppearance.BorderSize = 1;

                if( i == Editor.selectedFigureIndex )
                {
                    figureButtons[i].FlatAppearance.BorderColor = Color.Red;
                } else
                {
                    figureButtons[i].FlatAppearance.BorderColor = Color.Black;
                }
                
                panel1.Controls.Add(figureButtons[i]);
                figureButtons[i].Click += new EventHandler(this.selectFigureButton_Click);
            }
            
        }

        int i;

 
        PointF  u;
        Graphics g;


        private void EditorForm_Shown(object sender, EventArgs e)
        {
           Editor.currentFigure.BorderColor = borderColorButton.BackColor;
           Editor.currentFigure.FillColor = fillColorButton.BackColor;

            

        }

        private void selectFigureButton_Click(object sender, EventArgs e)
        {
            for (i = 0; i < figureButtons.Length; i++)
            {
                if(sender as Button == figureButtons[i])
                {
                    Editor.selectedFigureIndex = i;
                    Editor.resetSelectedFigure();
                    figureButtons[i].FlatAppearance.BorderColor = Color.Red;
                } else
                {
                    figureButtons[i].FlatAppearance.BorderColor = Color.Black;
                }
            
            }
        }


        private void updateEditorUIProps(object sender, EventArgs e)
        {
            Editor.uiProps.originPoint.X = (float)numericUpDown1.Value;
            Editor.uiProps.originPoint.Y = (float)numericUpDown2.Value;
            Editor.uiProps.wd = (float)numericUpDown4.Value;
            Editor.uiProps.hg = (float)numericUpDown3.Value;
            Editor.uiProps.endPoint.X = Editor.uiProps.originPoint.X + Editor.uiProps.wd;
            Editor.uiProps.endPoint.Y = Editor.uiProps.originPoint.Y + Editor.uiProps.hg;
        }

        private void createFigureFromUIButton_Click(object sender, EventArgs e)
        {
           Editor.currentFigure.calculatePoints(Editor.uiProps.originPoint, Editor.uiProps.endPoint);
           Editor.currentFigure.BorderColor = borderColorButton.BackColor;
           Editor.currentFigure.FillColor   = fillColorButton.BackColor;
           Editor.currentFigure.Thickness = thicknessSpinner.Value;

           Editor.addFigure(Editor.currentFigure);
          
           Invalidate();
        }

        private void createFigureByMouseButton_Click(object sender, EventArgs e)
        {
 
            Editor.state = States.DRAW_VIA_MOUSE;
            Cursor = Cursors.Cross;
            if (Editor.paths.Length > 0)
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
                u = e.Location;
                return;
            }

            Editor.pathsCopy = new PointF[Editor.paths.Length][];
            

            if(Editor.Resizer.checkIfPointWasClicked(e))
            {
                Cursor = Cursors.SizeAll;
            }



            if (!(Editor.paths.Length > 0))
            {
                // Если еще не были созданые фигиры, то выходим
                return;
            }

            for (i = 0; i < Editor.paths.Length; i++)
            {
                Editor.pathsCopy[i] = Editor.paths[i].PathPoints;
                if (Editor.figures[i].Selected && Editor.paths[i].IsVisible(e.Location))
                {
                    Editor.state = States.MOVE_SELECTION;
                }
            }
        }

   

        private void EditorForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (Editor.paths.Length > 0 && Editor.state == States.MOVE_SELECTION)
            {
                Editor.moveSelected((e.X - Editor.pressedPoint.X), (e.Y - Editor.pressedPoint.Y));
            }

            if (Editor.state == States.DRAW_VIA_MOUSE)
            {
                Editor.currentFigure.calculatePoints(Editor.pressedPoint, e.Location);

                Editor.addTempPath();
                Invalidate();
            }
            else { 
                
                if (Editor.state != States.RESIZE_SELECTION) return;

                Editor.Resizer.resize(e, this);
            }
        }
        private void EditorForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (Editor.paths.Length > 0 && Editor.mouseDown == false && Editor.state != States.RESIZE_SELECTION && Editor.state != States.MOVE_SELECTION  )
            {
                for (i = Editor.figures.Length - 1; i > -1; i--)
                {
                    if (Editor.paths[i].IsVisible(e.Location))
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
                    Editor.mouseDown  = false; 
                    Cursor = Cursors.Default;
                    Editor.currentFigure.Thickness = thicknessSpinner.Value;
                
                    Editor.addFigure(Editor.currentFigure);

                    Invalidate();
                }
        }
        private void EditorForm_Paint(object sender, PaintEventArgs e)
        {

            g = e.Graphics;
            if (Editor.mouseDown)
            {
                Editor.currentFigure.FillColor = fillColorButton.BackColor;
                Editor.currentFigure.BorderColor = borderColorButton.BackColor;

                g.FillPath(new SolidBrush(fillColorButton.BackColor), Editor.currentPath);
                g.DrawPath(new Pen(borderColorButton.BackColor, (float)thicknessSpinner.Value), Editor.currentPath);
            }

            if (!(Editor.paths.Length > 0)) return;

            for (i = 0; i < Editor.paths.Length; i++)
            {
                g.FillPath(new SolidBrush(Editor.figures[i].FillColor), Editor.paths[i]);
                g.DrawPath(new Pen(Editor.figures[i].BorderColor, (float)Editor.figures[i].Thickness), Editor.paths[i]);
                if (Editor.state == States.RESIZE_SELECTION) Editor.Resizer.calculatePoints();
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
            
            if (e.KeyCode == Keys.Delete && Editor.paths.Length > 0) Editor.deleteSelected();

        }
        private void EditorForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                Editor.shiftPressed = false;
            }
        }
    }
}
