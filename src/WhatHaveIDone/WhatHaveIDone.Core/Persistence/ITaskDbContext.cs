using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.Persistence
{
    public interface ITaskDbContext : IDisposable
    {
        Task AddTaskType(TaskType taskType);
        Task<bool> TaskTypeExists(int id);

        Task<IReadOnlyList<TaskType>> GetAllTypes();

        Task<IReadOnlyList<TaskModel>> GetTasksInIntervalAsync(DateTime start, DateTime end);

        Task<TaskModel> CreateTaskAsync(string taskName, TaskType category, string comment, DateTime utcNow);

        Task<TaskModel> GetTaskByIdAsync(Guid id);

        Task SaveChangesAsync();

        Task<bool> DeleteTaskById(Guid id);
    }
}