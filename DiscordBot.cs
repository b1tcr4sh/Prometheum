using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using Prometheum.Config;

public class DiscordBot {
    public String token;
    private DiscordClient client;
    public async Task ConnectAsync() {
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
    public async Task SendMessageInEveryServer() {
        IReadOnlyDictionary<ulong, DiscordGuild> servers = client.Guilds;

        foreach (KeyValuePair<ulong, DiscordGuild> serverKeyPair in servers) {
            Console.WriteLine("Server {0}:", serverKeyPair.Value.Name);

            IReadOnlyList<DiscordChannel> channels = await serverKeyPair.Value.GetChannelsAsync();
            foreach (DiscordChannel channel in channels) {
                Console.WriteLine(" " + channel.Name);
            }
        }
    } 
}