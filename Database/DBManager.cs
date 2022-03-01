using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace Prometheum.Database {
    public class DBManager {
        private IMongoDatabase Database;
        public String[] CollectionNames = new String[] {
            "MinecraftServers"
        };
        public DBManager(string url, string databaseName) {

            try {
                MongoClient client = new MongoClient(url);
                Database = client.GetDatabase(databaseName);
            } catch (TimeoutException exception) {
                Console.WriteLine("Failed to connect to {0}: {1}", url, exception.Message);
            }

            Console.WriteLine($"Connected to database {databaseName} : {url}");

            SyncDatabaseCollections(Database).GetAwaiter().GetResult();
        }

        public async Task CreateDocument<T>(T objectToUpload, String collectionName) {
            IMongoCollection<T> collection = Database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(objectToUpload);
        }
        // public async Task RemoveDocument<T>(string collectionName) {
        //     IMongoCollection<T> collection = Database.GetCollection<T>(collectionName);
        // }
        public async Task<List<string>> GetCollectionNames() {
            IAsyncCursor<string> collections = await Database.ListCollectionNamesAsync();

            List<String> collectionNames = new List<string>();
            await collections.ForEachAsync(name => {
                collectionNames.Add(name);
            });

            return collectionNames;          
        } 

        private async Task SyncDatabaseCollections(IMongoDatabase db) {
            IAsyncCursor<string> collections = await db.ListCollectionNamesAsync();
            Console.WriteLine("Performing Collection Synchronization...");

            foreach (string name in CollectionNames) {
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
}