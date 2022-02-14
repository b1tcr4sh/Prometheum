using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Prometheum.Database {
    public class DBManager {
        public DBManager(string url, string databaseName) {
            MongoClient client = new MongoClient(new MongoUrl(url));
            IMongoDatabase db = client.GetDatabase(databaseName);

            Console.WriteLine($"Connected to ${databaseName} : ${url}");
        }
        private async void CreateTestCollection(IMongoDatabase db) {
            await db.CreateCollectionAsync("Test");
        }
    }
}