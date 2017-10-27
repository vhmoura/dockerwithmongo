using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AspCoreWithDocker
{
    [BsonIgnoreExtraElements]
    public class TodoModel
    {
        public ObjectId Id { get; set; }
        public string text { get; set; }
        public DateTime? completedAt { get; set; }
        public bool completed { get; set; }
    }

    public interface IMongoDataFactory {
        List<TodoModel> getData();
    }

    public class MongoDataFactory: IMongoDataFactory
    {
        readonly string connString = "mongodb://{0}:{1}@ds011715.mlab.com:11715/todoapp";
        protected IMongoClient _client;
        protected static IMongoDatabase _database;

        public MongoDataFactory()
        {
            var pass = CryptoHelp.Decrypt(Properties.Resources.ResourceManager.GetString("MongoPass"));
            var user = CryptoHelp.Decrypt(Properties.Resources.ResourceManager.GetString("MongoUser"));

            var connection = string.Format(connString, user, pass);
            _client = new MongoClient(connection);            
            _database = _client.GetDatabase("todoapp");
        }

        public  List<TodoModel> getData()
        {
            var filter = Builders<TodoModel>.Filter.Where(_=> true);

            var data = _database.GetCollection<TodoModel>("todos").Find(filter).ToList();            

            return data;
        }
    }
}
