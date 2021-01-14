using System;

namespace WhatHaveIDone.Core.Models
{
    public class TaskModel
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
    }
}