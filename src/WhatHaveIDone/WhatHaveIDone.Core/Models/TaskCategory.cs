using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace WhatHaveIDone.Core.Models
{
    public class TaskCategory
    {
        [Key]
        public Guid Id { get; set; }

        public Color Color { get; set; }

        public string Name { get; set; }
    }
}
