using System.IO;
using WhatHaveIDone.Core.Json;

namespace WhatHaveIDone.Core.Configuration
{
    public class ConfigurationProvider: IConfigurationProvider
    {
        private readonly IJsonSerializer _jsonSerializer;

        public ConfigurationProvider(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public TaskConfiguration LoadTaskConfiguration()
        {
            using var reader = new StreamReader("TaskConfiguration.json");
            return _jsonSerializer.DeserializeTaskConfiguration(reader);
        }
    }
}
