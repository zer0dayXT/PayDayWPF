using System;
using System.Collections.Generic;

namespace PayDayWPF.Infrastructure
{
    public class Package
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public decimal MeetingProfit { get; set; }
        public int MeetingCount { get; set; }
        public double MeetingsPerWeek { get; set; }
        public List<DateTime> MeetingsHeld { get; set; } = new List<DateTime>();
        public List<DateTime> MeetingsUnheld { get; set; } = new List<DateTime>();
        public DateTime? LastMark { get; set; }
    }
}
