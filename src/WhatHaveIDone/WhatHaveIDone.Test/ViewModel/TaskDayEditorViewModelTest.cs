using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using WhatHaveIDone.Core.ViewModels;
using WhatHaveIDone.Persistence;
using WhatHaveIDone.Test.Persistence;

namespace WhatHaveIDone.Test.ViewModel
{
    public class TaskDayEditorViewModelTest
    {
        private const string TaskName = "TaskName";
        private readonly DateTime _dayInLocalTime = new DateTime(2021, 1, 1);

        private TaskDbContext DataContext { get; set; }

        [Test]
        public async Task When_TaskIsAdded_ShouldUpdateTaskStatistics()
        {
            //arrange
            var sut = await CreateTaskListViewModel();
            var category1 = new Core.Models.TaskCategory();
            var category2 = new Core.Models.TaskCategory();

            //act
            sut.Tasks.Add(new TaskViewModel() { Category = category1, Name = "t1", Begin = _dayInLocalTime, End = _dayInLocalTime.AddHours(1) });
            sut.Tasks.Add(new TaskViewModel() { Category = category1, Name = "t2", Begin = _dayInLocalTime, End = _dayInLocalTime.AddHours(2) });
            sut.Tasks.Add(new TaskViewModel() { Category = category2, Name = "t3", Begin = _dayInLocalTime, End = _dayInLocalTime.AddHours(4) });

            //assert
            sut.TaskStatistics.ShouldNotBeNull();
            sut.TaskStatistics.Single(x => x.Category == category1).TotalMinutes.ShouldBe((1 + 2) * 60);
            sut.TaskStatistics.Single(x => x.Category == category2).TotalMinutes.ShouldBe(4 * 60);
        }

        [Test]
        public async Task ChangeDay_ByDefault_ShouldReloadTasks()
        {
            //arrange
            var sut = await CreateTaskListViewModel();
            await DataContext.CreateTaskAsync("day1", "comment", _dayInLocalTime.ToUniversalTime());
            await DataContext.CreateTaskAsync("day2", "comment", _dayInLocalTime.AddDays(1).ToUniversalTime());

            //act
            await sut.ChangeDay(_dayInLocalTime.AddDays(1));

            //assert
            sut.Tasks.ShouldContain(x => x.Name == "day2");
        }

        [Test]
        public async Task Load_TasksAreAvailable_ShouldLoadAllTaskForTheDay()
        {
            //arrange
            var sut = await CreateTaskListViewModel();
            await DataContext.CreateTaskAsync("task1", "comment", _dayInLocalTime.ToUniversalTime());
            await DataContext.CreateTaskAsync("task2", "comment", _dayInLocalTime.ToUniversalTime());
            await DataContext.CreateTaskAsync("task3", "comment", _dayInLocalTime.ToUniversalTime());
            await DataContext.CreateTaskAsync("taskForOtherDay", "comment", _dayInLocalTime.AddDays(1));

            //act
            await sut.Load();

            //assert
            sut.Tasks.Count().ShouldBe(3);
        }

        [Test]
        public async Task Load_UnfinishedTaskIsAvailable_CurrentTaskShouldBeUnfinishedTask()
        {
            //arrange
            var sut = await CreateTaskListViewModel();

            var unfinishedTask = await DataContext.CreateTaskAsync("unfinishedTask", "comment", _dayInLocalTime.ToUniversalTime());
            var finishedTask = await DataContext.CreateTaskAsync("finishedTask", "comment", _dayInLocalTime.ToUniversalTime());

            finishedTask.End = finishedTask.Begin.AddHours(1);

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

        private async Task StartTask(TaskDayEditorViewModel taskListViewModel)
        {
            taskListViewModel.TaskName = TaskName;
            await taskListViewModel.StartTask();
        }

        private async Task<TaskDayEditorViewModel> CreateTaskListViewModel()
        {
            DataContext = DataContextFixture.CreateTaskDbContext();
            var viewModel = new TaskDayEditorViewModel(DataContext);
            await viewModel.ChangeDay(_dayInLocalTime);

            await viewModel.Load();

            return viewModel;
        }
    }
}