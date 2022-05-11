using Newtonsoft.Json;
using PayDayWPF.Infrastructure;
using System.Collections.Generic;
using System.IO;
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
            var packages = await Load();
            packages.Add(package);
            using (var streamWriter = new StreamWriter(Path))
            {
                streamWriter.WriteLine(JsonConvert.SerializeObject(packages, Formatting.Indented));
            }
        }
    }
}
