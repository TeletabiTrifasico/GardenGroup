using MongoDB.Driver;
using System.Configuration;

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