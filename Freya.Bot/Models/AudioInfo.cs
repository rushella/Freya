using DSharpPlus.Entities;

namespace Freya.Bot.Models
{
    public class AudioInfo
    {
        public AudioSourceType Type { get; set; }
        public string Source { get; set; }
        public DiscordUser AddedBy { get; set; }
    }
}