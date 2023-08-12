using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.ViewModels
{
    public class TaskViewModel : MvxViewModel
    {
        private string _name;
        private TaskType _taskType;
        private string _comment;
        private DateTime _begin;
        private DateTime _temporaryEnd = DateTime.UtcNow;
        private DateTime? _end;
        private Guid _id;
        private ObservableCollection<TaskPropertyViewModel>  _dynamicPropertyValues;

        public Guid Id
        {
            get => _id;
            set
            {
                SetProperty(ref _id, value);
            }
        }

        public TaskType TaskType
        {
            get => _taskType;
            set
            {
                if (_taskType != value)
                {
                    DynamicPropertyValues = new ObservableCollection<TaskPropertyViewModel>(
                        value.DefaultProperties.Select(x=> 
                        ViewModelMapper.CreateTaskPropertyViewModel(x)));
                }
                SetProperty(ref _taskType, value);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                SetProperty(ref _comment, value);
            }
        }

        public DateTime Begin
        {
            get => _begin;
            set
            {
                SetProperty(ref _begin, value);
            }
        }

        public DateTime TemporaryEnd
        {
            get => _temporaryEnd;
            set
            {
                SetProperty(ref _temporaryEnd, value);
            }
        }

        public DateTime? End
        {
            get => _end;
            set
            {
                SetProperty(ref _end, value);
            }
        }

        public ObservableCollection<TaskPropertyViewModel> DynamicPropertyValues
        {
            get=> _dynamicPropertyValues;
            set
            {
                SetProperty(ref _dynamicPropertyValues, value);
            }
        }
    }
}