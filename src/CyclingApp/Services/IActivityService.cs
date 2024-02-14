using CyclingApp.Models;

namespace CyclingApp.Services;

/// <summary>
/// Service to manage activities.
/// </summary>
public interface IActivityService
{
    /// <summary>
    /// Get all activities.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for a cancellation request.</param>
    /// <returns>An enumerator that provides asynchronous iteration.</returns>
    /// <exception cref="TaskCanceledException">
    /// The task has been canceled by the provided <paramref name="cancellationToken"/>.
    /// </exception>
    IAsyncEnumerable<Activity> GetActivitiesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the activity with the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The id of the activity to return.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for a cancellation request.</param>
    /// <returns>The activity with the specified <paramref name="id"/>.</returns>
    /// <exception cref="TaskCanceledException">
    /// The task has been canceled by the provided <paramref name="cancellationToken"/>.
    /// </exception>
    Task<Activity> GetActivityAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new activity.
    /// </summary>
    /// <param name="creationTime">The creation time of the activity.</param>
    /// <param name="duration">The duration of the activity.</param>
    /// <param name="route">The route of the activity.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for a cancellation request.</param>
    /// <returns>
    /// The created activity.
    /// </returns>
    /// <exception cref="TaskCanceledException">
    /// The task has been canceled by the provided <paramref name="cancellationToken"/>.
    /// </exception>
    Task<Activity> CreateActivityAsync(DateTime creationTime, TimeSpan duration, ReadOnlyRoute route,
                                        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete the specified activity.
    /// </summary>
    /// <param name="id">The id pf the activity to delete.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for a cancellation request.</param>
    /// <returns>
    /// <see langword="true"/> if the activity> is successfully removed; otherwise, <see langword="false"/>. 
    /// This method also returns <see langword="false"/> if item was not found.
    /// </returns>
    /// <exception cref="TaskCanceledException">
    /// The task has been canceled by the provided <paramref name="cancellationToken"/>.
    /// </exception>
    Task DeleteActivityAsync(Guid id, CancellationToken cancellationToken = default);
}