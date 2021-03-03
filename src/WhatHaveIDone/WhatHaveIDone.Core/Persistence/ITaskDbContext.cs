using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.Persistence
{
    public interface ITaskDbContext : IDisposable
    {
        Task<IReadOnlyList<TaskCategory>> GetAllTaskCategories();

        Task<IReadOnlyList<TaskModel>> GetTasksInIntervalAsync(DateTime start, DateTime end);

        Task<TaskModel> CreateTaskAsync(string taskName, TaskCategory category, string comment, DateTime beginUtc);

        Task<TaskModel> GetTaskByIdAsync(Guid id);

        Task SaveChangesAsync();

        Task<bool> DeleteTaskById(Guid id);
    }
}