using System;
using System.Threading.Tasks;
using System.Net;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using CoreRCON;
using CoreRCON.Parsers.Standard;
using CoreRCON.PacketFormats;
using Prometheum;

namespace Prometheum.Commands {
    public class MinecraftUtils : BaseCommandModule {
        // TODO: Server register command that will register a server address and store it in DB as this Guild's MC server.  Will be a class member that is used in all minecraft server commands.

        [Command("status")]
        public async Task Status(CommandContext context, String ServerAddress) {
            // TODO: Check if address is a url or ip address, and fetch the address of the url if needed before passing in to query.

            MinecraftQueryInfo serverStatus = await ServerQuery.Info(IPAddress.Parse(ServerAddress), 25575, ServerQuery.ServerType.Minecraft) as MinecraftQueryInfo;

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
            embedBuilder.Color = DiscordColor.Green;
            embedBuilder.Title = $"{serverStatus.HostIp} Status";
            embedBuilder.Description = $"{serverStatus.MessageOfTheDay}";
            embedBuilder.AddField("Players Online:", serverStatus.NumPlayers);
            DiscordEmbed embed = embedBuilder.Build();

            await context.Channel.SendMessageAsync(embed);
        }
    }
}