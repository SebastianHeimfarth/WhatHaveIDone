using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace WhatHaveIDone.Core.ViewModels
{
    public class TaskListViewModel : MvxViewModel
    {
        private string _comment;
        private TaskModel _currentTask;
        private string _taskName;

        private ObservableCollection<TaskModel> _tasks = new ObservableCollection<TaskModel>();
        private bool _isTaskPaused;

        public TaskListViewModel()
        {
            StartTaskCommand = new MvxCommand(StartTask);
            StopTaskCommand = new MvxCommand(StopTask);
            EndTaskCommand = new MvxCommand(EndTask);
            ContinueTaskCommand = new MvxCommand(ContinueTask);
        }

        public bool CanStartTask => !string.IsNullOrEmpty(TaskName);
        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        public TaskModel CurrentTask
        {
            get { return _currentTask; }
            set { SetProperty(ref _currentTask, value); RaisePropertyChanged(() => IsTaskStarted); }
        }

        public bool IsTaskStarted => CurrentTask != null;
        public IMvxCommand StartTaskCommand { get; }
        public IMvxCommand StopTaskCommand { get; }
        public IMvxCommand ContinueTaskCommand { get; }

        public void ContinueTask()
        {
            CurrentTask = CurrentTask.CreateContinuationTask();
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

        public void EndTask()
        {
            CurrentTask.End = DateTime.UtcNow;
            IsTaskPaused = false;
            CurrentTask = null;
        }

        public ObservableCollection<TaskModel> Tasks
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

        public void StartTask()
        {
            CurrentTask = new TaskModel() { Name = TaskName, Comment = Comment, Begin = DateTime.UtcNow };
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