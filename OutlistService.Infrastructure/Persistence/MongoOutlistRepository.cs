using MongoDB.Driver;
using OutlistService.Domain.Entities;
using OutlistService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace OutlistService.Infrastructure.Persistence
{
    public class MongoOutlistRepository : IOutlistRepository
    {
        private readonly IMongoCollection<OutlistProduct> _collection;

        public MongoOutlistRepository(IConfiguration config)
        {
            var client = new MongoClient(config["Mongo:ConnectionString"]);
            var db = client.GetDatabase(config["Mongo:Database"]);
            _collection = db.GetCollection<OutlistProduct>("Outlist");
        }


        public async Task AddAsync(OutlistProduct product)
        {
            await _collection.InsertOneAsync(product);
        } 

        public async Task RemoveAsync(string code)
        {
           await _collection.DeleteOneAsync(p => p.ProductCode == code);
        }

        public async Task UpdateValidityAsync(string code, DateTime from, DateTime to)
        {
            await _collection.UpdateOneAsync(p => p.ProductCode == code,
                Builders<OutlistProduct>.Update
                    .Set(x => x.ValidFrom, from)
                    .Set(x => x.ValidTo, to));
        }

        public async Task<OutlistProduct?> GetByProductCodeAsync(string code)
        {
           return await _collection.Find(p => p.ProductCode == code).FirstOrDefaultAsync();
        }

        public async Task<List<OutlistProduct>> GetPagedAsync(int page, int size)
        {
            return await _collection.Find(_ => true)
           .Skip((page - 1) * size) 
           .Limit(size)
           .ToListAsync();
        }
    }
}
