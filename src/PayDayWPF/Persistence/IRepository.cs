using PayDayWPF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayDayWPF.Persistence
{
    internal interface IRepository
    {
        public Task<List<Package>> Load();
        public Task AddPackage(Package package);
        public Task UpdateMeetings(Guid id, List<DateTime> meetingsHeld);
    }
}
