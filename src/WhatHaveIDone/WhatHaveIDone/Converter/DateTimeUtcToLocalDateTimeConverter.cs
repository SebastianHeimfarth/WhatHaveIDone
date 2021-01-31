using System;
using System.Globalization;
using System.Windows.Data;

namespace WhatHaveIDone.Converter
{
    public class DateTimeUtcToLocalDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTime dateTime)
            {
                return dateTime.ToLocalTime();
            }
            var nullableDateTime = value as DateTime?;

            if(nullableDateTime != null)
            {
                return nullableDateTime.GetValueOrDefault().ToLocalTime();
            }

            return default(DateTime);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToUniversalTime();
            }
            var nullableDateTime = value as DateTime?;

            if (nullableDateTime != null)
            {
                return nullableDateTime.GetValueOrDefault().ToUniversalTime();
            }

            return default(DateTime);
        }
    }
}
