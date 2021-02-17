using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ws.Extensions.UI.Wpf.Converters
{
    public static class ConverterAssists
    {
        public const char INVERT_MARKER = '!';
        public const char HIDDEN_MARKER = '#';
        public const char IGNORE_CASE_MARKER = '~';

        /// <summary>
        /// Check that converter input value is valid
        /// </summary>
        /// <param name="value">Value passed into converter</param>
        /// <returns>True if value is valid</returns>
        public static bool CheckValueValidity(object value)
        {
            return (value != null && value != DependencyProperty.UnsetValue);
        }

        /// <summary>
        /// Check that converter input array is valid and so are all its members
        /// </summary>
        /// <param name="values">Array passed into converter</param>
        /// <returns>True if all array values are valid</returns>
        public static bool CheckAllValuesValidity(object[] values)
        {
            return (values != null && values.Count() > 0 && !(values.Any(v => !CheckValueValidity(v))));
        }

        /// <summary>
        /// Parse param for negative visibility
        /// </summary>
        /// <param name="parameter">Parameter passed into converter</param>
        /// <returns>Visibility.Hidden if parameter contains instructions for it, Visibility.Collapsed otherwise</returns>
        public static Visibility GetNegativeVisibility(object parameter)
        {
            Visibility negativeVisibility = Visibility.Collapsed;

            // # or Hidden in parameter ==> Visibility.Hidden
            if (CheckValueValidity(parameter))
            {
                string paramStr = parameter.ToString();
                if (CheckForCharAndRemove(HIDDEN_MARKER, ref paramStr))
                    negativeVisibility = Visibility.Hidden;
                else if (Enum.TryParse(paramStr, true, out Visibility parsedVisibility))
                    negativeVisibility = parsedVisibility;
            }
            return negativeVisibility;
        }

        /// <summary>
        /// Parse param for invert char and negative visibility (Hidden, Collapsed)
        /// </summary>
        /// <param name="parameter">Parameter passed into converter</param>
        /// <param name="invert">Result for Invert</param>
        /// <param name="negativeVisibility">Result for Negative Visibility</param>
        /// <returns>True if parameter was parsed successfully</returns>
        public static bool GetInvertAndNegativeVisibility(object parameter, out bool invert, out Visibility negativeVisibility)
        {
            negativeVisibility = Visibility.Collapsed;
            invert = false;

            // Parameter can be used to convert to Hidden instead of Collapsed
            if (CheckValueValidity(parameter) && (parameter is string))
            {
                string paramStr = parameter.ToString();
                invert = CheckForCharAndRemove(INVERT_MARKER, ref paramStr);
                negativeVisibility = GetNegativeVisibility(paramStr);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Parse param for invert char
        /// </summary>
        /// <param name="parameter">Parameter passed into converter</param>
        /// <param name="invert">Result for Invert</param>
        /// <returns>True if parameter was parsed successfully</returns>
        public static bool GetInvert(object parameter, out bool invert)
        {
            invert = false;
            if (CheckValueValidity(parameter) && (parameter is string))
            {
                string paramStr = parameter.ToString();
                invert = CheckForCharAndRemove(INVERT_MARKER, ref paramStr);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Parse param for ignore case
        /// </summary>
        /// <param name="parameter">Parameter passed into converter</param>
        /// <param name="stringComparison">StringComparison type</param>
        /// <returns>True if parameter was parsed successfully</returns>
        public static bool GetStringComparison(object parameter, out StringComparison stringComparison)
        {
            stringComparison = StringComparison.Ordinal;
            if (CheckValueValidity(parameter) && (parameter is string))
            {
                string paramStr = parameter.ToString();
                if (CheckForCharAndRemove(IGNORE_CASE_MARKER, ref paramStr))
                    stringComparison = StringComparison.OrdinalIgnoreCase;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Parse string for one or more values of enum type
        /// </summary>
        /// <param name="enumType">Enum type to parse for</param>
        /// <param name="str">String to parse</param>
        /// <returns>IEnumerable with all enum values parsed</returns>
        public static IEnumerable<Enum> ParseStringToEnum(Type enumType, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Empty or null string");
            }

            string[] strArray = str.Split(new[] { '|', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var enumArray = new Enum[strArray.Length];
            for (int i = 0; i < strArray.Length; ++i)
            {
                enumArray[i] = (Enum)Enum.Parse(enumType, strArray[i], true);
            }

            return enumArray;
        }

        public static double[] ParseStringToDouble(string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("Empty or null string");

            return str.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => double.Parse(s)).ToArray();
        }

        public static bool CheckForCharAndRemove(char c, ref string str)
        {
            bool containsChar = str.Contains(c);
            if (containsChar)
            {
                str = str.Remove(str.IndexOf(c), 1);
            }
            return containsChar;
        }
    }
}
