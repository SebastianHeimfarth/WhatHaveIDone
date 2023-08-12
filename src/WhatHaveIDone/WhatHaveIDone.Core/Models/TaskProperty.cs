using System.ComponentModel.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace WhatHaveIDone.Core.Models
{
    public class TaskProperty
    {
        [Key]
        public Guid Id { get; set; }

        public string Name => TaskPropertyType?.Name;
        public string Value { get; set; }
        public virtual TaskPropertyType TaskPropertyType { get; set; }
    }
}