using CyclingApp.Models;

namespace CyclingApp.DTOs;

/// <summary>
/// Contains convert extensions for DTOs.
/// </summary>
internal static class ConvertExtensions
{
    /// <summary>
    /// Create a <see cref="GeoLocation"/> model from the DTO.
    /// </summary>
    /// <param name="dto">The DTO.</param>
    /// <returns>The created model.</returns>
    public static GeoLocation ToModel(this StoreGeoLocationDto dto)
    {
        return new GeoLocation(dto.Timestamp, dto.Longitude, dto.Latitude, dto.Accuracy, dto.Altitude,
                                dto.AltitudeAccuracy, dto.Heading, dto.Speed);
    }

    /// <summary>
    /// Create a <see cref="StoreGeoLocationDto"/> from the model.
    /// </summary>
    /// <param name="geoLocation">The model.</param>
    /// <returns>THe created DTO.</returns>
    public static StoreGeoLocationDto ToDto(this GeoLocation geoLocation)
    {
        return new StoreGeoLocationDto
        {
            Longitude = geoLocation.Longitude,
            Latitude = geoLocation.Latitude,
            Accuracy = geoLocation.Accuracy,
            Altitude = geoLocation.Altitude,
            AltitudeAccuracy = geoLocation.AltitudeAccuracy,
            Heading = geoLocation.Heading,
            Speed = geoLocation.Speed,
            Timestamp = geoLocation.Timestamp,
        };
    }

    /// <summary>
    /// Create a <see cref="GeoLocation"/> model from the DTO.
    /// </summary>
    /// <param name="dto">The DTO.</param>
    /// <param name="id">The id of the activity to create.</param>
    /// <returns>The created model.</returns>
    public static Activity ToModel(this StoreActivityDto dto, Guid id)
    {
        var waypoints = dto.Route.Select(x => x.ToModel()).ToList();
        var route = new ReadOnlyRoute(waypoints);
        return new Activity(id, dto.CreationTime, dto.Duration, route);
    }

    /// <summary>
    /// Create a <see cref="StoreActivityDto"/> from the model.
    /// </summary>
    /// <param name="activity">The model.</param>
    /// <returns>THe created DTO.</returns>
    public static StoreActivityDto ToDto(this Activity activity)
    {
        return new StoreActivityDto
        {
            CreationTime = activity.CreationTime,
            Duration = activity.Duration,
            Route = activity.Route.Waypoints.Select(x => x.ToDto()).ToArray()
        };
    }
}
