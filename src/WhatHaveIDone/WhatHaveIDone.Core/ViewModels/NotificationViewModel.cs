using MvvmCross.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using WhatHaveIDone.Core.CoreAbstractions;
using WhatHaveIDone.Core.Persistence;

namespace WhatHaveIDone.Core.ViewModels
{
    public class NotificationViewModel : MvxViewModel
    {
        public NotificationViewModel(ITaskDbContext dbContext, IDispatcherTimer dispatcherTimer)
        {
            dispatcherTimer.StartTimer(TimeSpan.FromSeconds(1), UpdateRunningTask);
            _dbContext = dbContext;
        }

        private bool _isTaskRunning;

        public bool IsTaskRunning
        {
            get { return _isTaskRunning; }
            set { SetProperty(ref _isTaskRunning, value); }
        }

        private TaskViewModel _currentTask;
        private readonly ITaskDbContext _dbContext;

        public override async Task Initialize()
        {
            var tasks = await _dbContext.GetTasksInIntervalAsync(DateTime.Today.ToUniversalTime(), DateTime.Today.AddDays(1).ToUniversalTime());

            var runningTask = tasks.FirstOrDefault(x => !x.End.HasValue);
            if (runningTask != null)
            {
                CurrentTask = ViewModelMapper.MapTaskToViewModel(runningTask);
                IsTaskRunning = true;
            }
            else
            {
                CurrentTask = null;
                IsTaskRunning = false;
            }
        }

        public TaskViewModel CurrentTask
        {
            get { return _currentTask; }
            set { SetProperty(ref _currentTask, value); }
        }

        private void UpdateRunningTask()
        {
            if (CurrentTask != null)
            {
                CurrentTask.TemporaryEnd = DateTime.UtcNow;
            }
        }
    }
}