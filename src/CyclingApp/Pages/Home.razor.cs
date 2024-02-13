using CyclingApp.Services;
using Microsoft.AspNetCore.Components;

namespace CyclingApp.Pages;

/// <summary>
/// Home page of the app.
/// </summary>
public partial class Home
{
    /// <summary>
    /// Route of the page.
    /// </summary>
    public const string Route = "/";

    /// <summary>
    /// Service to get an activity.
    /// </summary>
    [Inject]
    protected IActivityService ActivityService { get; set; } = default!;

    /// <summary>
    /// Manager to navigate the app.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    private void NavigateToTracker()
    {
        NavigationManager.NavigateTo(Tracker.Route);
    }

    private void NavigateToActivity(Guid id)
    {
        NavigationManager.NavigateTo($"{Activity.Route}/{id}");
    }
}
