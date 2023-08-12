using Microsoft.EntityFrameworkCore;
using System;
using WhatHaveIDone.Core.Persistence;

namespace WhatHaveIDone.Test.Persistence
{
    public class DataContextFixture
    {
        public static TaskDbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TaskDbContext(options);
        }

        public static TaskDbContext CreateSQLiteContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseSqlite($"Data Source = {dbName}")
                .Options;

            return new TaskDbContext(options);
        }

    }
}