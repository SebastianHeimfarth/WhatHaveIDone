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
                DynamicPropertyValues = task.DynamicPropertyValues.Select(x => MapTaskPropertyToViewModel(x)).ToArray()
            };
        }

        public static TaskPropertyViewModel MapTaskPropertyToViewModel(TaskProperty taskProperty)
        {
            return new TaskPropertyViewModel
            {
                Id = taskProperty.Id,
                Name = taskProperty.Name,
                Value = taskProperty.Value
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
                var taskProperty = task.DynamicPropertyValues.Single(x => x.Id == item.Id);
                taskProperty.Value = item.Value;
            }
        }
    }
}