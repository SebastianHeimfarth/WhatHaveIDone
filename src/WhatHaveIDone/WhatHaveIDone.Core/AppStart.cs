using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Core
{
    public class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication app,
                IMvxNavigationService navigationService) : base(app, navigationService)
        {
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            return NavigationService.Navigate<TaskDayEditorViewModel, DateTime>(DateTime.Today);
        }
    }
}