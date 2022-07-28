using PayDayWPF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayDayWPF.Persistence
{
    public interface IRepository
    {
        public Task<List<Package>> Load();
        public Task AddPackage(Package package);
        public Task UpdateMeetings(Guid id, List<DateTime> meetingsHeld);
    }
}
