using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WhatHaveIDone.Core.Persistence;
using WhatHaveIDone.Core.Util;
using static WhatHaveIDone.Core.ViewModels.ViewModelMapper;

namespace WhatHaveIDone.Core.ViewModels
{
    public class TaskListViewModel : MvxViewModel
    {
        private string _comment;
        private TaskViewModel _currentTask;
        private string _taskName;

        private ObservableCollection<TaskViewModel> _tasks = new ObservableCollection<TaskViewModel>();
        private bool _isTaskPaused;
        private readonly ITaskDbContext _taskDbContext;

        public TaskListViewModel(ITaskDbContext taskDbContext)
        {
            StartTaskCommand = new MvxCommand(async () => await StartTask());
            StopTaskCommand = new MvxCommand(StopTask);
            EndTaskCommand = new MvxCommand(async () => await EndTask());
            ContinueTaskCommand = new MvxCommand(async () => await ContinueTask());
            _taskDbContext = taskDbContext;
        }

        public async Task Load()
        {
            var weekStart = DateTime.Today.GetStartOfWeek();
            var endOfWeek = weekStart.AddDays(7);

            var tasksForThisWeek = await _taskDbContext.GetTasksInIntervalAsync(weekStart, endOfWeek);

            foreach(var task in tasksForThisWeek)
            {
                var taskViewModel = MapTaskToViewModel(task);
                _tasks.Add(taskViewModel);

                if(!taskViewModel.End.HasValue)
                {
                    CurrentTask = taskViewModel;
                }
            }
        }

        public bool CanStartTask => !string.IsNullOrEmpty(TaskName);
        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        public TaskViewModel CurrentTask
        {
            get { return _currentTask; }
            set { SetProperty(ref _currentTask, value); RaisePropertyChanged(() => IsTaskStarted); }
        }

        public bool IsTaskStarted => CurrentTask != null;
        public IMvxCommand StartTaskCommand { get; }
        public IMvxCommand StopTaskCommand { get; }
        public IMvxCommand ContinueTaskCommand { get; }

        public async Task ContinueTask()
        {
            CurrentTask = await CreateContinuationTask();
            Tasks.Add(CurrentTask);
            IsTaskPaused = false;
        }

        public IMvxCommand EndTaskCommand { get; }

        public string TaskName
        {
            get { return _taskName; }
            set
            {
                SetProperty(ref _taskName, value);
                RaisePropertyChanged(() => CanStartTask);
            }
        }

        public async Task EndTask()
        {
            CurrentTask.End = DateTime.UtcNow;

            var taskModel = await _taskDbContext.GetTaskByIdAsync(CurrentTask.Id);
            UpdateTaskModel(taskModel, CurrentTask);
            await _taskDbContext.SaveChangesAsync();

            IsTaskPaused = false;
            CurrentTask = null;
        }

        public ObservableCollection<TaskViewModel> Tasks
        {
            get { return _tasks; }
            set { SetProperty(ref _tasks, value); }
        }

        public bool IsTaskPaused
        {
            get => _isTaskPaused; 
            set
            {
                SetProperty(ref _isTaskPaused, value);
            }
        }

        private async Task<TaskViewModel> CreateContinuationTask()
        {
            var taskModel = await _taskDbContext.CreateTaskAsync(TaskName, Comment, DateTime.UtcNow);

            return MapTaskToViewModel(taskModel);
        }

        public async Task StartTask()
        {
            var task = await _taskDbContext.CreateTaskAsync(TaskName, Comment, DateTime.UtcNow);

            CurrentTask = MapTaskToViewModel(task);
            Tasks.Add(CurrentTask);

            TaskName = null;
            Comment = null;
        }

        public void StopTask()
        {
            CurrentTask.End = DateTime.UtcNow;
            IsTaskPaused = true;
        }
    }
}