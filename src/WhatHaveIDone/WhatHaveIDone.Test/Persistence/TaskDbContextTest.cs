using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhatHaveIDone.Test.Persistence
{
    public class TaskDbContextTest
    {
        private static readonly DateTime _from = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime _to = new DateTime(2021, 1, 2, 0, 0, 0, DateTimeKind.Utc);




        [Test]
        [TestCase(-35, -25, 0)]
        [TestCase(-35, -24, 0)]
        [TestCase(-1, 0, 1)]
        [TestCase(-1, -1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(24, 26, 0)]
        [TestCase(25, 26, 0)]
        public async Task GetTasksInIntervalAsync_WithFinishedTasks_ShouldGetCorrectTasks(int startOffset, int endOffset, int expectedResultCount)
        {
            //arrange
            using var sut = DataContextFixture.CreateTaskDbContext();
            var taskWithoutEnding = new Core.Models.TaskModel() { Begin = _from.AddHours(startOffset), End = _to.AddHours(endOffset), Name = "finished Task" };
            sut.Tasks.Add(taskWithoutEnding);
            await sut.SaveChangesAsync();

            //act
            var result = await sut.GetTasksInIntervalAsync(_from, _to);

            //assert
            result.Count.ShouldBe(expectedResultCount);
        }

        [Test]
        [TestCase(-1, 1)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(24, 0)]
        public async Task GetTasksInIntervalAsync_WithUnfinishedFinishedTasks_ShouldGetCorrectTasks(int startOffset, int expectedResultCount)
        {
            //arrange
            using var sut = DataContextFixture.CreateTaskDbContext();
            var taskWithoutEnding = new Core.Models.TaskModel() { Begin = _from.AddHours(startOffset), Name = "unfinished Task" };
            sut.Tasks.Add(taskWithoutEnding);
            await sut.SaveChangesAsync();

            //act
            var result = await sut.GetTasksInIntervalAsync(_from, _to);

            //assert
            result.Count.ShouldBe(expectedResultCount);
        }
        
    }
}
