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
        private LavalinkGuildConnection _conn;

        public IReadOnlyList<LavalinkTrack> Queue => _internalQueue.AsReadOnly();
        public int CurrentAudioPosition { get; private set; }
        
        public event AsyncEventHandler<object, AsyncEventArgs> QueueEnded;
        
        public MusicPlayer()
        {
            _internalQueue = new List<LavalinkTrack>();
        }
        
        public async Task Connect(LavalinkNodeConnection node, DiscordChannel channel)
        {
            CurrentAudioPosition = -1;
            _conn = await node.ConnectAsync(channel);
            _conn.PlaybackFinished += async (conn, args) =>
            {
                Dequeue(0);
            
                if (_internalQueue.Count == 0)
                {
                    CurrentAudioPosition = -1;
                    QueueEnded?.Invoke(this, new AsyncEventArgs());
                    return;
                }
            
                var track = _internalQueue[CurrentAudioPosition];
            
                await _conn.PlayAsync(track);
            };
        }

        public async Task Disconnect()
        {
            await _conn.StopAsync();
            await _conn.DisconnectAsync();
        }
        
        public async Task Play(LavalinkTrack track = null)
        {
            if (track != null)
            {
                Enqueue(track);
            }
            
            if (_internalQueue.Any() && CurrentAudioPosition == -1)
            {
                CurrentAudioPosition = 0;
                var nextTrack = _internalQueue[CurrentAudioPosition];
                await _conn.PlayAsync(nextTrack);
            }
        }

        public async Task Pause()
        {
            await _conn.PauseAsync();
        }

        public async Task Stop()
        {
            await _conn.StopAsync();
            _internalQueue.Clear();
        }

        public async Task Resume()
        {
            await _conn.ResumeAsync();
        }
        
        public async Task PlayNext()
        {
            await _conn.StopAsync();
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