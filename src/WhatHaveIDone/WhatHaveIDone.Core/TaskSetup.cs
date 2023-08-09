using System.Threading.Tasks;
using WhatHaveIDone.Core.Configuration;
using WhatHaveIDone.Core.Persistence;

namespace WhatHaveIDone.Core
{
    public class TaskSetup : ITaskSetup
    {
        private readonly ITaskDbContext _taskDbContext;
        private readonly IConfigurationProvider _configuration;

        public TaskSetup(ITaskDbContext taskDbContext, IConfigurationProvider configuration)
        {
            _taskDbContext = taskDbContext;
            _configuration = configuration;
        }

        public async Task AddTaskTypesToDatabase()
        {
            var taskConfiguration = _configuration.LoadTaskConfiguration();
            foreach (var task in taskConfiguration.TaskTypes)
            {
                if (!await _taskDbContext.TaskTypeExists(task.Id))
                {
                    await _taskDbContext.AddTaskType(task);
                }
            }
        }
    }
}