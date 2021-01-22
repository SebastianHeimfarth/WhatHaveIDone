using MvvmCross.ViewModels;
using WhatHaveIDone.Core.ViewModels;

namespace WhatHaveIDone.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<TaskDayEditorViewModel>();
        }
    }
}