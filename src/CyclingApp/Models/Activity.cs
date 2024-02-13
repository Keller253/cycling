namespace CyclingApp.Models;

/// <summary>
/// A cycling activity.
/// </summary>
public class Activity
{
    /// <summary>
    /// Unique identifier of the activity.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Creation time of the activity.
    /// </summary>
    public DateTime CreationTime { get; }

    /// <summary>
    /// Duration of the activity.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Route of the activity.
    /// </summary>
    public ReadOnlyRoute Route { get; }

    /// <summary>
    /// Average speed during the activity in kilometers per hour.
    /// </summary>
    public double AvgSpeed { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="Activity"/> class.
    /// </summary>
    public Activity(Guid id, DateTime creationTime, TimeSpan duration, ReadOnlyRoute route)
    {
        Id = id;
        CreationTime = creationTime;
        Duration = duration;
        Route = route;
        AvgSpeed = route.CalculateAvgSpeed(Duration);
    }
}
