﻿using System.Diagnostics;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Thinktecture.Blazor.ScreenWakeLock;

namespace CyclingApp.Pages
{
    public partial class Home : IAsyncDisposable
    {
        private readonly Stopwatch _stopWatch = new();
        private readonly Timer _stopWatchWatcher;

        private bool _isPaused = false;
        private GeolocationPosition? _currentPosition;
        private GeolocationPositionError? _positionError;
        private TimeSpan _currentTime;
        private double _watchId;

        [Inject]
        protected IGeolocationService GeolocationService { get; set; } = default!;

        [Inject]
        protected IScreenWakeLockService ScreenWakeLockService { get; set; } = default!;

        public Home()
        {
            _stopWatchWatcher = new(WatchStopWatch);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender is false)
            {
                return;
            }

            _watchId = await GeolocationService.WatchPositionAsync(
                this,
                nameof(HandlePositionChanged),
                nameof(HandlePositionError),
                new PositionOptions
                {
                    EnableHighAccuracy = true,
                    MaximumAge = 0,
                    Timeout = 15_000
                });
        }

        [JSInvokable]
        public void HandlePositionChanged(GeolocationPosition pos)
        {
            _currentPosition = pos;
            _positionError = null;
            StateHasChanged();
        }

        [JSInvokable]
        public void HandlePositionError(GeolocationPositionError err)
        {
            _currentPosition = null;
            _positionError = err;
            StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            await GeolocationService.ClearWatchAsync(_watchId);
            await _stopWatchWatcher.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        private void Start()
        {
            if (_isPaused)
            {
                throw new InvalidOperationException("Cannot start while paused");
            }

            _ = ScreenWakeLockService.RequestWakeLockAsync();

            _stopWatch.Restart();

            _ = _stopWatchWatcher.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void Resume()
        {
            if (!_isPaused)
            {
                throw new InvalidOperationException("Cannot resume when not paused");
            }

            _stopWatch.Start();
            _isPaused = false;

            _ = _stopWatchWatcher.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void Pause()
        {
            _stopWatch.Stop();
            _ = _stopWatchWatcher.Change(Timeout.Infinite, Timeout.Infinite);
            _isPaused = true;
        }

        private void Stop()
        {
            _stopWatch.Stop();
            _ = _stopWatchWatcher.Change(Timeout.Infinite, Timeout.Infinite);
            _isPaused = false;

            _ = ScreenWakeLockService.RequestWakeLockAsync();
        }

        private void WatchStopWatch(object? state)
        {
            _currentTime = _stopWatch.Elapsed;
            StateHasChanged();
        }
    }
}
