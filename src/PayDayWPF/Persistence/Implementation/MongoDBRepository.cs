using PayDayWPF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayDayWPF.Persistence.Implementation
{
    internal class MongoDBRepository : IRepository
    {
        private string _password;

        public Task AddPackage(Package package)
        {
            throw new NotImplementedException();
        }

        public Task<List<Package>> Load()
        {
            throw new NotImplementedException();
        }

        public Task UpdateMeetingsHeld(Guid id, List<DateTime> meetingsHeld)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMeetingsUnheld(Guid id, List<DateTime> meetingsHeld)
        {
            throw new NotImplementedException();
        }

        public void SetPassword(string password)
        {
            _password = password;
        }
    }
}
