using Microsoft.EntityFrameworkCore;
using MvvmCross;
using MvvmCross.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using WhatHaveIDone.Core.Persistence;
using WhatHaveIDone.Persistence;

namespace WhatHaveIDone
{
    class IoCConfiguration
    {
        public static void ConfigureDependencies()
        {
            MvxIoCProvider.Initialize();
            
            Mvx.IoCProvider.RegisterSingleton<ITaskDbContext>(() => CreateDataContext());
        }

        private static ITaskDbContext CreateDataContext()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseSqlite("Data Source = Task.db")
                .Options;

            var dataContext = new TaskDbContext(options);

            return dataContext;
        }
    }
}
