using Newtonsoft.Json;
using System;

namespace WhatHaveIDone.Core.Json
{
    public class TimeOnlyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeOnly);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var stringValue = (string)reader.Value;
            return TimeOnly.ParseExact(stringValue, "HH:mm");
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value?.ToString());
        }
    }
}