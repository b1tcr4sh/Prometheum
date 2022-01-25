using System;
using System.Threading.Tasks;
using Prometheum;
using Prometheum.Config;

namespace Prometheum
{
    class Program
    {
        private static string token;
        static async Task Main(string[] args)
        {
            ConfigManager manager = new ConfigManager("./config.json");

            DiscordBot bot = new DiscordBot();
        }
        
    }
}
