using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Converter
{
    public class TaskViewModelToTaskDurationStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is TaskViewModel viewModel && viewModel.End.HasValue)
            {
                return $"({viewModel.End.Value - viewModel.Begin:hh\\:mm})";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
