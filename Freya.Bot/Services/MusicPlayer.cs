using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using Emzi0767.Utilities;

namespace Freya.Bot.Services
{
    public class MusicPlayer
    {
        private readonly List<LavalinkTrack> _internalQueue;
        private LavalinkGuildConnection _lavaGuildConnection;

        public IReadOnlyList<LavalinkTrack> Queue => _internalQueue.AsReadOnly();
        public int CurrentAudioPosition { get; private set; }
        
        public event AsyncEventHandler<object, AsyncEventArgs> QueueEnded;
        
        public MusicPlayer()
        {
            _internalQueue = new List<LavalinkTrack>();
        }
        
        public async Task Connect(LavalinkNodeConnection lava, DiscordChannel channel)
        {
            _lavaGuildConnection = await lava.ConnectAsync(channel);
            _lavaGuildConnection.PlaybackFinished += async (conn, args) => await PlayNext();
        }

        public async Task Disconnect()
        {
            await _lavaGuildConnection.StopAsync();
            await _lavaGuildConnection.DisconnectAsync();
        }
        
        public async Task Play()
        {
            if (!_internalQueue.Any())
            {
                return;
            }
            
            var track = _internalQueue[CurrentAudioPosition];
            
            await _lavaGuildConnection.PlayAsync(track);
        }

        public async Task Pause()
        {
            await _lavaGuildConnection.PauseAsync();
        }

        public async Task Stop()
        {
            await _lavaGuildConnection.StopAsync();
            
        }

        public async Task Resume()
        {
            await _lavaGuildConnection.ResumeAsync();
        }
        
        public async Task PlayNext()
        {
            Dequeue(0);
            
            if (_internalQueue.Count == 0)
            {
                QueueEnded?.Invoke(this, new AsyncEventArgs());
                return;
            }
            
            var track = _internalQueue[CurrentAudioPosition];
            
            await _lavaGuildConnection.PlayAsync(track);
        }

        public void Enqueue(LavalinkTrack track)
        {
            _internalQueue.Add(track);
        }
        
        public void Dequeue(int trackPosition)
        {
            _internalQueue.RemoveAt(trackPosition);
        }
    }
}