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
        public Rotator Rotator;
        public Selector Selector;
        public Mover Mover;
        public Drawer Drawer;
        
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
            Rotator = new Rotator(this);
            Selector = new Selector(this);
            Mover = new Mover(this);
            Drawer = new Drawer(this);
        }

        public void init()
        {
            resetSelectedFigure();
            uiProps.createFigureButtons();
            Rotator.createHandleButton();
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
