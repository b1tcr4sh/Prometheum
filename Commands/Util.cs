using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Prometheum.Commands
{
    public class Utils : BaseCommandModule {

        [Command("info")]
        public async Task Info(CommandContext context) {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            DiscordEmbed embed = builder.WithUrl("https://github.com/vividuwu/prometheum")
            .WithTitle("Prometheum Project 🔮")
            .WithColor(DiscordColor.HotPink)
            .AddField("Liscense:", "Apache 2.0")
            .AddField("Current Uptime", (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString())
            .Build();

            await context.Channel.SendMessageAsync(embed);
        } 
    }
}