using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WhatHaveIDone.Core.CoreAbstractions;
using WhatHaveIDone.Core.Models;
using WhatHaveIDone.Core.Persistence;
using static WhatHaveIDone.Core.ViewModels.ViewModelMapper;

namespace WhatHaveIDone.Core.ViewModels
{
    public class TaskDayEditorViewModel : MvxViewModel<DateTime>
    {
        private const int TaskMovingAmountInMinutes = 5;
        private readonly ObservableCollection<TaskCategory> _categories = new ObservableCollection<TaskCategory>();
        private readonly ITaskDbContext _taskDbContext;
        private readonly IMvxNavigationService _navigationService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly ObservableCollection<TaskViewModel> _tasks = new ObservableCollection<TaskViewModel>();
        private string _comment;

        private TaskViewModel _runningTask;

        private DateTime _dayInLocalTime;

        private bool _isTaskPaused;

        private TaskViewModel _selectedTask;

        private string _taskName;

        private IReadOnlyList<TaskStatisticViewModel> _taskStatistics;

        public TaskDayEditorViewModel(ITaskDbContext taskDbContext, IMvxNavigationService navigationService, IMessageBoxService messageBoxService, IDispatcherTimer dispatcherTimer)
        {
            StartTaskCommand = new MvxAsyncCommand(StartTask);
            StopTaskCommand = new MvxCommand(StopTask);
            EndTaskCommand = new MvxAsyncCommand(EndTask);
            ContinuePausedTaskCommand = new MvxAsyncCommand(ContinuePausedTask);
            ContinueSelectedTaskCommand = new MvxAsyncCommand(ContinueSelectedTask);
            DeleteTaskCommand = new MvxAsyncCommand(DeleteTask);

            MoveTaskBeginLeftCommand = new MvxCommand(MoveTaskBeginLeft);
            MoveTaskEndRightCommand = new MvxCommand(MoveTaskEndRight);
            MoveTaskBeginRightCommand = new MvxCommand(MoveTaskBeginRight);
            MoveTaskEndLeftCommand = new MvxCommand(MoveTaskEndLeft);

            NavigateToNextDayCommand = new MvxAsyncCommand(NavigateToNextDay);
            NavigateToPreviousDayCommand = new MvxAsyncCommand(NavigateToPreviousDay);

            _taskDbContext = taskDbContext;
            _navigationService = navigationService;
            _messageBoxService = messageBoxService;
            _tasks.CollectionChanged += OnTaskCollectionChanged;

            dispatcherTimer.StartTimer(TimeSpan.FromSeconds(1), UpdateRunningTask);
        }

        public override void Prepare(DateTime dateTime)
        {
            DayInLocalTime = dateTime;
        }

        private void MoveTaskEndLeft()
        {
            SelectedTask.End = SelectedTask.End.Value.AddMinutes(-TaskMovingAmountInMinutes);
        }

        public bool IsSelectedTaskFinished => SelectedTask != null && SelectedTask.End.HasValue;

        private void MoveTaskBeginRight()
        {
            SelectedTask.Begin = SelectedTask.Begin.AddMinutes(TaskMovingAmountInMinutes);
        }

        private void MoveTaskEndRight()
        {
            SelectedTask.End = SelectedTask.End.Value.AddMinutes(TaskMovingAmountInMinutes);
        }

        private void MoveTaskBeginLeft()
        {
            SelectedTask.Begin = SelectedTask.Begin.AddMinutes(-TaskMovingAmountInMinutes);
        }

        private void UpdateRunningTask()
        {
            if (RunningTask != null)
            {
                RunningTask.TemporaryEnd = DateTime.UtcNow;
            }
        }

        public bool CanStartTask => !string.IsNullOrEmpty(TaskName);

        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        public IMvxCommand ContinuePausedTaskCommand { get; }
        public IMvxCommand ContinueSelectedTaskCommand { get; }

        public TaskViewModel RunningTask
        {
            get { return _runningTask; }
            set { SetProperty(ref _runningTask, value); RaisePropertyChanged(() => IsTaskStarted); }
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

        public bool IsTaskStarted => RunningTask != null;

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
                RaisePropertyChanged(() => IsSelectedTaskFinished);
            }
        }

        public IMvxCommand StartTaskCommand { get; }
        public IMvxCommand DeleteTaskCommand { get; }
        public IMvxCommand StopTaskCommand { get; }
        public IMvxCommand MoveTaskBeginLeftCommand { get; }
        public IMvxCommand MoveTaskEndRightCommand { get; }
        public IMvxCommand MoveTaskBeginRightCommand { get; }
        public IMvxCommand MoveTaskEndLeftCommand { get; }
        public IMvxCommand NavigateToNextDayCommand { get; }
        public IMvxCommand NavigateToPreviousDayCommand { get; }

        public Task NavigateToNextDay()
        {
            if (DayInLocalTime < DateTime.Today)
            {
                return _navigationService.Navigate<TaskDayEditorViewModel, DateTime>(DayInLocalTime.AddDays(1));
            }

            return Task.CompletedTask;
        }

        public Task NavigateToPreviousDay()
        {
            return _navigationService.Navigate<TaskDayEditorViewModel, DateTime>(DayInLocalTime.AddDays(-1));
        }

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

        public  Task ContinuePausedTask()
        {
            return ContinueTask(RunningTask);
        }

        public Task ContinueSelectedTask()
        {
            if(RunningTask == null)
            {
                return ContinueTask(SelectedTask);
            }

            return Task.CompletedTask;
        }

        public async Task EndTask()
        {
            RunningTask.End = DateTime.UtcNow;
            await UpdateTask(RunningTask);

            IsTaskPaused = false;
            RunningTask = null;
        }

        public async Task Load()
        {
            var taskCategories = await _taskDbContext.GetAllTaskCategories();
            foreach (var category in taskCategories)
            {
                _categories.Add(category);
            }
            CategoryForNewTask = taskCategories.Single(x => x.Name == "Work");

            var tasksForThisWeek = await _taskDbContext.GetTasksInIntervalAsync(DayInLocalTime.Date.ToUniversalTime(), DayInLocalTime.Date.AddDays(1).ToUniversalTime());

            foreach (var task in tasksForThisWeek)
            {
                var taskViewModel = MapTaskToViewModel(task);
                _tasks.Add(taskViewModel);

                if (!taskViewModel.End.HasValue)
                {
                    RunningTask = taskViewModel;
                }
            }
        }

        public async Task OnBeforeClosing()
        {
            if (SelectedTask != null)
            {
                await UpdateTask(SelectedTask);
            }
        }

        public async Task StartTask()
        {
            var task = await _taskDbContext.CreateTaskAsync(TaskName, CategoryForNewTask, Comment, DateTime.UtcNow);

            RunningTask = MapTaskToViewModel(task);
            Tasks.Add(RunningTask);

            TaskName = null;
            Comment = null;
        }

        private async Task DeleteTask()
        {
            if (SelectedTask != null)
            {
                if (_messageBoxService.AskYesNoQuestion("Are you sure to delete the Task?", "Delete?"))
                {
                    var task = SelectedTask;

                    if (RunningTask == task)
                    {
                        RunningTask = null;
                    }

                    Tasks.Remove(task);
                    SelectedTask = null;
                    await _taskDbContext.DeleteTaskById(task.Id);
                }
            }
        }

        public void StopTask()
        {
            RunningTask.End = DateTime.UtcNow;
            IsTaskPaused = true;
            RaisePropertyChanged(() => IsSelectedTaskFinished);
            UpdateTaskStatistics();
        }

        private async Task ContinueTask(TaskViewModel taskViewModel)
        {
            RunningTask = await CreateContinuationTask(taskViewModel);
            Tasks.Add(RunningTask);
            IsTaskPaused = false;
        }

        private async Task<TaskViewModel> CreateContinuationTask(TaskViewModel taskViewModel)
        {
            var taskModel = await _taskDbContext.CreateTaskAsync(taskViewModel.Name, taskViewModel.Category, taskViewModel.Comment, DateTime.UtcNow);

            return MapTaskToViewModel(taskModel);
        }

        private void OnTaskCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateTaskStatistics();
        }

        private void UpdateTaskStatistics()
        {
            TaskStatistics = Tasks.
                            Where(x => x.End.HasValue).
                            GroupBy(x => x.Category).
                            Select(x => new TaskStatisticViewModel
                            {
                                Category = x.Key,
                                TimeSpan = new TimeSpan(x.Sum(y => (y.End.Value - y.Begin).Ticks))
                            }).ToList();
        }

        private async Task UpdateTask(TaskViewModel taskViewModel)
        {
            var taskModel = await _taskDbContext.GetTaskByIdAsync(taskViewModel.Id);
            UpdateTaskModel(taskModel, taskViewModel);
            UpdateTaskStatistics();
            await _taskDbContext.SaveChangesAsync();
        }
    }
}