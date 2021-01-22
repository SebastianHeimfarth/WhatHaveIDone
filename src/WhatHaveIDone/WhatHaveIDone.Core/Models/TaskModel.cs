using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WhatHaveIDone.Core.Models
{

    public class TaskModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Comment { get; set; }
        public virtual TaskCategory Category { get; set; }
        public DateTime Begin { get; set; }
        public DateTime? End { get; set; }
    }
}
