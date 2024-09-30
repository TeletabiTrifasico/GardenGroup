using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public class MongoCRUD
    {
        private readonly IMongoDatabase _db;

        protected MongoCRUD()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["GardenGroupDB"].ConnectionString);
            _db = client.GetDatabase(ConfigurationManager.ConnectionStrings["Collection"].ConnectionString);
        }

        protected IMongoCollection<T> GetCollection<T>(string name) =>
            _db.GetCollection<T>(name);

        protected void InsertRecord<T>(string table, T record)
        {
            var collection = GetCollection<T>(table);
            collection.InsertOne(record);
        }

        protected void InsertRecords<T>(string table, List<T> records)
        {
            var collection = GetCollection<T>(table);
            collection.InsertMany(records);
        }

        protected void UpdateRecord<T>(string table, Guid id, T record)
        {
            var collection = GetCollection<T>(table);
            collection.ReplaceOne(Builders<T>.Filter.Eq("_id", id), record);
        }

        protected void DeleteRecord<T>(string table, Guid id)
        {
            var collection = GetCollection<T>(table);
            collection.DeleteOne(Builders<T>.Filter.Eq("_id", id));
        }

       

    }
}
