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
        public int selectedIndex = 1;

        public Figure[]         figures = new Figure[0];
        public Figure           currentFigure;
        public Type[] registeredFigures = new Type[0];
        public int selectedFigureIndex = 0;

        public EditorForm form;

        public Resizer Resizer;
        public Aligner Aligner;
        
        public string state = States.IDLE;
        public bool mouseDown = false, shiftPressed = false, ctrlPressed = false;


        public UIProperties uiProps;

        public PointF pressedPoint = new PointF();

        public Editor(EditorForm form)
        {
            Resizer = new Resizer(this);
            this.form = form;
            Aligner = new Aligner(this);
            uiProps = new UIProperties(this);
        }

        public void init()
        {
            resetSelectedFigure();
            uiProps.createFigureButtons();
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


                
                if(figures[i].EndPoint.X > figures[i].OriginPoint.X)
                {
                    originPoint.X = figures[i].PathCopy.GetBounds().Left + x;
                    endPoint.X = figures[i].PathCopy.GetBounds().Right + x;
                } else
                {
                    originPoint.X = figures[i].PathCopy.GetBounds().Right + x;
                    endPoint.X = figures[i].PathCopy.GetBounds().Left + x;
                }

                if (figures[i].EndPoint.Y > figures[i].OriginPoint.Y)
                {
                    originPoint.Y = figures[i].PathCopy.GetBounds().Top + y;
                    endPoint.Y = figures[i].PathCopy.GetBounds().Bottom + y;
                }
                else
                {
                    originPoint.Y = figures[i].PathCopy.GetBounds().Bottom + y;
                    endPoint.Y = figures[i].PathCopy.GetBounds().Top + y;
                }

                

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

        public PointF getHandleCenterPoint(PointF centerPoint, float radius, float angle)
        {
            PointF hangleCenterPoint = new PointF();
            hangleCenterPoint.X = (float)(centerPoint.X + radius * Math.Cos(angle * Math.PI / 180));
            hangleCenterPoint.Y = (float)(centerPoint.Y + radius * Math.Sin(angle * Math.PI / 180));

            return hangleCenterPoint;
        }

        public float getNewAngle(PointF b2, PointF center)
        {
            float angle;

            const double pi2 = 180 / Math.PI;

            angle = -(float)(Math.Atan2(center.Y - center.Y, center.X + 10 - center.X) * pi2 - Math.Atan2(b2.Y - center.Y, b2.X - center.X) * pi2);

            return angle;
        }

        public void cloneFigures()
        {
            for (i = 0; i < figures.Length; i++)
            {
                figures[i].PathCopy = figures[i].NotTransformedPath;
                figures[i].CloneInstance = (Figure)figures[i].Clone();
            }
        }

    }
}
