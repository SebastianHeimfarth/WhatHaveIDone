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

        [Required]
        public string Name { get; set; }

        public string Comment { get; set; }
        public virtual TaskType TaskType
        {
            get => _taskType;
            set
            {
                if (value != null && value != _taskType && value.DefaultProperties != null)
                {
                    Properties = value.DefaultProperties.
                        Select(x => x.CreateProperty()).ToList();
                }
                _taskType = value;
            }
        }

        public virtual List<TaskProperty> Properties { get; set; } = new();
        public DateTime Begin { get; set; }
        public DateTime? End { get; set; }
    }
}