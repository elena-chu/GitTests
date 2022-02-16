using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestDrawing
{
    public class MainOrientationPanel: BaseActionPanel
    {
        public MainOrientationPanel()
        {
            IsMovable = true; 
        }

        public override void BuildSpecific(BuildParameters initialParams)
        {
            if(!_isInitialized)
            {
                _mainSquare = FindMain("MainSquare");
                _secondSquare = FindSecondary("SecSquare");

                if (IsMovable)
                {
                    _secondSquare.MouseLeftButtonDown += OnSecondSquareMouseLeftButtonDown;
                    _secondSquare.MouseLeftButtonUp += OnSecondSquareMouseLeftButtonUp;
                }
            }


            _mainSquare.Width = InitialParams.FOV * ScaleFactor;
            _mainSquare.Height = InitialParams.FOV * ScaleFactor;
            Canvas.SetLeft(_mainSquare, ( this.Width/2 - _mainSquare.Width /2));
            Canvas.SetTop(_mainSquare, (this.Height / 2 - _mainSquare.Height / 2));

            SetVectorPoint();
        }
    }
}
