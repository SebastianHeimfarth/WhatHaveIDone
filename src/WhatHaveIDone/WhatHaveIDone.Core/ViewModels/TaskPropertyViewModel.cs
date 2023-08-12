using MvvmCross.ViewModels;
using System;
using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.ViewModels
{
    public class TaskPropertyViewModel : MvxViewModel
    {
        private string _value;
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
                _value = value;
            }
        }

        public TaskPropertyType TaskPropertyType { get; init; }
    }
}
