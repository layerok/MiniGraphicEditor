using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGraphicEditor.Classes
{
    class Selector
    {
        Editor Editor;
        int i;

        public Selector(Editor Editor)
        {
            this.Editor = Editor;
        }

        public void updateMeta()
        {
            Editor.selectedAmount = 0;
            Editor.uiProps.rotateSpinnerValue = 0;
            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Selected)
                {
                    Editor.selectedAmount++;
                    Editor.selectedIndex = i;
                    Editor.uiProps.rotateSpinnerValue = Editor.figures[i].Angle;
                }
            }
        }

        public bool checkIfSelectedFigureClicked(PointF pressedPoint)
        {
            for (i = 0; i < Editor.figures.Length; i++)
            {
                if (Editor.figures[i].Selected && Editor.figures[i].Path.IsVisible(pressedPoint))
                {
                    return true;
                }
            }

            return false;
        }

        public bool toggle(PointF mouseUpPoint)
        {
            for (i = Editor.figures.Length - 1; i > -1; i--)
            {
                if (Editor.figures[i].Path.IsVisible(mouseUpPoint))
                {
                    Editor.figures[i].Selected = !Editor.figures[i].Selected;
                    return true;
                }
            }
            return false;
        }

        public void resetAll()
        {
            for (i = 0; i < Editor.figures.Length; i++) Editor.figures[i].Selected = false;
        }

    }
}
