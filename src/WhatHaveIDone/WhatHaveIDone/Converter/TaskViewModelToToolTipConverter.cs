using System;
using System.Globalization;
using System.Windows.Data;

namespace WhatHaveIDone.Converter
{
    public class TaskViewModelToToolTipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 3 && values[0] is string taskName && values[1] is DateTime taskStart)
            {
                var taskEnd = (DateTime?)values[2];

                if (taskEnd.HasValue)
                {
                    var duration = taskEnd - taskStart;
                    return $"{taskName} {duration:hh\\:mm}  [{taskStart.ToLocalTime():HH:mm}-{taskEnd.Value.ToLocalTime():HH:mm}]";
                }

                return $"{taskName} [{taskStart.ToLocalTime():HH:mm} ...";
            }

            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}