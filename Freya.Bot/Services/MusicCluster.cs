using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freya.Bot.Services
{
    public class MusicCluster
    {
        private readonly Dictionary<ulong, MusicPlayer> _players;

        public MusicCluster()
        {
            _players = new Dictionary<ulong, MusicPlayer>();
        }
        
        public async Task<MusicPlayer> GetOrRegisterPlayer(ulong guildId)
        {
            var player = new MusicPlayer();

            if (_players.Any(x => x.Key == guildId))
            {
                return _players[guildId];
            }
            
            _players.Add(guildId, player);

            return player;
        }

        public async Task UnregisterPlayer(ulong guildId)
        {
            _players.Remove(guildId);
        }
    }
}