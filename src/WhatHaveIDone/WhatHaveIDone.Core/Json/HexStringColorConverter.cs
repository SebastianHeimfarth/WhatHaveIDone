﻿using System;
using System.Drawing;

namespace WhatHaveIDone.Core.Json
{
    public class HexStringColorConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var hexString = (string)reader.Value;
            var intValue = int.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
            var baseColor = Color.FromArgb(intValue);
            return Color.FromArgb(255, baseColor);
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var intValue = ((Color)value).ToArgb();
            var hexString = intValue.ToString("X");
            writer.WriteValue(hexString);
        }
    }
}