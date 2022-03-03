using System;
using System.Collections.Generic;

namespace Prometheum.Database.MC {
    public class MinecraftServer {
        public ulong ServerId { get; set; }
        public string Address { get; set; }
        public List<MinecraftDiscordUserPair> Users { get; set; }

    }
}