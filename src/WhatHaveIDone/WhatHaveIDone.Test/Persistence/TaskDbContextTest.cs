using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhatHaveIDone.Core.Models;
using WhatHaveIDone.Persistence;

namespace WhatHaveIDone.Test.Persistence
{
    public class TaskDbContextTest
    {
        private static readonly DateTime _from = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime _to = new DateTime(2021, 1, 2, 0, 0, 0, DateTimeKind.Utc);


        [Test]
        public async Task CRUD_RoundTripTest()
        {
            var dbName = $"crudTest_{Guid.NewGuid()}.db";
            var sut = DataContextFixture.CreateSQLiteContext(dbName);

            //Create
            var taskType = new TaskType { Id = 1, Name = "TestTaskType" };
            var taskPropertyType1 = new TaskPropertyType { Name = "TestPropertyType1", DefaultValue = "V1" };
            var taskPropertyType2 = new TaskPropertyType { Name = "TestPropertyType2", DefaultValue = "V2" };
            taskType.DefaultProperties = new List<TaskPropertyType> { taskPropertyType1, taskPropertyType2 };

            var task = new TaskModel()
            {
                Begin = DateTime.UtcNow,
                Name = "TestTask",
                TaskType = taskType,
                Properties = new List<TaskProperty>
                {
                    new TaskProperty { TaskPropertyType = taskPropertyType1, Value = "A" },
                    new TaskProperty { TaskPropertyType = taskPropertyType2, Value = "B" }
                }
            };



            sut.Tasks.Add(task);
            await sut.SaveChangesAsync();
            await sut.DisposeAsync();

            //Read
            sut = DataContextFixture.CreateSQLiteContext(dbName);
            var result = await sut.Tasks.FindAsync(task.Id);
            result.ShouldNotBeNull();
            result.Name.ShouldBe(task.Name);
            result.TaskType.Name.ShouldBe(taskType.Name);
            result.TaskType.DefaultProperties.ShouldContain(x => x.Name == taskPropertyType1.Name && x.DefaultValue == taskPropertyType1.DefaultValue);
            result.Properties.ShouldContain(x => x.Name == taskPropertyType1.Name && x.Value == "A");
            result.Properties.ShouldContain(x => x.Name == taskPropertyType2.Name && x.Value == "B");


            //Update
            result.Name = "UpdatedName";
            result.Properties[0].Value = "UpdatedValue";
            await sut.SaveChangesAsync();
            await sut.DisposeAsync();

            //Read
            sut = DataContextFixture.CreateSQLiteContext(dbName);
            var updatedResult = await sut.Tasks.FindAsync(task.Id);
            updatedResult.ShouldNotBeNull();
            updatedResult.Name.ShouldBe("UpdatedName");
            updatedResult.Properties.ShouldContain(x => x.Name == taskPropertyType1.Name && x.Value == "UpdatedValue");

            //Delete
            sut.Tasks.Remove(updatedResult);
            await sut.SaveChangesAsync();
            await sut.DisposeAsync();

            //Read
            sut = DataContextFixture.CreateSQLiteContext(dbName);
            var deletedResult = await sut.Tasks.FindAsync(task.Id);
            deletedResult.ShouldBeNull();
        }




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
            using var sut = DataContextFixture.CreateInMemoryDb();
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
            using var sut = DataContextFixture.CreateInMemoryDb();
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