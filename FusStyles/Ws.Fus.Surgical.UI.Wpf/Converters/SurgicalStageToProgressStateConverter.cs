using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Fus.Surgical.UI.Wpf
{
    public class SurgicalStageToProgressStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return ProgressState.Regular;

            SurgicalMode current, param;
            if( Enum.TryParse<SurgicalMode>(value.ToString(), out current)  && Enum.TryParse<SurgicalMode>(parameter.ToString(), out param))
            {
                if (current == param)
                    return ProgressState.Active;
                else if (param < current)
                    return ProgressState.Completed;
                else
                    return ProgressState.Regular;
            }
            return ProgressState.Regular;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
