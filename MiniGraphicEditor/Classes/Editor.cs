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
using System.Reflection;
using System.Text.RegularExpressions;


namespace MiniGraphicEditor.Classes
{
    
    class Editor
    {

        int i, j;

        public int buttonFigureSize = 24;
        public int buttonFigureMarginRight = 5;
        public int buttonFigureColumns = 10;
        public int buttonFigureBlockTop = 6;
        public int buttonFigureBlockLeft = 600;

        public Figure[]         figures = new Figure[0];
        public Figure           currentFigure;
        public Type[] registeredFigures = new Type[0];
        public int selectedFigureIndex = 0;

        public EditorForm form;

        public Resizer Resizer;
        
        public string state = States.IDLE;
        public bool mouseDown = false, shiftPressed = false;


        public UIProperties uiProps = new UIProperties();

        public PointF pressedPoint = new PointF();

        public Editor(EditorForm form)
        {
            Resizer = new Resizer(this);
            this.form = form;
        }

        public void init()
        {
            resetSelectedFigure();
        }

        public void resetSelectedFigure()
        {
            this.currentFigure = (Figure)Activator.CreateInstance(registeredFigures[selectedFigureIndex]);
        }

        public void addCurrentFigure()
        {
            addFigure(this.currentFigure);
        }

        public void addFigure(Figure figure)
        {
            int newLength = figures.Length + 1;

            Array.Resize(ref figures, newLength);
            figures[newLength - 1] = figure;

            this.currentFigure = (Figure)Activator.CreateInstance(registeredFigures[selectedFigureIndex]);
        }

 


        public void moveSelected(float x, float y)
        {
            for (i = figures.Length - 1; i > -1; i--)
            {
                form.Cursor = Cursors.Hand;
                if (!figures[i].Selected) continue;


                for (j = 0; j < figures[i].Points.Length; j++)
                {
                    figures[i].Points[j].X = figures[i].PathCopy.PathPoints[j].X + x;
                    figures[i].Points[j].Y = figures[i].PathCopy.PathPoints[j].Y + y;

                }
                figures[i].Path.Reset(); figures[i].Path.AddPolygon(figures[i].Points);
                Resizer.calculatePoints();
                form.Invalidate();
            }
        }

        public void deleteSelected()
        {
            int unSelectedCount = 0, selectedCount = 0;
            for (i = 0; i < figures.Length; i++)
            {
                if (figures[i].Selected == false)
                {
                    figures[unSelectedCount] = figures[i];

                    figures[unSelectedCount].Selected = figures[i].Selected;

                    unSelectedCount++;
                }
                else selectedCount++;
            }

            int newLength = figures.Length - selectedCount;

            Array.Resize(ref figures, newLength);

            currentFigure.Path.Reset();

            form.Invalidate();
        }

        public void registerFigure(Type type)
        {
            Array.Resize(ref registeredFigures, registeredFigures.Length + 1);
            registeredFigures[registeredFigures.Length - 1] = type;

        }

    }
}
