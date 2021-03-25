using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.UI.Wpf.ViewModels.TripletCoordinate
{
    public class CoordinateAxialDm : CoordinateDm
    {
        public CoordinateAxialDm(TripletDm tripletDm):base(tripletDm)
        {

        }
        public CoordinateAxialDm() : base()
        {

        }
        public override void SetDefaults()
        {
            base.SetDefaults();

            PlusCode = "S";
            MinusCode = "I";
            PlusEnCoded = "Superior";
            MinusEnCoded = "Inferior";
            Increment = 0.1;
            MaxValue = 40;
            MinValue = -40;
        }
    }
}
