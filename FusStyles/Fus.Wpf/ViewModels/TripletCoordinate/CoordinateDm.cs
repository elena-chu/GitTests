using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.UI.Wpf.ViewModels.TripletCoordinate
{
    public class CoordinateDm : BindableBase, IDataErrorInfo, ICoordinateValidation
    {
        double _coordinate;
        string _footer;
        TripletDm _parent;
        string _plusCode;
        string _plusEnCoded;
        string _minusCode;
        string _minusEnCoded;
        double _minValue;
        double _maxValue;
        double _increment;
        string _error;
        DisplayStatus _displayStatus;
        int _numberOfFractionDigits = 1;
        public CoordinateDm(TripletDm tripletDm)
        {
            Parent = tripletDm;
            SetDefaults();

        }
        public CoordinateDm()
        {
            SetDefaults();
        }
        public virtual void SetDefaults()
        {
            PlusCode = "+";
            MinusCode = "-";
            PlusEnCoded = "Positive";
            MinusEnCoded = "Negative";
            Increment = 1;
            MaxValue = double.MaxValue;
            MinValue = double.MinValue;
        }
       
        
        
        public TripletDm Parent

        {
            set
            {
                _parent = value;
                RaisePropertyChanged();
            }
            get
            {
                return _parent;
            }
        }


        public double Coordinate
        {
            get
            {
                return _coordinate;
            }
            set
            {
                if (value <= MaxValue && value >= MinValue)
                {
                    _coordinate = Math.Round(value, 2);
                    Error = null;
                   
                }

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CoordinateSignCode));
                RaisePropertyChanged(nameof(CoordinateDisplay));
                RaisePropertyChanged(nameof(Footer));
                if (Parent != null)
                {
                    if (Parent is TripletDm)
                    {
                        TripletDm _par = Parent;
                        //_par.RaisePropertyChanged(nameof(_par.Point)); // to be resolved if we really need it
                    }
                }

            }
        }
        public string CoordinateSignCode
        {
            get
            {
                return Math.Sign(Coordinate) >= 0 ? PlusCode : MinusCode;
            }
        }
        public string CoordinateDisplay
        {
            get
            {
                return GetValueDisplay();
            }
            set
            {
                SetValueDisplay(value);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Coordinate));
                RaisePropertyChanged(nameof(CoordinateSignCode));
                RaisePropertyChanged(nameof(Footer));
            }
        }
       
        public string Footer
        {
            get
            {
                if (_footer != null)
                {
                    return _footer;
                }
                else
                {
                    if (this.CoordinateSignCode == MinusCode)
                    {
                        return MinusEnCoded;
                    }
                    if (this.CoordinateSignCode == PlusCode)
                    {
                        return PlusEnCoded;
                    }
                    return null;
                }
            }
            set
            {
                _footer = value;
                RaisePropertyChanged();
            }
        }
        public string PlusCode
        {
            get
            {
                return _plusCode.ToUpper();
            }
            set
            {
                _plusCode = value;
                RaisePropertyChanged();
            }
        }
        public string PlusEnCoded
        {
            get
            {
                return _plusEnCoded;
            }
            set
            {
                _plusEnCoded = value;
                RaisePropertyChanged();
            }
        }
        public string MinusCode
        {
            get
            {
                return _minusCode.ToUpper();
            }
            set
            {
                _minusCode = value;
                RaisePropertyChanged();
            }
        }
        public string MinusEnCoded
        {
            get
            {
                return _minusEnCoded;
            }
            set
            {
                _minusEnCoded = value;
                RaisePropertyChanged();
            }
        }
        public double MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
                RaisePropertyChanged();
            }
        }
        public double MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
                RaisePropertyChanged();
            }
        }
        public double Increment
        {
            get
            {
                return _increment;
            }
            set
            {
                _increment = value;
                RaisePropertyChanged();
            }
        }
        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
                RaisePropertyChanged();
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
        public int NumberOfFractionDigits
        {
            get
            {
                return _numberOfFractionDigits;
            }
            set
            {
                _numberOfFractionDigits = value;
                RaisePropertyChanged();
            }
        }
        public bool IsValid(string valueDisplay)
        {

            string input = valueDisplay.ToUpper().Trim();
            string prefix = "+";
            string number = input;
            if (input.StartsWith(MinusCode) || input.StartsWith(PlusCode) || input.StartsWith("+") || input.StartsWith("-"))
            {
                prefix = input.Substring(0, 1).ToUpper();
                number = input.Substring(1);
            }
            double parsedNumber;
            if (string.IsNullOrWhiteSpace(number))
            {
                return true;
            }
            if (double.TryParse(number, out parsedNumber))
            {

                if (prefix == "-" || prefix == MinusCode)
                {
                    parsedNumber = -parsedNumber;
                }

                if (Math.Round(parsedNumber, NumberOfFractionDigits) != parsedNumber)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }

        }
        protected string GetValueDisplay()
        {
            string format = GetFormat();
            return $"{CoordinateSignCode} {Math.Abs(Coordinate).ToString(format)}";
        }
        protected void SetValueDisplay(string valueDisplay)
        {
            string input = valueDisplay.ToUpper();
            string prefix = "+";
            string number = input;
            Error = null;
            if (input.StartsWith(MinusCode) || input.StartsWith(PlusCode) || input.StartsWith("+") || input.StartsWith("-"))
            {
                prefix = input.Substring(0, 1).ToUpper();
                number = input.Substring(1);
            }
            double parsedNumber;
            bool okWithTheNumber = false;
            if (string.IsNullOrWhiteSpace(number))
            {
                parsedNumber = 0;
                okWithTheNumber = true;
            }
            else
            {
                okWithTheNumber = double.TryParse(number, out parsedNumber);
            }
            if (okWithTheNumber)
            {

                if (prefix == "-" || prefix == MinusCode)
                {
                    parsedNumber = -parsedNumber;
                }
                if (parsedNumber <= MaxValue && parsedNumber >= MinValue)
                {
                    Coordinate = Math.Round(parsedNumber, NumberOfFractionDigits);

                }
                else
                {
                    if (parsedNumber > MaxValue)
                    {
                        Coordinate = MaxValue;
                    }
                    if (parsedNumber < MinValue)
                    {
                        Coordinate = MinValue;
                    }

                }
            }
            else
            {
                Error = $"Number parsing failed for:  {number} ";
            }


        }
        private string GetFormat()
        {
            string ret = "0";
            if (NumberOfFractionDigits > 0)
            {
                ret += ".";
                for (int i = 0; i < NumberOfFractionDigits; i++)
                {
                    ret += "0";
                }
            }
            return ret;
        }

        public string this[string columnName]// very basic error reporting. All properties can be validated TODO - Not in use - we do not create errors!!!
        {
            get
            {
                switch (columnName)
                {
                    case "CoordinateDisplay":
                        if (_error != null)
                            return _error;
                        break;
                }

                return _error;
            }
        }
    }
}
