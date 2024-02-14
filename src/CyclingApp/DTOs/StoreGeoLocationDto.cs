using CyclingApp.Models;

namespace CyclingApp.DTOs;

/// <summary>
/// DTO to store a <see cref="GeoLocation"/>.
/// </summary>
internal class StoreGeoLocationDto
{
    /// <inheritdoc cref="GeoLocation.Longitude"/>
    public double Longitude { get; set; }

    /// <inheritdoc cref="GeoLocation.Latitude"/>
    public double Latitude { get; set; }

    /// <inheritdoc cref="GeoLocation.Accuracy"/>
    public double Accuracy { get; set; }

    /// <inheritdoc cref="GeoLocation.Altitude"/>
    public double? Altitude { get; set; }

    /// <inheritdoc cref="GeoLocation.AltitudeAccuracy"/>
    public double? AltitudeAccuracy { get; set; }

    /// <inheritdoc cref="GeoLocation.Heading"/>
    public double? Heading { get; set; }

    /// <inheritdoc cref="GeoLocation.Speed"/>
    public double? Speed { get; set; }

    /// <inheritdoc cref="GeoLocation.Timestamp"/>
    public DateTime Timestamp { get; set; }
}
