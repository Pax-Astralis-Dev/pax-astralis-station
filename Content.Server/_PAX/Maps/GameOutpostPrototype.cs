using System.Diagnostics;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._PAX.Maps;

/// <summary>
/// Prototype data for a station, because all station are GamMaps this is just a wrapper around it. This lets you define color, location.
/// </summary>
[Prototype("sectorOutpost"), PublicAPI]
[DebuggerDisplay("Outpost [{Name}]")]
public sealed partial class GameOutpostPrototype : IPrototype
{

    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Configuration on where this outpost should be spawned
    /// </summary>
    [DataField("location", required: true)]
    public Location Location { get; private set; } = new(0);

    /// <summary>
    /// Configuration on where this map should be spawned
    /// </summary>
    [DataField("path", required: true)]
    public ResPath Path { get; private set; } = default!;

}
