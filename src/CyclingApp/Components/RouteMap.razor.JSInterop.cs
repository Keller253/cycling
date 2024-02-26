using System.Runtime.Versioning;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CyclingApp.Components;

[SupportedOSPlatform("browser")]
public partial class RouteMap : IAsyncDisposable
{
    private IJSObjectReference? _jSModule;

    /// <summary>
    /// TODO
    /// </summary>
    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    private async Task<ElementBoundingRectangle> GetBoundingRectangleAsync(ElementReference element)
    {
        var module = await GetOrImportJSModule();
        return await module.InvokeAsync<ElementBoundingRectangle>("getBoundingRectangle", element);
    }

    private async Task<IDisposable> CreateResizeObserverAsync(ElementReference element, Action callback)
    {
        var target = callback.Target ?? throw new NotSupportedException("Static methods are not supported");
        var targetReference = DotNetObjectReference.Create(target);
        var module = await GetOrImportJSModule();
        await module.InvokeVoidAsync("createResizeObserver", element, targetReference, callback.Method.Name);
        return targetReference;
    }

    private async Task<IJSObjectReference> GetOrImportJSModule()
    {
        _jSModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/RouteMap.razor.js");
        return _jSModule;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_jSModule is not null)
        {
            await _jSModule.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}

[SupportedOSPlatform("browser")]
public class ElementBoundingRectangle
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Top { get; set; }
    public double Right { get; set; }
    public double Bottom { get; set; }
    public double Left { get; set; }
}
