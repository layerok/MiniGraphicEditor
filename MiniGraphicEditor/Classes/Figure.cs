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
        protected PointF _centerPoint;
        protected PointF _originPoint;
        protected PointF _endPoint;
        protected float _width;
        protected float _height;
        protected Color _borderColor = Color.FromArgb(0, 0, 0);
        protected Color _fillColor = Color.FromArgb(255, 255, 255);
        protected bool _selected;
        protected int _angle = 0;
        protected int _previousAngle = 0;
        protected decimal _thickness;
        protected GraphicsPath _path;
        protected GraphicsPath _notTransformedPath;
        protected GraphicsPath _pathCopy;

        public Figure()
        {
            _path = new GraphicsPath();
            _notTransformedPath = new GraphicsPath();
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

        public GraphicsPath NotTransformedPath
        {
            get
            {
                return _notTransformedPath;
            }
            set
            {
                _notTransformedPath = value;
            }
        }

        public int Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
            }
        }

        public int PreviousAngle
        {
            get
            {
                return _previousAngle;
            }
            set
            {
                _previousAngle = value;
            }
        }

        public PointF OriginPoint
        {
            get
            {
                return _originPoint;
            }
            set
            {
                _originPoint = value;
            }
        }
        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        public PointF EndPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                _endPoint = value;
            }
        }

        public PointF CenterPoint
        {
            get
            {
                return _centerPoint;
            }
            set
            {
                _centerPoint = value;
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

        public abstract GraphicsPath createPath();

        public void initCalculations(PointF originPoint, PointF endPoint)
        {

            _width = endPoint.X - originPoint.X;
            _height = endPoint.Y - originPoint.Y;

            _centerPoint.X = originPoint.X + (_width / 2);
            _centerPoint.Y = originPoint.Y + (_height / 2);
            _originPoint = originPoint;
            _endPoint = endPoint;


            calculatePoints(originPoint, endPoint);

            _path = createPath();
            _notTransformedPath = createPath();
            

            rotatePath();
        }

        public void rotatePath()
        {
            Matrix rotateMatrix = new Matrix();

            rotateMatrix.RotateAt(Angle, CenterPoint);

            Path.Transform(rotateMatrix);
        }

    }
    
}
