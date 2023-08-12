using System.ComponentModel.DataAnnotations;
using System;

namespace WhatHaveIDone.Core.Models
{
    public class TaskPropertyType
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public TaskProperty CreateProperty()
        {
            return new TaskProperty
            {
                TaskPropertyType = this,
                Value = DefaultValue
            };
        }
    }
}