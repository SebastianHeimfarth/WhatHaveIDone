using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhatHaveIDone.Core.CoreAbstractions;
using WhatHaveIDone.Core.Models;
using WhatHaveIDone.Core.Persistence;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Test.ViewModel
{
    public class NotificationViewModelTest
    {
        private readonly DateTime _from = new DateTime(2021, 1, 8, 23, 0, 0, DateTimeKind.Utc);
        private readonly DateTime _to = new DateTime(2021, 1, 9, 23, 0, 0, DateTimeKind.Utc);

        [Test]
        public async Task Initialize_EmptyTask_ShouldNotIndicateRunningTask()
        {
            //arrange
            NotificationViewModel sut = CreateNotificationViewModel(new List<TaskModel>());

            //act
            await sut.Initialize();

            //assert
            sut.CurrentTask.ShouldBeNull();
            sut.IsTaskRunning.ShouldBeFalse();
        }

        [Test]
        public async Task Initialize_ClosedTask_ShouldNotIndicateRunningTask()
        {
            //arrange
            NotificationViewModel sut = CreateNotificationViewModel(new List<TaskModel>() { new TaskModel { Begin = _from, End = _to, Name = "FinishedTask" } });

            //act
            await sut.Initialize();

            //assert
            sut.CurrentTask.ShouldBeNull();
            sut.IsTaskRunning.ShouldBeFalse();
        }

        [Test]
        public async Task Initialize_UnfinnishedTask_ShouldIndicateRunningTask()
        {
            //arrange
            NotificationViewModel sut = CreateNotificationViewModel(new List<TaskModel>() { new TaskModel { Begin = _from, Name = "running Task" } });

            //act
            await sut.Initialize();

            //assert
            sut.CurrentTask.ShouldNotBeNull();
            sut.CurrentTask.Name.ShouldBe("running Task");
            sut.IsTaskRunning.ShouldBeTrue();
        }

        [Test]
        public async Task Initialize_UnfinnishedTaskFinishes_ShouldIndicateNotRunningTask()
        {
            //arrange
            var task = new TaskModel { Begin = _from, Name = "running Task" };
            NotificationViewModel sut = CreateNotificationViewModel(new List<TaskModel>() { task });

            //act
            await sut.Initialize();
            task.End = DateTime.UtcNow;
            await sut.Initialize();

            //assert
            sut.CurrentTask.ShouldBeNull();
            sut.IsTaskRunning.ShouldBeFalse();
        }

        private NotificationViewModel CreateNotificationViewModel(IReadOnlyList<TaskModel> taskModels)
        {
            return new NotificationViewModel(FakeDbContext(taskModels), Substitute.For<IDispatcherTimer>());
        }

        private ITaskDbContext FakeDbContext(IReadOnlyList<TaskModel> taskModels)
        {
            var dbContext = Substitute.For<ITaskDbContext>();
            dbContext.GetTasksInIntervalAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(Task.FromResult(taskModels));
            return dbContext;
        }
    }
}