using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.UI.Wpf.ViewModels.TripletCoordinate
{
    public class CoordinateSagitalDm:CoordinateDm
    {
        public CoordinateSagitalDm(TripletDm tripletDm):base(tripletDm)
        {
            
        }
        public CoordinateSagitalDm() : base()
        {

        }
        public override void SetDefaults()
        {
            base.SetDefaults();

            PlusCode = "L";
            MinusCode = "R";
            PlusEnCoded = "Left";
            MinusEnCoded = "Right";
            Increment = 0.1;
            MaxValue = 40;
            MinValue = -40;
        }
    }
}
