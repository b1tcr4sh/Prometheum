using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using CoreRCON;
using CoreRCON.Parsers.Standard;
using CoreRCON.PacketFormats;
using Prometheum.Database;
using Prometheum.Database.MC;

namespace Prometheum.Commands {
    [Group("minecraft")]
    public class MinecraftUtils : BaseCommandModule {

        [Command("register")]
        [Description("Registers a Minecraft server with this Discord server.")]
        public async Task RegisterServer(CommandContext context, string ServerAddress) {
            MinecraftServer server = new MinecraftServer() { Address = ServerAddress, Users = new List<MinecraftDiscordUserPair>(), ServerId = context.Guild.Id};

            if (DBManager.GetMinecraftServerDocument(server.ServerId) == null) {
                await DBManager.CreateDocumentAsync<MinecraftServer>(server, "MinecraftServers");
            } else if (DBManager.GetMinecraftServerDocument(server.ServerId) != server) {
                DBManager.UpdateMinecraftServerDocument((MinecraftServer serverToCompare) => serverToCompare.ServerId == server.ServerId, ServerAddress);
            }

            await context.Channel.SendMessageAsync($"Added server {ServerAddress} to the database");
        }

        // TODO: Server register command that will register a server address and store it in DB as this Guild's MC server.  Will be a class member that is used in all minecraft server commands.

        [Command("status")]
        [Description("Shows a status of the Discord Server's registered minecraft server.")]
        public async Task Status(CommandContext context) {
            // TODO: Check if address is a url or ip address, and fetch the address of the url if needed before passing in to query.
            MinecraftServer server = DBManager.GetMinecraftServerDocument(context.Guild.Id);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
            if (server == null) {
                embedBuilder.WithColor(DiscordColor.Red);
                embedBuilder.WithTitle("This Discord server does not have an associated Minecraft server");
                embedBuilder.WithFooter("Use the <minecraft register> command to register one!");
                await context.Channel.SendMessageAsync(embedBuilder.Build());

                return;
            }

            MinecraftQueryInfo serverStatus = await ServerQuery.Info(IPAddress.Parse(server.Address), 25575, ServerQuery.ServerType.Minecraft) as MinecraftQueryInfo;

            embedBuilder.Color = DiscordColor.Green;
            embedBuilder.Title = $"{serverStatus.HostIp} Status";
            embedBuilder.Description = $"{serverStatus.MessageOfTheDay}";
            embedBuilder.AddField("Players Online:", serverStatus.NumPlayers);
            DiscordEmbed embed = embedBuilder.Build();

            await context.Channel.SendMessageAsync(embed);
        }
    }
}