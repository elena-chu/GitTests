using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ws.Extensions.UI.Wpf.Converters
{
    /// <summary>
    /// Returns true if Enum matches any in the list inside parameter
    /// Use ConverterAssist's INVERT_MARKER to invert results
    /// </summary>
    public class EnumToBooleanConverter : ConverterMarkupExtension<EnumToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value) || !ConverterAssists.CheckValueValidity(parameter))
                return Binding.DoNothing;


            string paramString = parameter.ToString();
            bool invert = ConverterAssists.CheckForCharAndRemove(ConverterAssists.INVERT_MARKER, ref paramString);

            var enums = ConverterAssists.ParseStringToEnum(value.GetType(), paramString);
            bool enumInList = false;
            foreach (Enum paramValue in enums)
            {
                if (value.Equals(paramValue))
                {
                    enumInList = true;
                    break;
                }
            }

            return !invert ? enumInList : !enumInList;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Returns Visible if Enum matches any in the list inside parameter
    /// Use ConverterAssist's INVERT_MARKER to invert results
    /// Use ConverterAssist's HIDDEN_MARKER to return Hidden instead of Collapsed
    /// Can invert and use Hidden simultaneously
    /// </summary>
    public class EnumToVisibilityConverter : ConverterMarkupExtension<EnumToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value) || !ConverterAssists.CheckValueValidity(parameter))
                return Binding.DoNothing;

            string paramsParsed = parameter.ToString();
            bool invert = ConverterAssists.CheckForCharAndRemove('!', ref paramsParsed);

            Visibility negativeVisibility = Visibility.Collapsed;
            if (ConverterAssists.CheckForCharAndRemove(ConverterAssists.HIDDEN_MARKER, ref paramsParsed))
                negativeVisibility = Visibility.Hidden;

            bool enumInList = false;
            var enums = ConverterAssists.ParseStringToEnum(value.GetType(), paramsParsed);
            foreach (Enum paramValue in enums)
            {
                if (value.Equals(paramValue))
                {
                    enumInList = true;
                    break;
                }
            }

            return !invert ? enumInList ? Visibility.Visible : negativeVisibility :
                             enumInList ? negativeVisibility : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
