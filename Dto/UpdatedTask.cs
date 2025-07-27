using System;
using System.Collections.Generic;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Dto
{
    public class UpdateSchedule
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public string? Status { get; set; }
    }
}