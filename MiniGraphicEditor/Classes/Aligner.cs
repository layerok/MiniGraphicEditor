using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniGraphicEditor.Classes
{
    class Aligner
    {

        int i, k;
        Editor Editor;
        Button[] alignmentButtons = new Button[4];
        String[] alignmentButtonsText = new string[] { "←", "↑", "→", "↓" };

        public Aligner(Editor editor)
        {
            this.Editor = editor;
            createButtons();
        }

        public void createButtons()
        {
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

                Editor.form.Controls.Add(alignmentButtons[i]);
                alignmentButtons[i].Click += new EventHandler(this.alignFigureButton_Click);
            }
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
                                if (Editor.figures[k].OriginPoint.X > Editor.figures[k].EndPoint.X)
                                {
                                    p1.X = Editor.Resizer.selectionRect.Left - Editor.figures[k].Width;
                                    p2.X = Editor.Resizer.selectionRect.Left;
                                }
                                else
                                {
                                    p1.X = Editor.Resizer.selectionRect.Left;
                                    p2.X = Editor.Resizer.selectionRect.Left + Editor.figures[k].Width;
                                }


                                p1.Y = Editor.figures[k].OriginPoint.Y;
                                p2.Y = Editor.figures[k].OriginPoint.Y + Editor.figures[k].Height;

                            }
                            else if (i == 1)
                            {
                                if (Editor.figures[k].OriginPoint.Y > Editor.figures[k].EndPoint.Y)
                                {
                                    p1.Y = Editor.Resizer.selectionRect.Top - Editor.figures[k].Height;
                                    p2.Y = Editor.Resizer.selectionRect.Top;
                                }
                                else
                                {
                                    p1.Y = Editor.Resizer.selectionRect.Top;
                                    p2.Y = Editor.Resizer.selectionRect.Top + Editor.figures[k].Height;
                                }



                                p1.X = Editor.figures[k].OriginPoint.X;
                                p2.X = Editor.figures[k].OriginPoint.X + Editor.figures[k].Width;
                            }
                            else if (i == 2)
                            {
                                if (Editor.figures[k].OriginPoint.X > Editor.figures[k].EndPoint.X)
                                {
                                    p1.X = Editor.Resizer.selectionRect.Right;
                                    p2.X = Editor.Resizer.selectionRect.Right + Editor.figures[k].Width;
                                }
                                else
                                {
                                    p1.X = Editor.Resizer.selectionRect.Right - Editor.figures[k].Width;
                                    p2.X = Editor.Resizer.selectionRect.Right;
                                }


                                p1.Y = Editor.figures[k].OriginPoint.Y;
                                p2.Y = Editor.figures[k].OriginPoint.Y + Editor.figures[k].Height;
                            }
                            else if (i == 3)
                            {

                                if (Editor.figures[k].OriginPoint.X > Editor.figures[k].EndPoint.X)
                                {
                                    p1.Y = Editor.Resizer.selectionRect.Bottom;
                                    p2.Y = Editor.Resizer.selectionRect.Bottom + Editor.figures[k].Height;
                                }
                                else
                                {
                                    p1.Y = Editor.Resizer.selectionRect.Bottom - Editor.figures[k].Height;
                                    p2.Y = Editor.Resizer.selectionRect.Bottom;
                                }



                                p1.X = Editor.figures[k].OriginPoint.X;
                                p2.X = Editor.figures[k].OriginPoint.X + Editor.figures[k].Width;
                            }




                            Editor.figures[k].initCalculations(p1, p2);

                        }
                    }
                }

                Editor.Resizer.calculatePoints();

                Editor.form.Invalidate();

            }
        }

        public void showButtons()
        {
            if (Editor.selectedAmount > 1)
            {
                for (i = 0; i < alignmentButtons.Length; i++)
                {
                    //
                    alignmentButtons[i].Visible = true;
                    alignmentButtons[i].Location = new Point((int)Editor.Resizer.selectionRect.X - Editor.Resizer.pointSize + (i * 35), (int)Editor.Resizer.selectionRect.Y - (Editor.Resizer.pointSize * 4));
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
        }

    }




}
