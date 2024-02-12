using System.Diagnostics;

using CyclingApp.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CyclingApp.Pages
{
    public partial class Tracker : IAsyncDisposable
    {
        /// <summary>
        /// Route of the page.
        /// </summary>
        public const string Route = "/tracker";

        private readonly Stopwatch _stopWatch = new();
        private readonly Timer _stopWatchWatcher;

        private bool _isPaused = false;
        private GeolocationPositionError? _positionError;
        private TimeSpan _currentTime;
        private double _watchId;
        private Route _route;

        [Inject]
        protected IGeolocationService GeolocationService { get; set; } = default!;

        //[Inject]
        //protected IScreenWakeLockService ScreenWakeLockService { get; set; } = default

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        public Tracker()
        {
            _stopWatchWatcher = new(WatchStopWatch);
            _route = new();
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
            if (_stopWatch.IsRunning)
            {
                var waypoint = new GeoLocation(pos.TimestampAsUtcDateTime, pos.Coords.Longitude, pos.Coords.Latitude,
                                    pos.Coords.Accuracy, pos.Coords.Altitude, pos.Coords.AltitudeAccuracy,
                                    pos.Coords.Heading, pos.Coords.Speed);
                _route.AddWaypoint(waypoint);
            }
            _positionError = null;
            StateHasChanged();
        }

        [JSInvokable]
        public void HandlePositionError(GeolocationPositionError err)
        {
            _positionError = err;
            StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            await GeolocationService.ClearWatchAsync(_watchId);
            await _stopWatchWatcher.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        private void NavigateToHome()
        {
            NavigationManager.NavigateTo(Home.Route);
        }

        private void Start()
        {
            if (_isPaused)
            {
                throw new InvalidOperationException("Cannot start while paused");
            }

            //_ = ScreenWakeLockService.RequestWakeLockAsync();

            _route = new();
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

            //_ = ScreenWakeLockService.RequestWakeLockAsync();
        }

        private void WatchStopWatch(object? state)
        {
            _currentTime = _stopWatch.Elapsed;
            StateHasChanged();
        }
    }
}
