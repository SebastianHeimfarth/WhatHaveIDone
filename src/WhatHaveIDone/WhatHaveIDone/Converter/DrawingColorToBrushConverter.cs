using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using DrawingColor = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;

namespace WhatHaveIDone.Converter
{
    public class DrawingColorToBrushConverter : IValueConverter
    {
        private readonly MediaColor _defaultMediaColor = System.Windows.Media.Colors.Transparent;

        private readonly IDictionary<DrawingColor, SolidColorBrush> _cache = new Dictionary<DrawingColor, SolidColorBrush>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DrawingColor color)
            {
                if (!_cache.TryGetValue(color, out var brush))
                {
                    brush = new SolidColorBrush(MediaColor.FromArgb(color.A, color.R, color.G, color.B));
                    _cache[color] = brush;
                }

                return brush;
            }

            return _defaultMediaColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}