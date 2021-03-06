﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WhatHaveIDone.CustomControls;

namespace WhatHaveIDone.Converter
{
    public class TaskLeftOffsetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 4 && values[0] is double parentWidth && values[1] is DateTime timeLineStart && values[2] is DateTime timeLineEnd && values[3] is DateTime taskBegin)
            {
                double left;
                if (taskBegin <= timeLineStart || timeLineEnd == timeLineStart)
                {
                    left = 0;
                }
                else
                {
                    var scalingPerMinute = parentWidth / ((timeLineEnd - timeLineStart).TotalMinutes + 2 * TaskTimelineControl.ExtraSpacingOnBeginningAndEnd);
                    left = ((taskBegin - timeLineStart).TotalMinutes + TaskTimelineControl.ExtraSpacingOnBeginningAndEnd) * scalingPerMinute;
                }

                return new Thickness(left, 5, 0, 0);
            }

            throw new ArgumentException("expected 4 Arguments 'parentWidth'  'timeLineStart' 'timeLineEnd' 'taskStart'");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}