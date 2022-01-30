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
    class MinecraftUtils : BaseCommandModule {
        [Command("status")]
        public async Task Status(CommandContext context, String ServerAddress) {
            //  RCON rconClient = new RCON(IPAddress.Parse(ServerAddress), 25575, RconPassword);

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