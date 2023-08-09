using Newtonsoft.Json;
using System.IO;
using WhatHaveIDone.Core.Configuration;

namespace WhatHaveIDone.Core.Json
{
    public class JsonSerializer : IJsonSerializer
    {
        public TaskConfiguration DeserializeTaskConfiguration(StreamReader reader)
        {
            var jsonSerializer = Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings { Converters = new[] { new HexStringColorConverter() } });

            return jsonSerializer.Deserialize<TaskConfiguration>(new JsonTextReader(reader));
        }
    }
}
