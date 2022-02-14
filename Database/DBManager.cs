using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Prometheum.Database {
    public class DBManager {
        public DBManager(string url, string databaseName) {
            MongoClient client = new MongoClient(url);
            IMongoDatabase db = client.GetDatabase(databaseName);
            
            Console.WriteLine($"Connected to database {databaseName} : {url}");
        }
    }
}