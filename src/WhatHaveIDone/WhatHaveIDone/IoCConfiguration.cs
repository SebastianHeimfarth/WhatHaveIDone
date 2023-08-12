using Microsoft.EntityFrameworkCore;
using MvvmCross;
using MvvmCross.IoC;
using System.Linq;
using WhatHaveIDone.Core;
using WhatHaveIDone.Core.Configuration;
using WhatHaveIDone.Core.CoreAbstractions;
using WhatHaveIDone.Core.Persistence;
using WhatHaveIDone.Core.ViewModels;
using WhatHaveIDone.CoreAbstractions;
using WhatHaveIDone.Notification;
using WhatHaveIDone.Views;

namespace WhatHaveIDone
{
    internal class IoCConfiguration
    {
        public static void ConfigureDependencies()
        {
            MvxIoCProvider.Initialize();

            var relevantAssemblies = new[] { typeof(IoCConfiguration).Assembly, typeof(TaskViewModel).Assembly };
            var creatableTypes = relevantAssemblies.SelectMany(x=>x.GetTypes()).Where(x => !x.IsGenericType && !x.IsAbstract && !x.IsInterface);
            creatableTypes.AsInterfaces().RegisterAsDynamic();

            Mvx.IoCProvider.RegisterSingleton<ITaskDbContext>(() => CreateDataContext());
            Mvx.IoCProvider.RegisterType<NotificationPopupView, NotificationPopupView>();
            Mvx.IoCProvider.RegisterType<NotificationViewModel, NotificationViewModel>();
            
            var notificationViewModel = Mvx.IoCProvider.Resolve<NotificationViewModel>();
            var notificationView = Mvx.IoCProvider.Resolve<NotificationPopupView>();
            notificationView.DataContext = notificationViewModel;

            Mvx.IoCProvider.RegisterSingleton(new NotificationService(notificationView));
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