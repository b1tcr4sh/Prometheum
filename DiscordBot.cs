using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

public class DiscordBot {
    public String token;
    public async Task InitAsync() {
        DiscordClient client = new DiscordClient(new DiscordConfiguration {
            AutoReconnect = true,
            MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
            Token = token,
            TokenType = TokenType.Bot
        });

        await client.ConnectAsync();
        await Task.Delay(-1);
    } 
}