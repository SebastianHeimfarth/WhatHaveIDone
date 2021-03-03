using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WhatHaveIDone.Converter
{
    public class NullToVisibilityConveter : IValueConverter
    {
        public bool IsInverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = value == null;
            if(IsInverted)
            {
                isVisible = !isVisible;
            }

            return isVisible? Visibility.Visible: Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}