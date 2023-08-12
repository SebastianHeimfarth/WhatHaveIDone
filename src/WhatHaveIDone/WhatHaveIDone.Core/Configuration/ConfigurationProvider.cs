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

        public GeneralSettings LoadGeneralSettings()
        {
            using var reader = new StreamReader("GeneralSettings.json");
            return _jsonSerializer.DeserializeGeneralSettings(reader);
        }

        public TaskConfiguration LoadTaskConfiguration()
        {
            using var reader = new StreamReader("TaskConfiguration.json");
            return _jsonSerializer.DeserializeTaskConfiguration(reader);
        }
    }
}
