using System.Collections.ObjectModel;
using Geolocation;

namespace CyclingApp.Models;

/// <summary>
/// Base class for a route.
/// </summary>
public abstract class RouteBase
{
    private readonly List<GeoLocation> _waypoints;

    /// <summary>
    /// Waypoints of the route.
    /// </summary>
    public ReadOnlyCollection<GeoLocation> Waypoints { get; }

    /// <summary>
    /// Distance of the route in absolute meters.
    /// </summary>
    public double Distance { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="RouteBase"/> class.
    /// </summary>
    protected RouteBase(IList<GeoLocation> waypoints)
    {
        _waypoints = new(waypoints);
        Waypoints = new(_waypoints);

        Distance = CalculateDistance();
    }

    /// <summary>
    /// Calculate the average speed in kilometers per hour for the route.
    /// </summary>
    /// <param name="duration">The duration it took for the route.</param>
    /// <returns>The average speed in kilometers per hour.</returns>
    public double CalculateAvgSpeed(TimeSpan duration)
    {
        return Distance / 1000 / duration.TotalHours;
    }

    /// <summary>
    /// Add a waypoint to the route.
    /// </summary>
    /// <param name="waypoint">The waypoint to add.</param>
    protected void AddWaypoint(GeoLocation waypoint)
    {
        _waypoints.Add(waypoint);

        Distance = CalculateDistance();
    }

    /// <summary>
    /// Remove waypoint from the route.
    /// </summary>
    /// <param name="waypoint">The waypoint to remove.</param>
    protected void RemoveWaypoint(GeoLocation waypoint)
    {
        _ = _waypoints.Remove(waypoint);

        Distance = CalculateDistance();
    }

    private double CalculateDistance()
    {
        var distance = 0d;

        if (_waypoints.Count < 2)
        {
            return distance;
        }

        var waypoints = _waypoints.OrderBy(x => x.Timestamp).ToList();
        for (var i = 0; i < waypoints.Count - 1; i++)
        {
            var origin = waypoints[i];
            var destination = waypoints[i + 1];

            distance += GeoCalculator.GetDistance(origin.Latitude, origin.Longitude,
                                                  destination.Latitude, destination.Longitude,
                                                  0, DistanceUnit.Meters);
        }

        return distance;
    }
}
