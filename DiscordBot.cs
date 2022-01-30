using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using Prometheum.Config;

namespace Prometheum {
    public class DiscordBot {
        public String token;
        private DiscordClient client;
        public async Task ConnectAsync() {

            Config config = ConfigInit();

            if (config.Token == null) return;

            client = new DiscordClient(new DiscordConfiguration {
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                Token = token,
                TokenType = TokenType.Bot
            });

            await client.ConnectAsync();

            client.GuildDownloadCompleted += ListServersAndChannels;

            await Task.Delay(-1);
        }
        private Config ConfigInit() {
            ConfigManager manager = new ConfigManager("./config.json");
            token = manager.Config.Token;

            return manager.Config;
        }
        public async Task ListServersAndChannels(object sender, GuildDownloadCompletedEventArgs e) {
            foreach (KeyValuePair<ulong, DiscordGuild> serverKeyPair in client.Guilds) {
                Console.WriteLine("Server {0}:", serverKeyPair.Value.Name);

                IReadOnlyList<DiscordChannel> channels = await serverKeyPair.Value.GetChannelsAsync();
                foreach (DiscordChannel channel in channels) {
                    Console.WriteLine("     " + channel.Name);
                }
            }
        }
    }
}