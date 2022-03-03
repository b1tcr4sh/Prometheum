using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace Prometheum.Database {
    public static class DBManager {
        private static IMongoDatabase Database;
        private static string[] expectedCollectionNames = { "MinecraftServers" };
        public static void Init(string url, string databaseName) {

            MongoClient client = new MongoClient(url);
            Database = client.GetDatabase(databaseName);

            Console.WriteLine($"Connected to database {databaseName} : {url}");

            SyncDatabaseCollections(Database).GetAwaiter().GetResult();
        }

        public static async Task CreateDocument<T>(T objectToUpload, String collectionName) {
            IMongoCollection<T> collection = Database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(objectToUpload);
        }
        // public async Task RemoveDocument<T>(string collectionName) {
        //     IMongoCollection<T> collection = Database.GetCollection<T>(collectionName);
        // }
        public static async Task<List<string>> GetCollectionNames() {
            IAsyncCursor<string> collections = await Database.ListCollectionNamesAsync();

            List<String> collectionNames = new List<string>();
            await collections.ForEachAsync(name => {
                collectionNames.Add(name);
            });

            return collectionNames;          
        } 

        private static async Task SyncDatabaseCollections(IMongoDatabase db) {
            IAsyncCursor<string> collections = await db.ListCollectionNamesAsync();
            Console.WriteLine("Performing Collection Synchronization...");

            foreach (string name in expectedCollectionNames) {
                bool isFoundInCollection = false;

                await collections.ForEachAsync(localName => {
                    if (name.Equals(localName)) {
                        isFoundInCollection = true;
                    }
                });

                if (isFoundInCollection == false) {
                    await db.CreateCollectionAsync(name);
                    Console.WriteLine($"Created new collection {name}");
                }
            }
            Console.WriteLine("Done!");
        }
    }
    public enum CollectionNames {
        MinecraftServers
    }
}