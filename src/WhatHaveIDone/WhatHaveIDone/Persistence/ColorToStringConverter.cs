using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Drawing;

namespace WhatHaveIDone.Persistence
{
    public class ColorToIntegerConverter : ValueConverter<Color, int>
    {
        public static int ConvertColorToInt(Color color)
        {
            return color.ToArgb();
        }

        public static Color ConvertIntToColor(int argb)
        {
            return Color.FromArgb(argb);
        }

        public ColorToIntegerConverter(ConverterMappingHints mappingHints = null) :
            base(x => ConvertColorToInt(x), x => ConvertIntToColor(x), mappingHints)
        {
        }
    }
}