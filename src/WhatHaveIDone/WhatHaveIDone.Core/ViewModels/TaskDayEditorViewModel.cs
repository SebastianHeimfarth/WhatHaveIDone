using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WhatHaveIDone.Core.Models;
using WhatHaveIDone.Core.Persistence;
using static WhatHaveIDone.Core.ViewModels.ViewModelMapper;

namespace WhatHaveIDone.Core.ViewModels
{
    public class TaskDayEditorViewModel : MvxViewModel
    {
        private readonly ITaskDbContext _taskDbContext;
        private readonly ObservableCollection<TaskViewModel> _tasks = new ObservableCollection<TaskViewModel>();
        private string _comment;
        private TaskViewModel _currentTask;
        private DateTime _dayInLocalTime;
        private bool _isTaskPaused;
        private string _taskName;
        private TaskViewModel _selectedTask;
        private IReadOnlyList<TaskStatisticViewModel> _taskStatistics;

        public TaskDayEditorViewModel(ITaskDbContext taskDbContext)
        {
            DayInLocalTime = DateTime.Today;
            StartTaskCommand = new MvxCommand(async () => await StartTask());
            StopTaskCommand = new MvxCommand(StopTask);
            EndTaskCommand = new MvxCommand(async () => await EndTask());
            ContinueTaskCommand = new MvxCommand(async () => await ContinueTask());
            _taskDbContext = taskDbContext;

            _tasks.CollectionChanged += OnTaskCollectionChanged;
        }

        public TaskViewModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                SetProperty(ref _selectedTask, value);
            }
        }

        public bool CanStartTask => !string.IsNullOrEmpty(TaskName);

        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        public IMvxCommand ContinueTaskCommand { get; }

        public TaskViewModel CurrentTask
        {
            get { return _currentTask; }
            set { SetProperty(ref _currentTask, value); RaisePropertyChanged(() => IsTaskStarted); }
        }

        public DateTime DayInLocalTime
        {
            get => _dayInLocalTime;
            private set
            {
                SetProperty(ref _dayInLocalTime, value);
            }
        }

        public IMvxCommand EndTaskCommand { get; }

        public bool IsTaskPaused
        {
            get => _isTaskPaused;
            set
            {
                SetProperty(ref _isTaskPaused, value);
            }
        }

        public bool IsTaskStarted => CurrentTask != null;

        public IMvxCommand StartTaskCommand { get; }

        public IMvxCommand StopTaskCommand { get; }

        public string TaskName
        {
            get { return _taskName; }
            set
            {
                SetProperty(ref _taskName, value);
                RaisePropertyChanged(() => CanStartTask);
            }
        }

        public ObservableCollection<TaskViewModel> Tasks
        {
            get { return _tasks; }
        }

        public IReadOnlyList<TaskStatisticViewModel> TaskStatistics
        {
            get { return _taskStatistics; }
            set { SetProperty(ref _taskStatistics, value); }
        }

        public Task ChangeDay(DateTime localDateTime)
        {
            DayInLocalTime = localDateTime;
            return Load();
        }

        public async Task ContinueTask()
        {
            CurrentTask = await CreateContinuationTask();
            Tasks.Add(CurrentTask);
            IsTaskPaused = false;
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

        public async Task Load()
        {
            var tasksForThisWeek = await _taskDbContext.GetTasksInIntervalAsync(DayInLocalTime.Date.ToUniversalTime(), DayInLocalTime.Date.AddDays(1).ToUniversalTime());

            foreach (var task in tasksForThisWeek)
            {
                var taskViewModel = MapTaskToViewModel(task);
                _tasks.Add(taskViewModel);

                if (!taskViewModel.End.HasValue)
                {
                    CurrentTask = taskViewModel;
                }
            }
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

        private async Task<TaskViewModel> CreateContinuationTask()
        {
            var taskModel = await _taskDbContext.CreateTaskAsync(CurrentTask.Name, CurrentTask.Comment, DateTime.UtcNow);

            return MapTaskToViewModel(taskModel);
        }

        private void OnTaskCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TaskStatistics = Tasks.
                Where(x => x.End.HasValue).
                GroupBy(x => x.Category).
                Select(x => new TaskStatisticViewModel
                {
                    Category = x.Key,
                    TotalMinutes = Convert.ToInt32(x.Sum(y => (y.End.Value - y.Begin).TotalMinutes))
                }).ToList();
        }
    }

    public class TaskStatisticViewModel : MvxViewModel
    {
        private TaskCategory _category;
        private int _totalMinutes;

        public TaskCategory Category
        {
            get => _category;
            set
            {
                SetProperty(ref _category, value);
            }
        }

        public int TotalMinutes
        {
            get => _totalMinutes;
            set
            {
                SetProperty(ref _totalMinutes, value);
            }
        }
    }
}