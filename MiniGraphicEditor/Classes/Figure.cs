using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace MiniGraphicEditor.Classes
{

    abstract class Figure
    {
        protected PointF[] _points;
        protected Color _borderColor = Color.FromArgb(0, 0, 0);
        protected Color _fillColor = Color.FromArgb(255, 255, 255);
        protected bool _selected;
        protected decimal _thickness;
        protected GraphicsPath _path;
        protected GraphicsPath _pathCopy;

        public Figure()
        {
            _path = new GraphicsPath();
        }

        public PointF[] Points { 
            get
            {
                 return _points;
            } 
        }
        public Color BorderColor { 
            get
            {
               return _borderColor;
            }
            set
            {
                _borderColor = value;
            }
        }

        public Color FillColor { 
            get
            {
                return _fillColor;
            }
            set
            {
                _fillColor = value;
            }
        }
        public bool Selected { 
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }

        public decimal Thickness { 
            get
            {
                return _thickness;
            }
            set
            {
                _thickness = value;
            }
        }

        public GraphicsPath Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }


        public GraphicsPath PathCopy
        {
            get
            {
                return _pathCopy;
            }
            set
            {
                _pathCopy = value;
            }
        }


        public abstract void calculatePoints(PointF originPoint, PointF endPoint);

        public abstract void createPath(PointF originPoint, PointF endPoint);
    }
    
}
