using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MiniEditor.Classes
{
    
    class Editor
    {

   
        public GraphicsPath[]   paths = new GraphicsPath[0];
        public Figure[]         figures = new Figure[0];
        public GraphicsPath     currentPath = new GraphicsPath();
        public Figure           currentFigure = new Lightning();
        public int              totalFiguresCount = 0;
        public string state = States.IDLE;
        public bool mouseDown = false;

        public UIProperties props = new UIProperties();



       


        
        public void addCurrentFigure()
        {
            addFigure(this.currentFigure);
        }

        public void addFigure(Figure figure)
        {
            this.totalFiguresCount++;

            this.currentPath.Reset();
            this.currentPath.AddPolygon(figure.Points);

            Array.Resize(ref figures, totalFiguresCount);
            figures[totalFiguresCount - 1] = figure;

            Array.Resize(ref paths, totalFiguresCount);
            paths[totalFiguresCount - 1] = new GraphicsPath();
            paths[totalFiguresCount - 1].AddPath(currentPath, true);

            this.currentFigure = new Lightning();


        }

        public void addTempPath()
        {
            this.currentPath.Reset();
            this.currentPath.AddPolygon(this.currentFigure.Points);
        }
        


    }
}
