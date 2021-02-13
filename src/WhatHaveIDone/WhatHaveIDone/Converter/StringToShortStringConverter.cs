using System;
using System.Globalization;
using System.Windows.Data;

namespace WhatHaveIDone.Converter
{
    public class StringToShortStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (parameter is string lengthString)
                {
                    var length = System.Convert.ToInt32(lengthString);
                    if (stringValue.Length >= length)
                    {
                        return $"{stringValue.Substring(0, length)}...";
                    }
                }

                return value;
            }

            throw new ArgumentException($"{nameof(value)} is expected to be a string");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}