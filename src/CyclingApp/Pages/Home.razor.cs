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

    private readonly List<Models.Activity> _activities = [];

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

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await foreach (var activity in ActivityService.GetActivitiesAsync())
        {
            _activities.Add(activity);
        }
    }

    private void NavigateToTracker()
    {
        NavigationManager.NavigateTo(Tracker.Route);
    }

    private void NavigateToActivity(Guid id)
    {
        NavigationManager.NavigateTo($"{Activity.Route}/{id}");
    }
}
