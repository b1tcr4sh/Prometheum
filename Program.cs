using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Prometheum;
using Prometheum.Config;

namespace Prometheum
{
    static class Program
    {
        private static StartupOptions options;
        static async Task<int> Main(string[] args)
        {
            options = new StartupOptions(); 

            if (args.Length >= 1) {
            ArgHandler handler = new ArgHandler();

                options = handler.ParseArgs(args);
            }

            DiscordBot bot = new DiscordBot(options);

            await bot.ConnectAsync();

            return 0;
        }
    }
}
