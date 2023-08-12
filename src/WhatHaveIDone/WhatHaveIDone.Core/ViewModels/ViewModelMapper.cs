using System.Collections.ObjectModel;
using System.Linq;
using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.ViewModels
{
    public static class ViewModelMapper
    {
        public static TaskViewModel MapTaskToViewModel(TaskModel task)
        {
            return new TaskViewModel
            {
                Begin = task.Begin,
                End = task.End,
                Id = task.Id,
                Comment = task.Comment,
                Name = task.Name,
                TaskType = task.TaskType,
                DynamicPropertyValues = new ObservableCollection<TaskPropertyViewModel>(task.Properties.Select(x => MapTaskPropertyToViewModel(x)))
            };
        }

        public static TaskPropertyViewModel MapTaskPropertyToViewModel(TaskProperty taskProperty)
        {
            return new TaskPropertyViewModel
            {
                Id = taskProperty.Id,
                Name = taskProperty.Name,
                Value = taskProperty.Value,
                TaskPropertyType = taskProperty.TaskPropertyType
            };
        }

        public static TaskPropertyViewModel CreateTaskPropertyViewModel(TaskPropertyType type)
        {
            return new TaskPropertyViewModel
            {
                Id = type.Id,
                Name = type.Name,
                Value = type.DefaultValue,
                TaskPropertyType = type
            };
        }

        public static void UpdateTaskModel(TaskModel task, TaskViewModel viewModel)
        {
            task.Begin = viewModel.Begin;
            task.Comment = viewModel.Comment;
            task.TaskType = viewModel.TaskType;
            task.End = viewModel.End;
            task.Name = viewModel.Name;

            foreach(var item in viewModel.DynamicPropertyValues) 
            {                 
                var taskProperty = task.Properties.Single(x => x.TaskPropertyType == item.TaskPropertyType);
                taskProperty.Value = item.Value;
            }
        }
    }
}