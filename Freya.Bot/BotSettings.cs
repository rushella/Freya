using System;

namespace Freya.Bot
{
    public class BotSettings
    {
        public string DiscordBotToken { get; }

        private Settings()
        {
            DiscordBotToken = Environment.GetEnvironmentVariable("FREYA_DISCORDBOT_TOKEN");
        }

        public static BotSettings Initialize()
        {
            return new Settings();
        }
    }
}