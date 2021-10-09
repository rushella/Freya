using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using Freya.Bot.Services;

namespace Freya.Bot.Commands
{
    public class AudioModule : BaseCommandModule
    {
        private readonly MusicCluster _musicCluster;
        private readonly MusicSearchService _musicSearchService;

        public AudioModule(MusicCluster musicCluster, MusicSearchService musicSearchService)
        {
            _musicCluster = musicCluster;
            _musicSearchService = musicSearchService;
        }
        
        [Command, Aliases("j")]
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
            var node = lava.ConnectedNodes.Values.First();
            
            await player.Connect(node, channel);
            await ctx.RespondAsync($"Joined {channel.Name}!");
        }

        [Command, Aliases("p")]
        public async Task Play(CommandContext ctx, string query)
        {
            var track = await _musicSearchService.SearchMusic(query);
            var player = await _musicCluster.GetOrRegisterPlayer(ctx.Guild.Id);

            await player.Play(track);
        }
        
        [Command, Aliases("n")]
        public async Task Next(CommandContext ctx)
        {
            var player = await _musicCluster.GetOrRegisterPlayer(ctx.Guild.Id);

            await player.PlayNext();
        }
        
        [Command, Aliases("s")]
        public async Task Stop(CommandContext ctx)
        {
            var player = await _musicCluster.GetOrRegisterPlayer(ctx.Guild.Id);

            await player.Stop();
        }
        
        [Command, Aliases("l")]
        public async Task Leave(CommandContext ctx)
        {
            var player = await _musicCluster.GetOrRegisterPlayer(ctx.Guild.Id);
            
            await player.Disconnect();
            await _musicCluster.UnregisterPlayer(ctx.Guild.Id);
        }
        
        [Command, Aliases("q")]
        public async Task Queue(CommandContext ctx)
        {
            var player = await _musicCluster.GetOrRegisterPlayer(ctx.Guild.Id);

            var description = new StringBuilder();

            for (var i = 0; i < player.Queue.Count; i++)
            {
                var track = player.Queue[i];
                
                description.Append($"[{i + 1}] {track.Author} - {track.Title}\n");
            }
            
            var embed = new DiscordEmbedBuilder().WithDescription(description.ToString()).Build();

            await ctx.RespondAsync(embed);
        }
    }
}