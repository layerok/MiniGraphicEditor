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


namespace MiniEditor
{
    public partial class EditorForm : Form
    {
        Editor Editor = new Editor();
        
        public EditorForm()
        {
            InitializeComponent();
        }

        int i, j, k, z, q;

        float t_, width, height, top, wd, hg;
        bool   f2, f3, f_tmp, shift, f4 = false;
        double pi = Math.PI / 180;
        PointF[][] v1;
        PointF[] c = new PointF[8];
        Color[] c1, c2;
        float[] t;
        PointF d, d1, u, w1, w2, dh;

        Color c_1 = Color.Black, c_2 = Color.DarkRed;

        GraphicsPath[] p;
        RectangleF rect;

        private void EditorForm_Shown(object sender, EventArgs e)
        {
           Editor.currentFigure.BorderColor = borderColorButton.BackColor;
           Editor.currentFigure.FillColor = fillColorButton.BackColor;
            t_ = (float)numericUpDown8.Value;
        }


        private void updateEditorUIProps(object sender, EventArgs e)
        {
            Editor.props.originPoint.X = (float)numericUpDown1.Value;
            Editor.props.originPoint.Y = (float)numericUpDown2.Value;
            Editor.props.wd = (float)numericUpDown4.Value;
            Editor.props.hg = (float)numericUpDown3.Value;
            Editor.props.endPoint.X = Editor.props.originPoint.X + Editor.props.wd;
            Editor.props.endPoint.Y = Editor.props.originPoint.Y + Editor.props.hg;
        }

        private void createFigureFromUIButton_Click(object sender, EventArgs e)
        {
           Editor.currentFigure.calculatePoints(Editor.props.originPoint, Editor.props.endPoint);
           Editor.currentFigure.BorderColor = borderColorButton.BackColor;
           Editor.currentFigure.FillColor   = fillColorButton.BackColor;
           Editor.currentFigure.Thickness = (float)numericUpDown8.Value;

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
            if (Editor.paths.Length == 0) { c_1 = borderColorButton.BackColor; return; }
            f3 = false;
            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Selected) { c1[i] = borderColorButton.BackColor; f3 = true; }
            }
            if (f3 == false) c_1 = borderColorButton.BackColor;
            Invalidate();
        }

        private void fillColorButton_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = fillColorButton.BackColor;
            colorDialog1.ShowDialog();
            fillColorButton.BackColor = colorDialog1.Color;
            if (Editor.paths.Length == 0) { c_2 = borderColorButton.BackColor; return; }
            f3 = false;
            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Selected) { c2[i] = fillColorButton.BackColor; f3 = true; }
            }
            if (f3 == false) c_2 = fillColorButton.BackColor;
            Invalidate();
        }



        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Editor.state == States.DRAW_VIA_MOUSE)
            {
                Editor.mouseDown = true;
                d = u = e.Location;
                return;
            }

            v1 = new PointF[Editor.paths.Length][];
            d1 = e.Location;
            for (i = 0; i < 8; i++)
            {
                if (e.X >= c[i].X && e.X <= c[i].X + 10 && e.Y >= c[i].Y && e.Y <= c[i].Y + 10)
                {
                    z = i; f4 = true; Cursor = Cursors.SizeAll; width = rect.Width; height = rect.Height; top = rect.Top;

                    for (k = 0; k < Editor.paths.Length; k++)
                    {

                        if (Editor.figures[k].Selected)
                        {
                            v1[k] = Editor.paths[k].PathPoints;
                        }

                    }
                    return;
                }
            }

            if (Editor.paths.Length > 0)
            {
                for (i = 0; i < Editor.paths.Length; i++)
                {
                    v1[i] = Editor.paths[i].PathPoints;
                    if (Editor.figures[i].Selected && Editor.paths[i].IsVisible(e.Location))
                    {
                        f2 = true;
                    }
                }
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Editor.paths.Length > 0 &&f2 )
            {
                for (i = Editor.paths.Length - 1; i > -1; i--)
                {
                        Cursor = Cursors.Hand;
                        if (Editor.figures[i].Selected)
                        {
                            for ( j= 0; j <Editor.currentFigure.Points.Length; j++)
                            {
                               Editor.currentFigure.Points[j].X = v1[i][j].X+(e.X - d1.X);
                               Editor.currentFigure.Points[j].Y = v1[i][j].Y+(e.Y - d1.Y);
                               
                            }
                            Editor.paths[i].Reset(); Editor.paths[i].AddPolygon(Editor.currentFigure.Points);
                            kvadrati();
                            Invalidate();
                        }  
                }
            }
            if (Editor.state == States.DRAW_VIA_MOUSE)
            {
                Editor.currentFigure.calculatePoints(d, e.Location);

                Editor.addTempPath();
                Invalidate();
            }
            else { 
                
                if (f4 == false) return;
                
                 dh.Y = e.Y - d1.Y;
                 dh.X = e.X - d1.X;
                if (z == 1 || z == 6)
                {
                    Cursor = Cursors.SizeNS;
                    if (z == 1) { w1.Y = rect.Bottom; dh.Y *= -1; if (e.Y >= rect.Bottom -10) { return; } } else { w1.Y = rect.Top; if (e.Y <= rect.Top+10) { return; } } 

                    for (i = 0; i < Editor.paths.Length; i++)
                    {
                        if (Editor.figures[i].Selected)
                        {

                            for (k = 0; k <Editor.currentFigure.Points.Length; k++)
                            {
                               Editor.currentFigure.Points[k].Y = (v1[i][k].Y - w1.Y) * (height + dh.Y) / height;
                               Editor.currentFigure.Points[k].Y += w1.Y;
                               Editor.currentFigure.Points[k].X = v1[i][k].X; 
                            }
                            Editor.paths[i].Reset(); Editor.paths[i].AddPolygon(Editor.currentFigure.Points);
                        }
                        Invalidate();
                    }
                }
                if (z == 3 || z == 4)
                {
                    Cursor = Cursors.SizeWE;
                    
                    if (z == 3) { w1.X = rect.Right; dh.X *= -1; if (e.X >= rect.Right -10) { return; } } else { w1.X = rect.Left; if (e.X <= rect.Left + 10) { return; } }
                    
                    for (i = 0; i < Editor.paths.Length; i++)
                    {
                        if (Editor.figures[i].Selected)
                        {
                            for (k = 0; k <Editor.currentFigure.Points.Length; k++)
                            {
                               Editor.currentFigure.Points[k].X = (v1[i][k].X - w1.X) * (width + dh.X) / width;
                               Editor.currentFigure.Points[k].X += w1.X;
                               Editor.currentFigure.Points[k].Y = v1[i][k].Y;
                            }

                            Editor.paths[i].Reset(); Editor.paths[i].AddPolygon(Editor.currentFigure.Points);
                        }
                        Invalidate();

                    }
                }
                if (z == 0 || z == 7)
                {
                    Cursor = Cursors.SizeNWSE;
                    
                    if (z == 0) { w2.Y = rect.Bottom; w1.X = rect.Right; dh.X *= -1;dh.Y *= -1; if (e.X >= rect.Right - 10 || e.Y >= rect.Bottom - 10) { return; } }
                    else { w1.X = rect.Left; w2.Y = rect.Top; if (e.Y <= rect.Top + 10|| e.X <= rect.Left + 10) { return; } }
                    
                    for (i = 0; i < Editor.paths.Length; i++)
                    {
                        if (Editor.figures[i].Selected)
                        {
                            for (k = 0; k <Editor.currentFigure.Points.Length; k++)
                            {
                               Editor.currentFigure.Points[k].Y = (v1[i][k].Y - w2.Y) * (height + dh.Y) / height;
                               Editor.currentFigure.Points[k].Y += w2.Y;
                                if (shift) { dh.X = dh.Y; } else { dh.X = dh.X; }
                               Editor.currentFigure.Points[k].X = (v1[i][k].X - w1.X) * (width + dh.X) / width;
                               Editor.currentFigure.Points[k].X += w1.X;
                            }

                            Editor.paths[i].Reset(); Editor.paths[i].AddPolygon(Editor.currentFigure.Points);
                        }
                        Invalidate();

                    }
                }
                if (z == 2 || z == 5)
                {
                    Cursor = Cursors.SizeNESW;
                    
                    if (z == 2) { w2.Y = rect.Bottom; w1.X = rect.Left;dh.Y *= -1; if (e.Y >= rect.Bottom - 10|| e.X <= rect.Left + 10) { return; } }
                    else { w1.X = rect.Right; w2.Y = rect.Top; dh.X *= -1; if (e.X >= rect.Right - 10|| e.Y <= rect.Top + 10) { return; } }

                    for (i = 0; i < Editor.paths.Length; i++)
                    {
                        if (Editor.figures[i].Selected)
                        {
                            for (k = 0; k <Editor.currentFigure.Points.Length; k++)
                            {
                               Editor.currentFigure.Points[k].Y = (v1[i][k].Y - w2.Y) * (height + dh.Y) / height;
                               Editor.currentFigure.Points[k].Y += w2.Y;
                                if (shift) { dh.X = dh.Y; } else { dh.X=dh.X; }
                               Editor.currentFigure.Points[k].X = (v1[i][k].X - w1.X) * (width + dh.X) / width;
                               Editor.currentFigure.Points[k].X += w1.X;
                            }

                            Editor.paths[i].Reset(); Editor.paths[i].AddPolygon(Editor.currentFigure.Points);
                        }
                        Invalidate();

                    }
                }
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
                if (Editor.paths.Length > 0 && Editor.mouseDown == false && f4==false &&f2==false )
                {
                    for (i = Editor.figures.Length - 1; i > -1; i--)
                    {
                        if (Editor.paths[i].IsVisible(e.Location))
                        {
                            Editor.figures[i].Selected = !Editor.figures[i].Selected;
                            kvadrati();
                            Invalidate(); return;
                        }
                    }
                    for (i = 0; i < Editor.figures.Length; i++) Editor.figures[i].Selected = false;
                    numericUpDown8.Value = (decimal)t_;
                    Invalidate();
                }
            f4 = false; Cursor = Cursors.Default;
            f2 = false;

            if (Editor.mouseDown)
                {
                    Editor.state = States.IDLE;
                    Editor.mouseDown  = false; 
                    Cursor = Cursors.Default;
                    Editor.currentFigure.Thickness = (float)numericUpDown8.Value;
                
                    Editor.addFigure(Editor.currentFigure);

                    Invalidate();
                }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
                if (Editor.paths.Length > 0)
                {
                    for (i = 0; i < Editor.paths.Length; i++)
                    {
                        g.FillPath(new SolidBrush(Editor.figures[i].FillColor), Editor.paths[i]);
                        g.DrawPath(new Pen(Editor.figures[i].BorderColor, Editor.figures[i].Thickness), Editor.paths[i]);
                        if (f4) kvadrati();
                    }
                    for (i = 0; i < Editor.figures.Length; i++)
                    {
                        if (Editor.figures[i].Selected)
                        {
                            for (j = 0; j < 8; j++)
                            {
                                g.FillRectangle(new SolidBrush(Color.Blue), c[j].X, c[j].Y, 10, 10);
                            }
                            break;
                        }
                    }
                }
                if (Editor.mouseDown)
                {
                    Editor.currentFigure.FillColor = fillColorButton.BackColor;
                    Editor.currentFigure.BorderColor = borderColorButton.BackColor;

                    g.FillPath(new SolidBrush(fillColorButton.BackColor), Editor.currentPath);
                    g.DrawPath(new Pen(borderColorButton.BackColor, (float)numericUpDown8.Value), Editor.currentPath);
                }
        }
        void kvadrati()
        {
            Editor.currentPath.Reset();
            for (j = 0; j < Editor.figures.Length; j++)
            {
                if (Editor.figures[j].Selected) Editor.currentPath.AddPath(Editor.paths[j], true);
            }
            rect = Editor.currentPath.GetBounds();

            c[0].X = c[3].X = c[5].X = rect.Left - 10;
            c[1].X = c[6].X = rect.Left + rect.Width / 2 -5;
            c[2].X = c[4].X = c[7].X = rect.Right;
            c[0].Y = c[1].Y = c[2].Y = rect.Top - 10;
            c[3].Y = c[4].Y = rect.Top + rect.Height / 2 - 5;
            c[5].Y = c[6].Y = c[7].Y = rect.Bottom;
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void выделитьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (i = 0; i < t.Length; i++) Editor.figures[i].Selected = true;
            Invalidate();
        }
        void delete()
        {
            
                k = q = 0;
                for (i = 0; i < Editor.paths.Length; i++)
                {
                    if (Editor.figures[i].Selected == false)
                    {
                        Editor.paths[k] = Editor.paths[i];

                        Editor.figures[k].Selected = Editor.figures[i].Selected;
                    ;
                    k++;
                    }
                    else q++;
                }
                //Editor.paths.Length -= q;
                // тут надо удалить фигуру


                Editor.currentPath.Reset();
            
            Invalidate();
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            delete();
        }
    
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                shift = true;
            }
            
            if (e.KeyCode == Keys.Delete && Editor.paths.Length > 0) delete();

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                shift = false;
            }
        }
    }
}
