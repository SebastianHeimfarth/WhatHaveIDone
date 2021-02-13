using MvvmCross.ViewModels;
using System;
using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.ViewModels
{
    public class TaskStatisticViewModel : MvxViewModel
    {
        private TaskCategory _category;
        private TimeSpan _timeSpan;

        public TaskCategory Category
        {
            get => _category;
            set
            {
                SetProperty(ref _category, value);
            }
        }

        public TimeSpan TimeSpan
        {
            get => _timeSpan;
            set
            {
                SetProperty(ref _timeSpan, value);
            }
        }
    }
}