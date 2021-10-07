using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;
using Freya.Bot.Services;

namespace Freya.Bot.Commands
{
    public class AudioModule : BaseCommandModule
    {
        private readonly MusicCluster _musicCluster;

        public AudioModule(MusicCluster musicCluster)
        {
            _musicCluster = musicCluster;
        }
        
        [Command]
        public async Task Join(CommandContext ctx)
        {
            var lava = ctx.Client.GetLavalink();

            if (!lava.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("The Lavalink connection is not established");
                return;
            }
            
            var channel = ctx.Member.VoiceState?.Channel;

            if (channel == null)
            {
                await ctx.RespondAsync("Join voice channel first.");
                return;
            }
            
            var player = await _musicCluster.GetOrRegisterPlayer(ctx.Guild.Id);
            var lavaNode = lava.ConnectedNodes.Values.First();
                
            await player.Connect(lavaNode, channel);
            await ctx.RespondAsync($"Joined {channel.Name}!");
        }

        [Command]
        public async Task Leave(CommandContext ctx)
        {
            var player = await _musicCluster.GetOrRegisterPlayer(ctx.Guild.Id);
            await player.Disconnect();
            await _musicCluster.UnregisterPlayer(ctx.Guild.Id);
        }
    }
}