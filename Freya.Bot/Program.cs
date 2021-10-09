using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using Freya.Bot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Freya.Bot
{
    public static class Program
    {
        private static void Main()
        {
            MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var services = ConfigureServices();

            var discord = services.GetRequiredService<DiscordClient>();
            var lavaConfig = services.GetRequiredService<LavalinkConfiguration>();
            var botConfig = services.GetRequiredService<IConfiguration>();

            var lava = discord.UseLavalink();
            
            await discord.ConnectAsync();
            await lava.ConnectAsync(lavaConfig);
            
            var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                Services = services,
                StringPrefixes = new []{ botConfig["prefix"] }
            });
            
            commands.RegisterCommands(Assembly.GetExecutingAssembly());
            
            await Task.Delay(Timeout.Infinite);
        }

        private static IServiceProvider ConfigureServices()
        {
            var endpoint = new ConnectionEndpoint
            {
                Hostname = "127.0.0.1",
                Port = 2333
            };

            var lavaConfig = new LavalinkConfiguration
            {
                Password = "youshallnotpass",
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };

            var botConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();
            
            var botSettings = BotSettings.Load();
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(botConfig)
                .AddSingleton(new DiscordClient(
                    new DiscordConfiguration 
                    {
                        TokenType = TokenType.Bot, 
                        Token = botSettings.DiscordBotToken,
                        MinimumLogLevel = LogLevel.Debug
                    }))
                .AddSingleton(lavaConfig)
                .AddSingleton<MusicSearchService>()
                .AddSingleton<MusicCluster>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    } 
}