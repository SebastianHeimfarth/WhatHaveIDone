using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatHaveIDone.Core.Persistence;
using WhatHaveIDone.Core.ViewModels;
using WhatHaveIDone.Persistence;
using WhatHaveIDone.Test.Persistence;

namespace WhatHaveIDone.Test.ViewModel
{
    public class TaskListViewModelTest
    {
        private const string TaskName = "TaskName";
        private TaskDbContext DataContext { get; set; }

        [Test]
        public async Task Load_UnfinishedTaskIsAvailable_ShouldLoadUnfinishedTask()
        {
            //arrange
            var sut = await CreateTaskListViewModel();
            await DataContext.CreateTaskAsync("unfinishedTask", "comment", DateTime.UtcNow);

            //act
            await sut.Load();


            //assert
            sut.CurrentTask.Name.ShouldBe("unfinishedTask");
        }

        [Test]
        public async Task StartTask_ByDefault_ShouldAddTask()
        {
            //arrange
            var sut = await CreateTaskListViewModel();

            //act
            sut.TaskName = "My new task";
            await sut.StartTask();

            //assert
            sut.IsTaskStarted.ShouldBeTrue();
            sut.Tasks.ShouldContain(x => x.Name == "My new task");
            DataContext.Tasks.Count().ShouldBe(1);
        }

        [Test]
        public async Task StopTask_ByDefault_ShouldPause()
        {
            //arrange
            var sut = await CreateTaskListViewModel();
            await StartTask(sut);

            //act
            sut.StopTask();

            //assert
            sut.IsTaskStarted.ShouldBeTrue();
            sut.IsTaskPaused.ShouldBeTrue();
        }

        [Test]
        public async Task ContinueTask_TaskIsPaused_ShouldContinue()
        {
            //arrange
            var sut = await CreateTaskListViewModel();
            await StartTask(sut);
            sut.StopTask();

            //act
            await sut.ContinueTask();

            //assert
            sut.IsTaskPaused.ShouldBeFalse();
            sut.CurrentTask.ShouldNotBeNull();
        }

        [Test]
        public async Task StopAndContinueTask_ByDefault_ShouldCreate2Tasks()
        {
            //arrange
            var sut = await CreateTaskListViewModel();
            await StartTask(sut);
            
            //act
            sut.StopTask();
            await sut.ContinueTask();
            await sut.EndTask();

            //assert
            sut.Tasks.Count.ShouldBe(2);
        }

        [Test]
        public async Task EndTask_ByDefault_ShouldEnd()
        {
            //arrange
            var sut = await CreateTaskListViewModel();
            await StartTask(sut);
            var task = sut.Tasks.Single();
            sut.StopTask();

            //act
            await sut.EndTask();

            //assert
            task.End.ShouldNotBeNull();
            sut.IsTaskPaused.ShouldBeFalse();
            sut.IsTaskStarted.ShouldBeFalse();
            DataContext.Tasks.Single().End.HasValue.ShouldBe(true);
        }

        private async Task StartTask(TaskListViewModel taskListViewModel)
        {
            taskListViewModel.TaskName = TaskName;
            await taskListViewModel.StartTask();
        }

        private async Task<TaskListViewModel> CreateTaskListViewModel()
        {
            DataContext = DataContextFixture.CreateTaskDbContext();
            var viewModel = new TaskListViewModel(DataContext);

            await viewModel.Load();

            return viewModel;
        }
    }
}
