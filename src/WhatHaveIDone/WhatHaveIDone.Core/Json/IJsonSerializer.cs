using System.IO;
using WhatHaveIDone.Core.Configuration;

namespace WhatHaveIDone.Core.Json
{
    public interface IJsonSerializer
    {
        GeneralSettings DeserializeGeneralSettings(StreamReader reader);
        TaskConfiguration DeserializeTaskConfiguration(StreamReader reader);
    }
}