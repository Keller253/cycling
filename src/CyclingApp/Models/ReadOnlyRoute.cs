namespace CyclingApp.Models;

/// <summary>
/// A read-only route.
/// </summary>
/// <param name="waypoints">The waypoints of the route.</param>
public class ReadOnlyRoute(IList<GeoLocation> waypoints) : RouteBase(waypoints)
{ }
