using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CyclingApp.Models;

namespace CyclingApp.Services;

/// <summary>
/// Service to manage activities.
/// </summary>
public interface IActivityService
{
    /// <summary>
    /// All activities available.
    /// </summary>
    ReadOnlyObservableCollection<Activity> Activities { get; }

    /// <summary>
    /// Create a new activity.
    /// </summary>
    /// <param name="creationTime">The creation time of the activity.</param>
    /// <param name="duration">The duration of the activity.</param>
    /// <param name="route">The route of the activity.</param>
    /// <returns></returns>
    Activity CreateActivity(DateTime creationTime, TimeSpan duration, ReadOnlyRoute route);

    bool TryDeleteActivity(Guid id, [NotNullWhen(true)] out Activity? activity);
}