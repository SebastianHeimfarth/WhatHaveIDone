using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.ViewModels
{
    public static class ViewModelMapper
    {
        public static TaskViewModel MapTaskToViewModel(TaskModel task)
        {
            return new TaskViewModel { Begin = task.Begin, End = task.End, Id = task.Id, Comment = task.Comment, Name = task.Name };
        }

        public static void UpdateTaskModel(TaskModel task, TaskViewModel viewModel)
        {
            task.Begin = viewModel.Begin;
            task.Comment = viewModel.Comment;
            task.End = viewModel.End;
        }
    }
}