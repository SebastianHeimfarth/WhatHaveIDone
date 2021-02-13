using Microsoft.EntityFrameworkCore;
using System;
using WhatHaveIDone.Persistence;

namespace WhatHaveIDone.Test.Persistence
{
    public class DataContextFixture
    {
        public static TaskDbContext CreateTaskDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TaskDbContext(options);
        }
    }
}