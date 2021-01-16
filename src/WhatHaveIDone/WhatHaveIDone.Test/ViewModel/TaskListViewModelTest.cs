using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Test.ViewModel
{
    public class TaskListViewModelTest
    {
        private const string TaskName = "TaskName";

        [Test]
        public void StartTask_ByDefault_ShouldAddTask()
        {
            //arrange
            var sut = CreateTaskListViewModel();

            //act
            sut.TaskName = "My new task";
            sut.StartTask();

            //assert
            sut.IsTaskStarted.ShouldBeTrue();
            sut.Tasks.ShouldContain(x => x.Name == "My new task");
        }

        [Test]
        public void StopTask_ByDefault_ShouldPause()
        {
            //arrange
            var sut = CreateTaskListViewModel();
            StartTask(sut);

            //act
            sut.StopTask();

            //assert
            sut.IsTaskStarted.ShouldBeTrue();
            sut.IsTaskPaused.ShouldBeTrue();
        }

        [Test]
        public void ContinueTask_TaskIsPaused_ShouldContinue()
        {
            //arrange
            var sut = CreateTaskListViewModel();
            StartTask(sut);
            sut.StopTask();

            //act
            sut.ContinueTask();

            //assert
            sut.IsTaskPaused.ShouldBeFalse();
            sut.CurrentTask.ShouldNotBeNull();
        }

        [Test]
        public void StopAndContinueTask_ByDefault_ShouldCreate2Tasks()
        {
            //arrange
            var sut = CreateTaskListViewModel();
            StartTask(sut);
            
            //act
            sut.StopTask();
            sut.ContinueTask();
            sut.EndTask();

            //assert
            sut.Tasks.Count.ShouldBe(2);
        }

        [Test]
        public void EndTask_ByDefault_ShouldEnd()
        {
            //arrange
            var sut = CreateTaskListViewModel();
            StartTask(sut);
            var task = sut.Tasks.Single();
            sut.StopTask();

            //act
            sut.EndTask();

            //assert
            task.End.ShouldNotBeNull();
            sut.IsTaskPaused.ShouldBeFalse();
            sut.IsTaskStarted.ShouldBeFalse();
        }

        private void StartTask(TaskListViewModel taskListViewModel)
        {
            taskListViewModel.TaskName = TaskName;
            taskListViewModel.StartTask();
        }

        private TaskListViewModel CreateTaskListViewModel()
        {
            return new TaskListViewModel();
        }


    }
}
