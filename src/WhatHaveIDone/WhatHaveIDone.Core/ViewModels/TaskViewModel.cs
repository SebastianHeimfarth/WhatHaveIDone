using MvvmCross.ViewModels;
using System;
using WhatHaveIDone.Core.Models;

namespace WhatHaveIDone.Core.ViewModels
{
    public class TaskViewModel : MvxViewModel
    {
        private DateTime _begin;
        private TaskCategory _category;
        private string _comment;
        private TimeSpan? _duration;
        private DateTime? _end;
        private Guid _id;
        private string _name;
        private DateTime _temporaryEnd = DateTime.UtcNow;

        public DateTime Begin
        {
            get => _begin;
            set
            {
                SetProperty(ref _begin, value);
                SetDuration();
            }
        }

        public TaskCategory Category
        {
            get => _category;
            set
            {
                SetProperty(ref _category, value);
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

        public TimeSpan? Duration
        {
            get => _duration;
            set
            {
                SetProperty(ref _duration, value);
            }
        }

        public DateTime? End
        {
            get => _end;
            set
            {
                SetProperty(ref _end, value);
                SetDuration();
            }
        }

        public Guid Id
        {
            get => _id;
            set
            {
                SetProperty(ref _id, value);
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

        public DateTime TemporaryEnd
        {
            get => _temporaryEnd;
            set
            {
                SetProperty(ref _temporaryEnd, value);
            }
        }

        private void SetDuration()
        {
            Duration = End.HasValue ? End - Begin : null;
        }
    }
}