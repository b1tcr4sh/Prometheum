using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using Prometheum.Config;

public class DiscordBot {
    public String token;
    private DiscordClient client;
    public async Task ConnectAsync() {
        // client.GuildDownloadCompleted += ListServersAndChannels;

        ConfigInit();

        client = new DiscordClient(new DiscordConfiguration {
            AutoReconnect = true,
            MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
            Token = token,
            TokenType = TokenType.Bot
        });

        await client.ConnectAsync();

        await Task.Delay(-1);
    }
    private void ConfigInit() {
        ConfigManager manager = new ConfigManager("./config.json");
        token = manager.Config.Token;
    }
    public async Task ListServersAndChannels(object sender, GuildDownloadCompletedEventArgs e) {
        foreach (KeyValuePair<ulong, DiscordGuild> serverKeyPair in client.Guilds) {
            Console.WriteLine("Server {0}:", serverKeyPair.Value.Name);

            IReadOnlyList<DiscordChannel> channels = await serverKeyPair.Value.GetChannelsAsync();
            foreach (DiscordChannel channel in channels) {
                Console.WriteLine(" " + channel.Name);
            }
        }
    }
}