using PayDayWPF.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayDayWPF.Persistence
{
    internal interface IRepository
    {
        public Task<List<Package>> Load();
        public Task AddPackage(Package package);
    }
}
