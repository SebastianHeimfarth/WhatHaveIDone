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
        private readonly ObservableCollection<TaskCategory> _categories = new ObservableCollection<TaskCategory>();
        private readonly ITaskDbContext _taskDbContext;
        private readonly ObservableCollection<TaskViewModel> _tasks = new ObservableCollection<TaskViewModel>();
        private string _comment;

        private TaskViewModel _currentTask;

        private DateTime _dayInLocalTime;

        private bool _isTaskPaused;

        private TaskViewModel _selectedTask;

        private string _taskName;

        private IReadOnlyList<TaskStatisticViewModel> _taskStatistics;

        public TaskDayEditorViewModel(ITaskDbContext taskDbContext)
        {
            DayInLocalTime = DateTime.Today;
            StartTaskCommand = new MvxAsyncCommand(StartTask);
            StopTaskCommand = new MvxCommand(StopTask);
            EndTaskCommand = new MvxAsyncCommand(EndTask);
            ContinueTaskCommand = new MvxAsyncCommand(ContinueTask);
            _taskDbContext = taskDbContext;

            _tasks.CollectionChanged += OnTaskCollectionChanged;
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
                if (SetProperty(ref _dayInLocalTime, value))
                {
                    RaisePropertyChanged(() => DayUtc);
                    RaisePropertyChanged(() => EndUtc);
                }
            }
        }

        public DateTime DayUtc => DayInLocalTime.ToUniversalTime();

        public IMvxCommand EndTaskCommand { get; }

        public DateTime EndUtc => DayUtc.Add(TimeLineLength);

        public bool IsTaskPaused
        {
            get => _isTaskPaused;
            set
            {
                SetProperty(ref _isTaskPaused, value);
            }
        }

        public bool IsTaskStarted => CurrentTask != null;

        public TaskViewModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (_selectedTask != null)
                {
                    UpdateTask(_selectedTask).Wait();
                }
                SetProperty(ref _selectedTask, value);
            }
        }

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

        public ObservableCollection<TaskCategory> Categories => _categories;

        private TaskCategory _categoryForNewTask;
        public TaskCategory CategoryForNewTask
        {
            get { return _categoryForNewTask; }
            set { SetProperty(ref _categoryForNewTask, value); }
        }

        public IReadOnlyList<TaskStatisticViewModel> TaskStatistics
        {
            get { return _taskStatistics; }
            set { SetProperty(ref _taskStatistics, value); }
        }

        public TimeSpan TimeLineLength => TimeSpan.FromHours(24);

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
            await UpdateTask(CurrentTask);

            IsTaskPaused = false;
            CurrentTask = null;
        }

        public async Task Load()
        {
            var taskCategories = await _taskDbContext.GetAllTaskCategories();
            foreach (var category in taskCategories)
            {
                _categories.Add(category);
            }
            CategoryForNewTask = taskCategories.Single(x => x.Name == "Default");

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

        public async Task OnBeforeClosing()
        {
            if(SelectedTask != null)
            {
                await UpdateTask(SelectedTask);
            }
        }
        public async Task StartTask()
        {
            var task = await _taskDbContext.CreateTaskAsync(TaskName, CategoryForNewTask, Comment, DateTime.UtcNow);

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
            var taskModel = await _taskDbContext.CreateTaskAsync(CurrentTask.Name, CurrentTask.Category, CurrentTask.Comment, DateTime.UtcNow);

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

        private async Task UpdateTask(TaskViewModel taskViewModel)
        {
            var taskModel = await _taskDbContext.GetTaskByIdAsync(taskViewModel.Id);
            UpdateTaskModel(taskModel, taskViewModel);
            await _taskDbContext.SaveChangesAsync();
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