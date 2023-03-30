using MongoDB.Bson;
using MongoDB.Driver;
using PayDayWPF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayDayWPF.Persistence.Implementation
{
    internal class MongoDBRepository : IRepository
    {
        private string _password;

        public async Task AddPackage(Package package)
        {
            var mongoClient = new MongoClient($"mongodb://mo10097_payday:{_password}@mongo0.mydevil.net:27017/mo10097_payday");
            var database = mongoClient.GetDatabase("mo10097_payday");
            var collection = database.GetCollection<BsonDocument>("payday");
            await collection.InsertOneAsync(package.ToBsonDocument());
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
