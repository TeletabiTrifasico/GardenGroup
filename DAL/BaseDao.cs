using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL;

public class BaseDao
{
    private readonly IMongoDatabase _db;

    protected BaseDao()
    {
        var client = new MongoClient(ConfigurationManager.ConnectionStrings["GardenGroupDB"].ConnectionString);
        _db = client.GetDatabase(ConfigurationManager.ConnectionStrings["Collection"].ConnectionString);
    }

    protected IMongoCollection<T> GetCollection<T>(string name) =>
        _db.GetCollection<T>(name);

    protected FilterDefinition<T> FilterEq<T, TParam>(string field, TParam value) => Builders<T>.Filter.Eq(field, value);
}