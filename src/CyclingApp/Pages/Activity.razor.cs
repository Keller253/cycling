using CyclingApp.Services;
using Microsoft.AspNetCore.Components;

namespace CyclingApp.Pages;

/// <summary>
/// Page that displays an <see cref="Activity"/>.
/// </summary>
public partial class Activity
{
    /// <summary>
    /// Route of the page.
    /// </summary>
    /// <remarks>
    /// Requires an additional activity ID as parameter.
    /// </remarks>
    public const string Route = "/Activity";

    private Models.Activity? _activity;

    /// <summary>
    /// ID of the activity to show.
    /// </summary>
    [Parameter]
    public Guid Id { get; set; }

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
    protected override void OnParametersSet()
    {
        _activity = ActivityService.Activities.FirstOrDefault(x => x.Id == Id);
    }

    private void NavigateToHome()
    {
        NavigationManager.NavigateTo(Home.Route);
    }
}
