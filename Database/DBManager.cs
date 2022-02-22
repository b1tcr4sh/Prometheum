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
            "DiscordMinecraftIntegration"
        };
        public DBManager(string url, string databaseName) {
            MongoClient client = new MongoClient(url);
            Database = client.GetDatabase(databaseName);
            
            Console.WriteLine($"Connected to database {databaseName} : {url}");

            SyncDatabaseCollections(Database);
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

        private async void SyncDatabaseCollections(IMongoDatabase db) {
            IAsyncCursor<string> collections = await db.ListCollectionNamesAsync();

            foreach (string name in CollectionNames) {
                bool isFoundInCollection = false;
                await collections.ForEachAsync(localName => {
                    if (name == localName) {
                        isFoundInCollection = true;
                    }
                });

                if (isFoundInCollection) {
                    await db.CreateCollectionAsync(name);
                }
            }
        }
    }
}