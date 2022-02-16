using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ws.Fus.PlanningScans.UI.Wpf.Controls.ScanBox
{
    /// <summary>
    /// Interface representing class responsible for
    /// exchanging data between Application and ScanBox controls
    /// Values in this interface are absolute 2D values.
    /// </summary>
    public interface IScanBoxAware
    {
        /* ScanBox subscribes to these events. Parameters are converted to absolute 2D values 
         * These events fired as reaction on activity in other windows.
         */

        /// <summary>
        /// Fired when underlying 3d geometry changed. For example after rotation.
        /// </summary>
        event EventHandler GeometryChanged;
        /// <summary>
        /// Fired when scan Center changed.
        /// </summary>
        event EventHandler<Point> CenterChanged;
        /// <summary>
        /// Fired when "From" limit changed.
        /// </summary>
        event EventHandler<double> FromChanged;
        /// <summary>
        /// Fired when "To" limit changed.
        /// </summary>
        event EventHandler<double> ToChanged;
        /// <summary>
        /// Fired when Slice Number changed. Triggered by changes of From and To limits.
        /// </summary>
        event EventHandler<int> SliceNumberChanged;
        /// <summary>
        /// Fired when Keep Rotation changed. Triggered on changes active window.
        /// </summary>
        event EventHandler<bool> KeepRotationChanged;
        /// <summary>
        /// Fires when window enters into disposing 
        /// </summary>
        event EventHandler Disposing;

        /*======================================================*/

        /* ScanBox call these function to update about its activity inside specific window.
         * ScanBox call these function with absolute 2d values.  
         */

        /// <summary>
        /// Property holding size of UI element which defines the size of window.
        /// </summary>
        Size ContainerSize { get; set; }

        /// <summary>
        /// Called on moving scan center - while mouse pressed
        /// </summary>
        /// <param name="center">current coordinate</param>
        /// <param name="prevCenter">previous coordinate</param>
        void UpdateCenterMoving(Point center, Point prevCenter);
        /// <summary>
        /// Called when scan center moved - on mouse release
        /// </summary>
        /// <param name="center">current coordinate</param>
        /// <param name="prevCenter">previous coordinate</param>
        void UpdateCenterMoved(Point center, Point prevCenter);

        
        /// <summary>
        /// Called on moving "From" limit - while mouse pressed
        /// </summary>
        /// <param name="from">From length in pixels</param>
        void UpdateFromMoving(double from);
        /// <summary>
        ///  Called when "From" limit changed - on mouse release
        /// </summary>
        /// <param name="from">From length in pixels</param>
        void UpdateFrom(double from);


        /// <summary>
        /// Called on moving "To" limit - while mouse pressed
        /// </summary>
        /// <param name="to">To length in pixels</param>
        void UpdateToMoving(double to);
        /// <summary>
        ///  Called when "To" limit changed - on mouse release
        /// </summary>
        /// <param name="to">To length in pixels</param>
        void UpdateTo(double to);


        /// <summary>
        /// Called when rotation finished - on mouse release
        /// </summary>
        /// <param name="angle"></param>
        void UpdateRotation(double angle);


        /// <summary>
        /// Property notifying whether the ScanBoxControl in secondary window has horizontal orientation
        /// </summary>
        bool IsAxisHorizontal { get; }
        /// <summary>
        /// Property notifying whether the ScanBoxControl is rotatable
        /// </summary>
        bool IsRotatable { get; }
        /// <summary>
        /// Property notifying whether the ScanBoxControl is movable
        /// </summary>
        bool IsMovable { get; }


        /// <summary>
        /// Scan parameters for building ScanBoxControl 
        /// </summary>
        /// <returns></returns>
        ScanBoxParams GetScanBoxParams();
        /*======================================================*/
    }
}
