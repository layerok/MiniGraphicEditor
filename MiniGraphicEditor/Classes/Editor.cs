using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;



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

        public int selectedAmount = 0;

        public Figure[]         figures = new Figure[0];
        public Figure           currentFigure;
        public Type[] registeredFigures = new Type[0];
        public int selectedFigureIndex = 0;

        public EditorForm form;

        public Resizer Resizer;
        
        public string state = States.IDLE;
        public bool mouseDown = false, shiftPressed = false, ctrlPressed = false;


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


                PointF originPoint = new PointF(), endPoint = new PointF();


                

                originPoint.X = figures[i].PathCopy.GetBounds().Left + x;
                originPoint.Y = figures[i].PathCopy.GetBounds().Top + y;
                endPoint.X = figures[i].PathCopy.GetBounds().Right + x;
                endPoint.Y = figures[i].PathCopy.GetBounds().Bottom + y;

                figures[i].initCalculations(originPoint, endPoint);
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
