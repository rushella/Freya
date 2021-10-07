using System;

namespace Freya.Bot
{
    public class BotSettings
    {
        public string DiscordBotToken { get; }

        private BotSettings()
        {
            DiscordBotToken = Environment.GetEnvironmentVariable("FREYA_DISCORDBOT_TOKEN", EnvironmentVariableTarget.User);
        }

        public static BotSettings Load()
        {
            return new BotSettings();
        }
    }
}