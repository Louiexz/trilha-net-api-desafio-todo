using System;
using System.Collections.Generic;

namespace TrilhaApiDesafio.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime UpdatedAt { get; set; }
        public EnumScheduleStatus Status { get; set; }
    }
}