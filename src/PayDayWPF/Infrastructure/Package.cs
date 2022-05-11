using System;
using System.Collections.Generic;

namespace PayDayWPF.Infrastructure
{
    internal class Package
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public decimal MeetingProfit { get; set; }
        public int MeetingCount { get; set; }
        public List<DateTime> MeetingsHeld { get; set; } = new List<DateTime>();
    }
}
