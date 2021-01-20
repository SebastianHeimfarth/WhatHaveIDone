
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.Persistence
{
    public interface ITaskDbContext
    {
        Task<IReadOnlyList<TaskModel>> GetTasksInIntervalAsync(DateTime start, DateTime end);
        Task<TaskModel> CreateTaskAsync(string taskName, string comment, DateTime utcNow);
        Task<TaskModel> GetTaskByIdAsync(Guid id);
        Task SaveChangesAsync();
    }


}
