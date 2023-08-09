using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;

namespace WhatHaveIDone.Core.Models
{
    public class TaskModel
    {
        private TaskType _taskType;

        [Key]
        public Guid Id { get; set; }

        //public int TaskTypeId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Comment { get; set; }
        public TaskType TaskType
        {
            get => _taskType;
            set
            {
                if (value != null && value != _taskType && value.DefaultProperties != null)
                {
                    DynamicPropertyValues = value.DefaultProperties.
                        Select(x => x.Clone()).ToList();
                }
                _taskType = value;
            }
        }

        public List<TaskProperty> DynamicPropertyValues { get; set; } = new();
        public DateTime Begin { get; set; }
        public DateTime? End { get; set; }
    }
}