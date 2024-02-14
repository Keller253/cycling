using CyclingApp.Models;

namespace CyclingApp.DTOs;

/// <summary>
/// DTO to store an <see cref="Activity"/>.
/// </summary>
internal class StoreActivityDto
{
    /// <inheritdoc cref="Activity.CreationTime"/>
    public DateTime CreationTime { get; set; }

    /// <inheritdoc cref="Activity.Duration"/>
    public TimeSpan Duration { get; set; }

    /// <inheritdoc cref="Activity.Route"/>
    public StoreGeoLocationDto[] Route { get; set; } = [];
}
