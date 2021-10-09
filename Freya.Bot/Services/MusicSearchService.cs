using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Lavalink;

namespace Freya.Bot.Services
{
    public class MusicSearchService
    {
        private readonly LavalinkNodeConnection _node;
        // private const string YouTubeUrlPattern = @"^(https?\:\/\/)?(www\.youtube\.com|youtu\.?be)\/.+$";
        
        public MusicSearchService(DiscordClient client)
        {
            _node = client.GetLavalink().GetIdealNodeConnection();
        }

        public async Task<LavalinkTrack> SearchMusic(string query)
        {
            return Uri.IsWellFormedUriString(query, UriKind.RelativeOrAbsolute) ? 
                _node.Rest.GetTracksAsync(new Uri(query)).Result.Tracks.FirstOrDefault() : null;
        }
    }
}