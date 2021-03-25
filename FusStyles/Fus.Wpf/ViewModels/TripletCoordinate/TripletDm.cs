using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Ws.Fus.UI.Wpf.ViewModels.TripletCoordinate
{
    public class TripletDm: BindableBase// TODO consider adding the CoordinatesDirection in case it affects the nature of the coordinates
    {
        Point3D _point = new Point3D();
        public TripletDm()
        {
            X = new CoordinateSagitalDm  (this);
            Y = new CoordinateCoronalDm(this);
            Z = new CoordinateAxialDm(this);
        }
        public TripletDm(Point3D point)
        {
            _point = point;
            
            X = new CoordinateSagitalDm(this);
            Y = new CoordinateCoronalDm(this);
            Z = new CoordinateAxialDm(this);
        }
        CoordinateDm _x1;
        CoordinateDm _y2;
        CoordinateDm _z3;
      
        SeverityLevel _severityLevel;
        DisplayStatus _displayStatus= DisplayStatus.Active;
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
            {  _point = value;
                X.Coordinate = _point.X;
                Y.Coordinate = _point.Y;
                Z.Coordinate = _point.Z;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Y));
                RaisePropertyChanged(nameof(Z));
                RaisePropertyChanged(nameof(Z));
            }
        }
        public DisplayStatus DisplayStatus
        {
            get { return _displayStatus; }
            set { _displayStatus = value;
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
    }
}
