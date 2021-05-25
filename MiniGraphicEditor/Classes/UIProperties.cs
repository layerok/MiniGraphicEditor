using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MiniGraphicEditor.Classes
{
    class UIProperties
    {
        Editor Editor;
        Button[] figureButtons;
        public PointF originPoint, endPoint;
        public float wd, hg;

        int i;

        public UIProperties(Editor Editor)
        {
            this.Editor = Editor;
        }

        public void createFigureButtons()
        {
            figureButtons = new Button[Editor.registeredFigures.Length];

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

                Editor.form.panel1.Controls.Add(figureButtons[i]);
                figureButtons[i].Click += new EventHandler(this.selectFigureButton_Click);
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

    }
}
