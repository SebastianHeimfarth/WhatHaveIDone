using System.Threading.Tasks;

namespace WhatHaveIDone.Core
{
    public interface ITaskSetup
    {
        Task AddTaskTypesToDatabase();
    }
}