using System.IO;
using WhatHaveIDone.Core.Configuration;

namespace WhatHaveIDone.Core.Json
{
    public interface IJsonSerializer
    {
        TaskConfiguration DeserializeTaskConfiguration(StreamReader reader);
    }
}