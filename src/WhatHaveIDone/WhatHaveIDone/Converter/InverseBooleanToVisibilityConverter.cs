using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WhatHaveIDone.Converter
{
    internal class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                if (booleanValue)
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }

            throw new ArgumentException($"value must be boolean", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}