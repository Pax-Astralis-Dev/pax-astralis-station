using System.Diagnostics;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server._PAX.Maps;

/// <summary>
/// Prototype data for a station, because all station are GamMaps this is just a wrapper around it. This lets you define color, location.
/// </summary>
[Prototype("sectorStation"), PublicAPI]
[DebuggerDisplay("Station [{Name}]")]
public sealed partial class GameStationPrototype : IPrototype
{

    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Configuration on where this map should be spawned
    /// </summary>
    [DataField("location", required: true)]
    public Location Location { get; private set; } = new(0);

    /// <summary>
    /// Configuration on where this map should be spawned
    /// </summary>
    [DataField("stationId", required: true)]
    public string StationId { get; private set; } = default!;

    [DataField("stationName", required: true)]
    public string StationName { get; private set; } = default!;
}
