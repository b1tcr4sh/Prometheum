using System;
using System.Threading.Tasks;
using Prometheum;
using Prometheum.Config;

namespace Prometheum
{
    static class Program
    {
        private static DiscordBotOptions options;
        static async Task Main(string[] args)
        {
            options = new DiscordBotOptions(); 

            if (args.Length >= 1) {
                ParseArgs(args);
            }

            DiscordBot bot = new DiscordBot(options);

            await bot.ConnectAsync();
        }
        private static void ParseArgs(string[] args) {
            switch (args[0]) {
                case "-d":
                    options.UseDebugToken = true;
                    break;
            }
        }
    }
}
