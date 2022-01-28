using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using Prometheum.Config;

using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

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
        List<ServerChannel> servers = new List<ServerChannel>();

        foreach (KeyValuePair<ulong, DiscordGuild> serverKeyPair in client.Guilds) {
            Console.WriteLine("Server {0}:", serverKeyPair.Value.Name);

            IReadOnlyList<DiscordChannel> channels = await serverKeyPair.Value.GetChannelsAsync();

            
            String[] channelNames = new String[channels.Count];

            int i = 0;
            foreach (DiscordChannel channel in channels) {
                Console.WriteLine("     " + channel.Name);
                channelNames[i] = channel.Name;
                i++;
            }

            servers.Add(new ServerChannel {
                ServerName = serverKeyPair.Value.Name,
                ChannelNames = channelNames
            });
        }

        string serializedJson = JsonSerializer.Serialize<List<ServerChannel>>(servers);

        File.WriteAllText(@"./Servers.json", serializedJson);
    }
}