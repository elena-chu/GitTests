using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.Interfaces.Overlays
{
	public enum UiMode
	{
        None = 78,
        TextOverlay = 2,
        SpotsOverlay = 7,
        BathLimitsOverlay = 12,
        RegionsOverlay = 14,
        IntersectingSlicesOverlay = 19,
        FiducialsOverlay = 21,
        CalibrationDataOverlay = 25,
        RegManEdit = 40,
        NPRPolygonsOverlay = 41,
        ScanGridOverlay = 44,
        PreOpSegmentationOverlay = 47,
        RigidNPROverlay = 57,
        MeshNPROverlay = 58,
        ACPCOverlay = 64,
        ACOverlay = 65,
        PCOverlay = 66,
        MidLineOverlay = 67,
        TargetOverlay = 68,
        ACPCBorderPlan = 70,
        MeasurementOverlay = 71,
        MeasurementDistanceOverlay = 72,
        MeasurementAreaOverlay = 73,
        MeasurementAngleOverlay = 74,
        MeshNPRAirOverlay = 75,
        MeshNPRMembraneFolds = 76,
        Tractography = 77,

        Zoom = 82,
        Pan = 83,
        Windowing = 84,
    }
}
