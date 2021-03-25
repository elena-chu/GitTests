using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.UI.Wpf.ViewModels.TripletCoordinate
{
    public class CoordinateCoronalDm : CoordinateDm
    {
        public CoordinateCoronalDm(TripletDm tripletDm):base(tripletDm)
        {

        }
        public CoordinateCoronalDm() : base()
        {

        }
        public override void SetDefaults()
        {
            base.SetDefaults();

            PlusCode = "A";
            MinusCode = "P";
            PlusEnCoded = "Anterior";
            MinusEnCoded = "Posterior";
            Increment = 0.1;
            MaxValue = 20;
            MinValue = -20;
        }
    }
}
