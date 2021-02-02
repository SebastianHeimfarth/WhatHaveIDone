using Microsoft.EntityFrameworkCore;
using MvvmCross;
using MvvmCross.IoC;
using WhatHaveIDone.Core;
using WhatHaveIDone.Core.Persistence;
using WhatHaveIDone.Persistence;

namespace WhatHaveIDone
{
    internal class IoCConfiguration
    {
        public static void ConfigureDependencies()
        {
            MvxIoCProvider.Initialize();

            Mvx.IoCProvider.RegisterSingleton<ITaskDbContext>(() => CreateDataContext());
            Mvx.IoCProvider.RegisterType<IMessageBoxService, MessageBoxService>();
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