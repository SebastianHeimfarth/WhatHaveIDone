using System;
using System.Globalization;
using System.Windows.Data;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Converter
{
    public class IsSelectedTaskConverter : IMultiValueConverter
    {
        private const double BorderThicknessWhenElementIsSelected = 4d;
        private const double BorderThicknessWhenElementIsNotSelected = 0d;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values != null && values.Length == 2 && values[0] is TaskViewModel task && values[1] is TaskViewModel selectedTask)
            {
                if(task == selectedTask)
                {
                    return BorderThicknessWhenElementIsSelected;
                }
            }

            return BorderThicknessWhenElementIsNotSelected;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
