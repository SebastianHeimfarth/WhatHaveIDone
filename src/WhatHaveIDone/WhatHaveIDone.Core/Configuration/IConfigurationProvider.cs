using System.Threading.Tasks;

namespace WhatHaveIDone.Core.Configuration
{
    public interface IConfigurationProvider
    {
        TaskConfiguration LoadTaskConfiguration();
    }
}