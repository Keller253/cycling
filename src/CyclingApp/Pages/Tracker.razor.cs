using System.Diagnostics;
using CyclingApp.Models;
using CyclingApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Thinktecture.Blazor.ScreenWakeLock;

namespace CyclingApp.Pages;

/// <summary>
/// Page to track a new activity.
/// </summary>
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
    private DateTime _creationTime;

    /// <summary>
    /// Service to get an activity.
    /// </summary>
    [Inject]
    protected IActivityService ActivityService { get; set; } = default!;

    /// <summary>
    /// Service to interact with the Geolocation Api.
    /// </summary>
    [Inject]
    protected IGeolocationService GeolocationService { get; set; } = default!;

    /// <summary>
    /// Service to interact with the Screen Wake Lock Api.
    /// </summary>
    [Inject]
    protected IScreenWakeLockService ScreenWakeLockService { get; set; } = default!;

    /// <summary>
    /// Manager to navigate the app.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Tracker"/> class.
    /// </summary>
    public Tracker()
    {
        _stopWatchWatcher = new(WatchStopWatch);
        _route = new();
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Handles a new position received from the Geolocation Api.
    /// </summary>
    // Required to be public in order to be invokable from JavaScript.
    [JSInvokable]
    public void HandlePositionChanged(GeolocationPosition pos)
    {
        if (_stopWatch.IsRunning)
        {
            var waypoint = new GeoLocation(pos.TimestampAsUtcDateTime, pos.Coords.Latitude, pos.Coords.Longitude,
                                pos.Coords.Accuracy, pos.Coords.Altitude, pos.Coords.AltitudeAccuracy,
                                pos.Coords.Heading, pos.Coords.Speed);
            _route.AddWaypoint(waypoint);
        }
        _positionError = null;
        StateHasChanged();
    }

    /// <summary>
    /// Handles a position error received from the Geolocation Api.
    /// </summary>
    // Required to be public in order to be invokable from JavaScript.
    [JSInvokable]
    public void HandlePositionError(GeolocationPositionError err)
    {
        _positionError = err;
        StateHasChanged();
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await GeolocationService.ClearWatchAsync(_watchId);
        await _stopWatchWatcher.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    private void NavigateToHome()
    {
        NavigationManager.NavigateTo($".{Home.Route}");
    }

    private void Start()
    {
        if (_isPaused)
        {
            throw new InvalidOperationException("Cannot start while paused");
        }

        //_ = ScreenWakeLockService.RequestWakeLockAsync();

        _route = new();
        _creationTime = DateTime.UtcNow;
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
        WatchStopWatch(null);
        _isPaused = true;
    }

    private void Stop()
    {
        _stopWatch.Stop();
        _ = _stopWatchWatcher.Change(Timeout.Infinite, Timeout.Infinite);
        WatchStopWatch(null);
        _isPaused = false;

        //_ = ScreenWakeLockService.RequestWakeLockAsync();

        _ = Task.Run(async () =>
        {
            var route = new ReadOnlyRoute(_route.Waypoints);
            var activity = await ActivityService.CreateActivityAsync(_creationTime, _stopWatch.Elapsed, route);
            NavigationManager.NavigateTo($".{Activity.Route}/{activity.Id}");
        });
    }

    private void WatchStopWatch(object? state)
    {
        _currentTime = _stopWatch.Elapsed;
        StateHasChanged();
    }
}
