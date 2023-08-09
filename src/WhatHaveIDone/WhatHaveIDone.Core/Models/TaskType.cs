using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;
using WhatHaveIDone.Core.Json;

namespace WhatHaveIDone.Core.Models
{
    public class TaskType
    {
        [Key]
        public int Id { get; set; }

        [JsonConverter(typeof(HexStringColorConverter))]
        public Color Color { get; set; }

        public string Name { get; set; }

        public List<TaskProperty> DefaultProperties { get; set; }
    }
}