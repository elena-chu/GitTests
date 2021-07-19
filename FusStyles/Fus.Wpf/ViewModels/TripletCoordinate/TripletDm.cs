using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Ws.Fus.UI.Wpf.ViewModels.TripletCoordinate
{
    public class TripletDm : BindableBase
    {
        Point3D  _point = new Point3D();
      
        bool _hasValue = false;
       
        public TripletDm()
        {
            SetDimentions();
        }
        public TripletDm(Point3D point)
        {
            _point = point;
            SetDimentions();
        }
        private void SetDimentions()
        {
            X = new CoordinateDm(this) // Sagital
            {
                PlusCode = "R",
                MinusCode = "L",
                PlusEnCoded = "Right",
                MinusEnCoded = "Left",
                Increment = 0.1,
                MaxValue = 999.9,
                MinValue = -999.9
            };
            X.HasValue = false;
            Y = new CoordinateDm(this) // Coronal
            {
                PlusCode = "A",
                MinusCode = "P",
                PlusEnCoded = "Anterior",
                MinusEnCoded = "Posterior",
                Increment = 0.1,
                MaxValue = 999.9,
                MinValue = -999.9
            };
            Y.HasValue = false;
            Z = new CoordinateDm(this) // Axial
            {
                PlusCode = "S",
                MinusCode = "I",
                PlusEnCoded = "Superior",
                MinusEnCoded = "Inferior",
                Increment = 0.1,
                MaxValue = 999.9,
                MinValue = -999.9
            };
            Z.HasValue = false;

        }
        CoordinateDm _x1;
        CoordinateDm _y2;
        CoordinateDm _z3;

        SeverityLevel _severityLevel;
        DisplayStatus _displayStatus = DisplayStatus.Active;
       
        public bool HasValue
        {
            get
            {
                if (!_hasValue)
                {
                    if (X.HasValue && Y.HasValue && Z.HasValue)
                    {
                        _hasValue = true;
                    }
                }
                
                return _hasValue;
            }
            set
            {
                _hasValue = value;
                
                if (!_hasValue)
                {
                    Reset();
                }
                RaisePropertyChanged();
                X.Raise();
                Y.Raise();
                Z.Raise();


            }
        }
        
        public Point3D Point
        {
            get
            {
                _point.X = X.Coordinate;
                _point.Y = Y.Coordinate;
                _point.Z = Z.Coordinate;
                return _point;
            }
            set
            {  
                X.Coordinate = value.X;
                Y.Coordinate = value.Y;
                Z.Coordinate = value.Z;
                _point = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(X));
                RaisePropertyChanged(nameof(Y));
                RaisePropertyChanged(nameof(Z));
            }
        }
        public DisplayStatus DisplayStatus
        {
            get { return _displayStatus; }
            set
            {
                _displayStatus = value;
                RaisePropertyChanged();
            }
        }

        public SeverityLevel SeverityLevel
        {
            get
            {
                return _severityLevel;
            }
            set
            {
                _severityLevel = value;
                RaisePropertyChanged();
            }
        }

        public CoordinateDm X
        {
            get
            {
                return _x1;
            }
            set
            {
                _x1 = value;
                _x1.Parent = this;
                _x1.Coordinate = _point.X;
                RaisePropertyChanged();
            }
        }

        public CoordinateDm Y
        {
            get
            {
                return _y2;
            }
            set
            {
                _y2 = value;
                _y2.Parent = this;
                _y2.Coordinate = _point.Y;
                RaisePropertyChanged();
            }
        }

        public CoordinateDm Z
        {
            get
            {
                return _z3;
            }
            set
            {
                _z3 = value;
                _z3.Parent = this;
                _z3.Coordinate = _point.Z;
                RaisePropertyChanged();
            }
        }
        public void Notify()
        {
            this.RaisePropertyChanged(nameof(HasValue));
            this.RaisePropertyChanged(nameof(Point));
        }
        public void Reset()
        {
            _hasValue = false;
            X.Reset();
            Y.Reset();
            Z.Reset();
            Notify();
        }
    }
}
