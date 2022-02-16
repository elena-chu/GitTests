using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.Interfaces.Overlays
{
	public enum UiMode
	{
        None = 82,
        TextOverlay = 2,
        SpotsOverlay = 7,
        BathLimitsOverlay = 12,
        RegionsOverlay = 14,
        IntersectingSlicesOverlay = 19,
        FiducialsOverlay = 21,
        AdjustmentPoint = 22,
        CalibrationDataOverlay = 25,
        BackgroundElimination = 38,
        RegManEdit = 40,
        NPRPolygonsOverlay = 41,
        ScanGridOverlay = 44,
        PreOpSegmentationOverlay = 47,
        RigidNPROverlay = 57,
        MeshNPROverlay = 58,
        TempMonitoringVolume = 62,
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
        MeasurementAngle90 = 75,
        MeasurementAngleACPC90 = 76,
        MeshNPRAirOverlay = 77,
        MeshNPRMembraneFolds = 78,
        Tractography = 79,
        LastDoseMesh = 80,
        AccumDoseMesh = 81,

        // modes
        ManualAdjust = 83,
        AddPositionData = 84,
        SetTilt = 85,
        Zoom = 86,
        Pan = 87,
        Windowing = 88,
        SynchronizeStripDisplay = 89,
        SynchronizePositioning = 90,
        Clone = 91,
        Flicker = 92,
        Animate = 93,
        LayerLast = 94,  // Must be last to enable looping on layers.
    }
}
