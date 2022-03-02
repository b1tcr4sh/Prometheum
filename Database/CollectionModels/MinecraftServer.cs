using System;
using System.Collections.Generic;

namespace Prometheum.Database.MC {
    public class MinecraftServer {
        public string Address { get; set; }
        public List<MinecraftDiscordUserPair> Users { get; set; }

    }
}