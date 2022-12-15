using Newtonsoft.Json;
using PayDayWPF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PayDayWPF.Persistence.Implementation
{
    internal class JsonRepository : IRepository
    {
        const string Path = @"C:\Users\Zer0day\Documents\PayDayWPF.txt";

        public async Task<List<Package>> Load()
        {
            using (var reader = new StreamReader(Path))
            {
                return JsonConvert.DeserializeObject<List<Package>>(await reader.ReadToEndAsync());
            }
        }

        public async Task AddPackage(Package package)
        {
            package.Id = Guid.NewGuid();
            var packages = await Load();
            packages.Add(package);
            await SaveAll(packages);
        }

        public async Task UpdateMeetingsHeld(Guid id, List<DateTime> meetingsHeld)
        {
            var packages = await Load();
            var package = packages.Single(e => e.Id == id);
            package.MeetingsHeld = meetingsHeld;
            await SaveAll(packages);
        }

        public async Task UpdateMeetingsUnheld(Guid id, List<DateTime> meetingsUnheld)
        {
            var packages = await Load();
            var package = packages.Single(e => e.Id == id);
            package.MeetingsUnheld = meetingsUnheld;
            await SaveAll(packages);
        }

        private async Task SaveAll(List<Package> packages)
        {
            using (var streamWriter = new StreamWriter(Path))
            {
                await streamWriter.WriteLineAsync(JsonConvert.SerializeObject(packages, Formatting.Indented));
            }
        }
    }
}
