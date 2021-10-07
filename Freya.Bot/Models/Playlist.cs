using System.Collections.Generic;
using DSharpPlus.Entities;

namespace Freya.Bot.Models
{
    public class Playlist
    {
        public List<AudioInfo> Audios { get; set; }
        public DiscordUser CreatedBy { get; set; }
    }
}