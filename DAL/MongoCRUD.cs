using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MongoCRUD
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _db;

        public MongoCRUD()
        {
            _client = new MongoClient(ConfigurationManager.ConnectionStrings["GardenGroupDB"].ConnectionString);
            _db = _client.GetDatabase(ConfigurationManager.ConnectionStrings["Collection"].ConnectionString);
        }
    }
}
