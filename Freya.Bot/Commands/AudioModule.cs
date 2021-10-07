using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Freya.Bot.Commands
{
    public class MusicModule : BaseCommandModule
    {
        [Command("play")]
        public async Task Play(CommandContext ctx)
        {
            await ctx.RespondAsync("bruhhhh");
        }
    }
}