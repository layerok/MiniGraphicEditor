using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace MiniEditor.Classes
{

    abstract class Figure
    {
        protected PointF[] _points;
        protected Color _borderColor = Color.FromArgb(0, 0, 0);
        protected Color _fillColor = Color.FromArgb(255, 255, 255);
        protected bool _selected;
        protected decimal _thickness;
        protected string _base64Image;

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


        public abstract void calculatePoints(PointF originPoint, PointF endPoint);
    }
    
}
