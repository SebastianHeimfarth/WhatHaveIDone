using MvvmCross.ViewModels;

namespace WhatHaveIDone.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterCustomAppStart<AppStart>();
        }
    }
}