using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Core
{
    public class AppStart : MvxAppStart
    {
        private readonly ITaskSetup _taskSetup;

        public AppStart(IMvxApplication app,
                IMvxNavigationService navigationService, ITaskSetup taskSetup) : base(app, navigationService)
        {
            _taskSetup = taskSetup;
        }

        protected override async Task<object> ApplicationStartup(object hint = null)
        {
            await _taskSetup.AddTaskTypesToDatabase();
            return await base.ApplicationStartup(hint);
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            return NavigationService.Navigate<TaskDayEditorViewModel, DateTime>(DateTime.Today);
        }
    }
}