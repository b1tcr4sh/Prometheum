using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Prometheum.Database.MC {
    public class MinecraftServer {
        public ObjectId _id { get; set; }
        public ulong ServerId { get; set; }
        public string Address { get; set; }
        public List<MinecraftDiscordUserPair> Users { get; set; }

    }
}