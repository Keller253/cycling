using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using CyclingApp.Models;
using CyclingApp.Resources;
using Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CyclingApp.Components;

/// <summary>
/// Component visualizing a given route.
/// </summary>
public partial class RouteMap : IDisposable
{
    private Canvas2DContext _context = default!;
    private IDisposable _containerResizeObserver = default!;
    private bool _isRendered;

    /// <summary>
    /// Route to be visualized by the component;
    /// </summary>
    [Parameter]
    public RouteBase? Route { get; set; } = new Route();

    /// <summary>
    /// HTML style attribute.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// HTML class attribute.
    /// </summary>
    [Parameter]
    public string Class { get; set; } = string.Empty;

    /// <summary>
    /// Reference to the components container.
    /// </summary>
    protected ElementReference ContainerReference { get; private set; } = default!;

    /// <summary>
    /// Reference to the underlying canvas.
    /// </summary>
    protected BECanvasComponent CanvasReference { get; private set; } = default!;

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _context = await CanvasReference.CreateCanvas2DAsync();

            await DrawCanvas();
            _containerResizeObserver = await CreateResizeObserverAsync(ContainerReference, OnContainerResize);
        }
        _isRendered = true;
    }

    /// <inheritdoc/>
    protected override async Task OnParametersSetAsync()
    {
        if (_isRendered)
        {
            await DrawCanvas();
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _containerResizeObserver.Dispose();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Callback to be invoked when the <see cref="ContainerReference"/> resizes.
    /// </summary>
    [JSInvokable]
    public async void OnContainerResize()
    {
        await DrawCanvas();
    }

    private async Task DrawCanvas()
    {
        // Clear canvas
        await _context.ClearRectAsync(0, 0, CanvasReference.Width, CanvasReference.Height);

        // Resize canvas
        var size = await GetBoundingRectangleAsync(ContainerReference);
        await CanvasReference.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            { nameof(CanvasReference.Width), (long)size.Width },
            { nameof(CanvasReference.Height), (long)size.Height }
        }));

        var centerX = size.Width / 2;
        var centerY = size.Height / 2;
        var scale = Math.Min(size.Width, size.Height) / 650;    // Scale to always show at least 650m

        // Draw background circles
        await _context.SetLineWidthAsync(1);
        await _context.SetStrokeStyleAsync(Themes.Default.PaletteDark.Surface.Value);
        var stepSize = 100 * scale;
        for (var i = stepSize; i < Math.Max(size.Width, size.Height + stepSize); i += stepSize)
        {
            await _context.BeginPathAsync();
            await _context.ArcAsync(centerX, centerY, i, 0, 2 * Math.PI);
            await _context.StrokeAsync();
        }

        if (Route is null)
            return;

        // Draw route
        await _context.SetStrokeStyleAsync(Themes.Default.PaletteDark.Primary.Value);
        await _context.SetFillStyleAsync(Themes.Default.PaletteDark.Primary.Value);
        // Current
        await _context.BeginPathAsync();
        await _context.ArcAsync(centerX, centerY, 8, 0, 2 * Math.PI);
        await _context.FillAsync();
        // Waypoints
        if (Route.Waypoints.Count > 0)
        {
            await _context.SetLineWidthAsync(4);
            await _context.BeginPathAsync();
            await _context.MoveToAsync(centerX, centerY);

            var waypoints = Route.Waypoints.OrderByDescending(x => x.Timestamp).ToArray();
            var previousX = centerX;
            var previousY = centerY;

            for (var i = 1; i < waypoints.Length; i++)
            {
                var previous = waypoints[i - 1];
                var current = waypoints[i];

                var bearing = GeoCalculator.GetBearing(previous.Latitude, previous.Longitude,
                                                        current.Latitude, current.Longitude);
                var distance = GeoCalculator.GetDistance(previous.Latitude, previous.Longitude,
                                                  current.Latitude, current.Longitude,
                                                  0, DistanceUnit.Meters);
                var distanceScaled = distance * scale;
                var bearingTransformed = bearing + -90;
                var bearingRad = (Math.PI / 180) * bearingTransformed;
                var x = distanceScaled * Math.Cos(bearingRad);
                var y = distanceScaled * Math.Sin(bearingRad);
                previousX += x;
                previousY += y;
                await _context.LineToAsync(previousX, previousY);
            }

            await _context.StrokeAsync();
        }
    }
}
