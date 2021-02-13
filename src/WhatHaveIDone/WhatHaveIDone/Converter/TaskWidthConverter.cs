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
            if (values != null && values.Length == 6 && values[0] is double parentWidth && values[1] is DateTime timeLineStart && values[2] is DateTime timeLineEnd && values[3] is DateTime taskBegin && values[5] is DateTime taskTemporaryEnd)
            {
                DateTime? taskEnd = (DateTime?)values[4];

                if (timeLineEnd == timeLineStart || taskBegin == taskEnd.GetValueOrDefault()) 
                {
                    return 0d;
                }
                else
                {
                    var scalingPerMinute = parentWidth / ((timeLineEnd - timeLineStart).TotalMinutes + 2 * TaskTimelineControl.ExtraSpacingOnBeginningAndEnd);

                    if (!taskEnd.HasValue)
                    {
                        var defaultWidth = 15d * scalingPerMinute;
                        var temporaryWidth = (taskTemporaryEnd - taskBegin).TotalMinutes * scalingPerMinute;

                        return Math.Max(temporaryWidth, defaultWidth);
                    }

                    var minWidth = 5d * scalingPerMinute;

                    return  Math.Max(minWidth, (taskEnd.Value - taskBegin).TotalMinutes * scalingPerMinute);
                }
            }

            throw new ArgumentException("expected 6 Arguments 'parentWidth'  'timeLineStart' 'timeLineEnd' 'taskStart' 'taskEnd' 'taskTemporaryEnd'");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
