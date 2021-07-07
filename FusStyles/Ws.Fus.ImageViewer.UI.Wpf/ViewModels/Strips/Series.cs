using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips
{
    public class Series 
    {
        public Series()
        {
        }

        //public Series(Study study)
        //{
        //    Study = study;
        //}

        public Series(string patientName,
                      string patientId,
                      double? sliceThickness = null,
                      double? sliceSpacing = null,
                      string resolution = null,
                      int? seriesNumber = null,
                      string seriesDescription = null)
        {
            PatientName = patientName;
            PatientId = patientId;
            SliceThickness = sliceThickness;
            SliceSpacing = sliceSpacing;
            Resolution = resolution;
            SeriesNumber = seriesNumber;
            SeriesDescription = seriesDescription;
        }

        public virtual string PatientName { get; set; }

        public virtual string PatientId { get; set; }

        public virtual double? SliceThickness { get; set; }

        public virtual double? SliceSpacing { get; set; }

        public virtual string Resolution { get; set; }

        public virtual string SeriesInstanceUid { get; set; }

        public virtual string StudyInstanceUid { get; set; }

        public virtual string Modality { get; set; }

        public virtual int? SeriesNumber { get; set; }

        public virtual DateTime? SeriesDate { get; set; }

        public virtual string SeriesDescription { get; set; }

        public virtual string SeriesCode { get; set; }

        public virtual string SeriesType { get; set; }

        public virtual int NumberOfSeriesRelatedInstances { get; set; }

        //public virtual Study Study { get; set; }

        //public virtual SeriesOrientation Orientation { get; set; } = SeriesOrientation.None;

        public virtual string ImagesUri { get; set; }

        public bool Equals(Series other)
        {
            if (other == null)
                return false;

            return SeriesInstanceUid == other.SeriesInstanceUid;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Series)obj);
        }

        // override object.GetHashCode
        public override int GetHashCode() => SeriesInstanceUid?.GetHashCode() ?? string.Empty.GetHashCode();
    }
}
