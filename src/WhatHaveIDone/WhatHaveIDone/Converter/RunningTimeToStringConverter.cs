using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WhatHaveIDone.Converter
{
    public class RunningTimeToStringConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values[0] is DateTime begin && values[1] is DateTime temporaryEnd)
            {
                var elapsed = temporaryEnd - begin;

                if (elapsed.TotalSeconds < 60)
                {
                    return $"{(int)elapsed.TotalSeconds}sec";
                }
                if(elapsed.TotalMinutes < 60)
                {
                    return $"{elapsed.Minutes}min {elapsed.Seconds}sec";
                }
                return $"{(int)elapsed.TotalHours}h {elapsed.Minutes}min {elapsed.Seconds}sec";

            }

            return "n/a";
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
