using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using Prometheum.Database.MC;

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

        public static async Task CreateDocumentAsync<T>(T objectToUpload, String collectionName) {
            IMongoCollection<T> collection = Database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(objectToUpload);
        }
        public static void UpdateMinecraftServerDocument<MinecraftServer>(Expression<Func<MinecraftServer, bool>> filter, string serverAddress) {
            IMongoCollection<MinecraftServer> collection = Database.GetCollection<MinecraftServer>("MinecraftServers");
            UpdateDefinition<MinecraftServer> update = Builders<MinecraftServer>.Update.Set("Address", serverAddress);

            collection.UpdateOne<MinecraftServer>(filter, update);
        }
        public static MinecraftServer GetMinecraftServerDocument(ulong serverId) {
            IMongoCollection<MinecraftServer> collection = Database.GetCollection<MinecraftServer>("MinecraftServers");
            IFindFluent<MinecraftServer, MinecraftServer> finder = collection.Find<MinecraftServer>(document => document.ServerId == serverId);
            
            if (finder.CountDocuments() > 1) {
                return null;
            } else if (finder.CountDocuments() == 0) {
                return null;
            } else {
                return finder.First<MinecraftServer>();
            }
        }
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