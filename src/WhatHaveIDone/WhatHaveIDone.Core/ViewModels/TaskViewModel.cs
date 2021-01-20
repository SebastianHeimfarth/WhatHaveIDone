using MvvmCross.ViewModels;
using System;

namespace WhatHaveIDone.Core.ViewModels
{
    public class TaskViewModel : MvxViewModel
    {
        private string _name;
        private string _comment;
        private DateTime _begin;
        private DateTime? _end;
        private Guid _id;

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
        public DateTime? End
        {
            get => _end;
            set
            {
                SetProperty(ref _end, value);
            }
        }
     
    }
}