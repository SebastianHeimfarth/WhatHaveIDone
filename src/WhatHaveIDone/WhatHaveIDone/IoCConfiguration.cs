using Microsoft.EntityFrameworkCore;
using MvvmCross;
using MvvmCross.IoC;
using WhatHaveIDone.Core;
using WhatHaveIDone.Core.CoreAbstractions;
using WhatHaveIDone.Core.Persistence;
using WhatHaveIDone.Core.ViewModels;
using WhatHaveIDone.CoreAbstractions;
using WhatHaveIDone.Notification;
using WhatHaveIDone.Persistence;
using WhatHaveIDone.Views;

namespace WhatHaveIDone
{
    internal class IoCConfiguration
    {
        public static void ConfigureDependencies()
        {
            MvxIoCProvider.Initialize();

            Mvx.IoCProvider.RegisterSingleton<ITaskDbContext>(() => CreateDataContext());
            Mvx.IoCProvider.RegisterType<IMessageBoxService, MessageBoxService>();
            Mvx.IoCProvider.RegisterType<IDispatcherTimer, DispatcherTimerWrapper>();
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