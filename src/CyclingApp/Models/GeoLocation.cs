namespace CyclingApp.Models;

/// <summary>
/// Position and altitude on earth.
/// </summary>
public class GeoLocation
{
    /// <summary>
    /// Longitude in decimal degrees.
    /// </summary>
    public double Longitude { get; }

    /// <summary>
    /// Latitude in decimal degrees.
    /// </summary>
    public double Latitude { get; }

    /// <summary>
    /// Accuracy of the <see cref="Latitude"/> and <see cref="Longitude"/> properties, expressed in meters.
    /// </summary>
    public double Accuracy { get; }

    /// <summary>
    /// Altitude in meters, relative to nominal sea level.
    /// </summary>
    /// <value>
    /// Can be <see langword="null"/> if the location source cannot provide the data.
    /// </value>
    public double? Altitude { get; }

    /// <summary>
    /// Accuracy of the altitude expressed in meters.
    /// </summary>
    /// <value>
    /// Can be <see langword="null"/> if the original data source cannot provide the data.
    /// </value>
    public double? AltitudeAccuracy { get; }

    /// <summary>
    /// Direction towards which the device is facing. This value, specified in degrees, indicates how far off from 
    /// heading true north the device is. 0 degrees represents true north, and the direction is determined clockwise
    /// (which means that east is 90 degrees and west is 270 degrees). If speed is 0, <see cref="Heading"/> is 
    /// <see cref="double.NaN"/>.
    /// </summary>
    /// <value>
    /// Can be <see langword="null"/> if the original data source cannot provide the data.
    /// </value>
    public double? Heading { get; }

    /// <summary>
    /// Velocity of the device in meters per second.
    /// </summary>
    /// <value>
    /// Can be <see langword="null"/> if the original data source cannot provide the data.
    /// </value>
    public double? Speed { get; }

    /// <summary>
    /// Time at which the location was retrieved.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="GeoLocation"/> class.
    /// </summary>
    public GeoLocation(DateTime timestamp, double longitude, double latitude, double accuracy,
            double? altitude, double? altitudeAccuracy, double? heading, double? speed)
    {
        Timestamp = timestamp;
        Longitude = longitude;
        Latitude = latitude;
        Accuracy = accuracy;
        Altitude = altitude;
        AltitudeAccuracy = altitudeAccuracy;
        Heading = heading;
        Speed = speed;
    }
}
