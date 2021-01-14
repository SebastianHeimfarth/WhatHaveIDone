using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.ViewModels
{
    public class TaskListViewModel : MvxViewModel
    {
        private string _comment;
        private string _taskName;
        private ObservableCollection<TaskModel> _tasks = new ObservableCollection<TaskModel>();

        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        public string TaskName
        {
            get { return _taskName; }
            set
            {
                SetProperty(ref _taskName, value);
                RaisePropertyChanged(() => CanAddTask);
            }
        }

        public ObservableCollection<TaskModel> Tasks
        {
            get { return _tasks; }
            set { SetProperty(ref _tasks, value); }
        }

        public IMvxCommand AddTaskCommand { get; }

        public bool CanAddTask => !string.IsNullOrEmpty(TaskName);

        public TaskListViewModel()
        {
            AddTaskCommand = new MvxCommand(AddTask);
        }

        public void AddTask()
        {
            Tasks.Add(new TaskModel() { Name = TaskName, Comment = Comment });
            TaskName = null;
            Comment = null;
        }
    }
}