using System;
using System.Globalization;
using System.Windows.Data;
using WhatHaveIDone.CustomControls;

namespace WhatHaveIDone.Converter
{
    public class TaskWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 5 && values[0] is double parentWidth && values[1] is DateTime timeLineStart && values[2] is DateTime timeLineEnd && values[3] is DateTime taskBegin)
            {
                DateTime? taskEnd = (DateTime?)values[4];

                if (timeLineEnd == timeLineStart || taskBegin == taskEnd.GetValueOrDefault()) 
                {
                    return 0d;
                }
                else
                {
                    var scalingPerMinute = parentWidth / (timeLineEnd - timeLineStart).TotalMinutes;

                    if (!taskEnd.HasValue)
                    {
                        return 50d * scalingPerMinute;
                    }

                    return (taskEnd.Value - taskBegin).TotalMinutes * scalingPerMinute;
                }
            }

            throw new ArgumentException("expected 5 Arguments 'parentWidth'  'timeLineStart' 'timeLineEnd' 'taskStart' 'taskEnd'");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
