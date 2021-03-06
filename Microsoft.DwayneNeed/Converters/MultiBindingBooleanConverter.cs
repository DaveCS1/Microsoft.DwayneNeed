﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.DwayneNeed.Converters
{
    public class MultiBindingBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string binaryOp = parameter is string ? (string) parameter : "";
            if (string.Compare(binaryOp, "and", true) != 0 &&
                string.Compare(binaryOp, "or", true) != 0)
                throw new ArgumentException("MultiBindingBooleanConverter parameter must be either \"and\" or \"or\".");
            bool isAnd = string.Compare(binaryOp, "and", true) == 0;
            bool? result = null;

            foreach (object value in values)
                if (result.HasValue)
                {
                    // Combine subsequent items.
                    if (isAnd)
                        result &= ConvertToBool(value);
                    else
                        result |= ConvertToBool(value);
                }
                else
                {
                    // First time.
                    result = ConvertToBool(value);
                }

            return result.GetValueOrDefault();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("MultiBindingBooleanConverter cannot convert back.");
        }

        private bool ConvertToBool(object obj)
        {
            if (obj is bool)
                return (bool) obj;
            if (obj is bool?)
                return ((bool?) obj).GetValueOrDefault();
            return false;
        }
    }
}