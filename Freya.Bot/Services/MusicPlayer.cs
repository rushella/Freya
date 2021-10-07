using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;

namespace Freya.Bot.Services
{
    public class MusicPlayer
    {
        private LavalinkGuildConnection _lavaConnection;
        private List<LavalinkTrack> _internalQueue;
        
        public IReadOnlyList<LavalinkTrack> Queue => _internalQueue.AsReadOnly();
        public int CurrentAudioPosition { get; private set; }
        
        public delegate void QueueEndedHandler(object sender, EventArgs e);
        public event QueueEndedHandler QueueEnded;

        public async Task Join(LavalinkGuildConnection lavaConnection, DiscordChannel channel)
        {
            _lavaConnection = lavaConnection;
            _lavaConnection.PlaybackFinished += async (conn, args) => await PlayNext();
            _internalQueue = new List<LavalinkTrack>();
        }

        public async Task Leave()
        {
            
        }
        
        public async Task Play()
        {
            if (!_internalQueue.Any())
            {
                return;
            }
            
            var track = _internalQueue[CurrentAudioPosition];
            
            await _lavaConnection.PlayAsync(track);
        }

        public async Task Pause()
        {
            await _lavaConnection.PauseAsync();
        }
        
        public async Task Resume()
        {
            await _lavaConnection.ResumeAsync();
        }
        
        public async Task PlayNext()
        {
            Dequeue(0);
            
            if (_internalQueue.Count == 0)
            {
                QueueEnded?.Invoke(this, EventArgs.Empty);
                return;
            }
            
            var track = _internalQueue[CurrentAudioPosition];
            
            await _lavaConnection.PlayAsync(track);
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