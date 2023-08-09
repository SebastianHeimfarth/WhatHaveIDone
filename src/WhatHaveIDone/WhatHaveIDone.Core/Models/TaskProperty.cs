using System.ComponentModel.DataAnnotations;
using System;

namespace WhatHaveIDone.Core.Models
{
    public class TaskProperty
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }

        public TaskProperty Clone() => new TaskProperty { Name = Name, Value = Value, Id = Guid.NewGuid() };
    }
}