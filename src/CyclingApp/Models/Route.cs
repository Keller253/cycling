namespace CyclingApp.Models
{
    /// <summary>
    /// An editable route.
    /// </summary>
    public class Route() : RouteBase([])
    {
        /// <summary>
        /// Add a waypoint to the route.
        /// </summary>
        /// <param name="waypoint">The waypoint to add.</param>
        public new void AddWaypoint(GeoLocation waypoint)
        {
            base.AddWaypoint(waypoint);
        }

        /// <summary>
        /// Remove waypoint from the route.
        /// </summary>
        /// <param name="waypoint">The waypoint to remove.</param>
        public new void RemoveWaypoint(GeoLocation waypoint)
        {
            base.RemoveWaypoint(waypoint);
        }
    }
}
