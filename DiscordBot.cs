using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using Prometheum.Config;
using Prometheum.Commands;
using Prometheum.DB;

namespace Prometheum {
    public class DiscordBot {
        public String token;
        private DiscordClient client;
        private DiscordBotOptions options;

        public DiscordBot() {}
        public DiscordBot(DiscordBotOptions options) {
            this.options = options;
        }
        public async Task ConnectAsync() {

            ConfigFile config = ConfigInit();
            DBManager manager = new DBManager(config.MongoConnectionURL, config.MongoDatabaseName);

            if (config.Token == null) return;

            DiscordConfiguration discordConfig = new DiscordConfiguration {
                    AutoReconnect = true,
                    MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                    TokenType = TokenType.Bot
            };

            if (options.UseDebugToken) {
                discordConfig.Token = config.TesterToken;
            } else {
                discordConfig.Token = config.Token;
            }

            client = new DiscordClient(discordConfig);
            
            CommandsNextExtension commandsNext = client.UseCommandsNext(new CommandsNextConfiguration {
                StringPrefixes = config.Prefixes,
                CaseSensitive = true,
                EnableDms = false
            });
            commandsNext.CommandErrored += HandleCommandError;
            commandsNext.RegisterCommands<MinecraftUtils>();
            commandsNext.RegisterCommands<Utils>();

            await client.ConnectAsync();

            client.GuildDownloadCompleted += ListServersAndChannels;

            await Task.Delay(-1);
        }
        private ConfigFile ConfigInit() {
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
        private async Task HandleCommandError(object sender, CommandErrorEventArgs e) {
            await e.Context.RespondAsync("The command failed to execute with an error, you may need to message your server admin");
            await e.Context.Channel.SendMessageAsync(e.Exception.Message);
        }
    }
}